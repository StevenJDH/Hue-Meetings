/*
 * This file is part of Hue Meetings <https://github.com/StevenJDH/Hue-Meetings>.
 * Copyright (C) 2022 Steven Jenkins De Haro.
 *
 * Hue Meetings is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Hue Meetings is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Hue Meetings.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hue_Meetings.Models;

namespace Hue_Meetings.Components.HueColor;

/// <summary>
/// Used to convert colors between XY and RGB.
/// </summary>
/// <remarks>
/// Based on http://www.developers.meethue.com/documentation/color-conversions-rgb-xy
/// </remarks>
public static class HueColorConverter // TODO: maybe add as extension to RGBColor class.
{
    public static Cie1931Point RgbToXy(RgbColor color, Cie1931Gamut? gamut)
    {
        // Apply gamma correction. Convert non-linear RGB color components to linear color intensity levels.
        double r = InverseGamma(color.R);
        double g = InverseGamma(color.G);
        double b = InverseGamma(color.B);

        /*
         * Hue bulbs (depending on the type) can display colors outside the sRGB gamut supported
         * by most computer screens.
         * To make sure all colors are selectable by the user, Philips in its implementation
         * decided to interpret all RGB colors as if they were from a wide (non-sRGB) gamut.
         * The effect of this is to map colors in sRGB to a broader gamut of colors the hue lights
         * can produce. This also results in some deviation of color on screen vs color in real-life.
         *
         * The Philips implementation describes the matrix below with the comment
         * "Wide Gamut D65", but the values suggest this is in fact not a standard
         * gamut but some custom gamut.
         *
         * The coordinates of this gamut have been identified as follows:
         *    red: (0.700607, 0.299301)
         *    green: (0.172416, 0.746797)
         *    blue: (0.135503, 0.039879)
         *
         * Note: substitute r = 1, g = 1, b = 1 in sequence into array below and convert from XYZ
         * to xyY coordinates. The plotted chart can be seen here:
         * https://en.wikipedia.org/wiki/CIE_1931_color_space#/media/File:CIE1931xy_blank.svg
         *
         * Also of interest, the white point is not D65 (0.31271, 0.32902), but a slightly
         * shifted version at (0.322727, 0.32902). This might be because true D65 is slightly
         * outside Gamut B (the position of D65 in the linked chart is slightly inaccurate).
         */
        double x = r * 0.664511f + g * 0.154324f + b * 0.162028f;
        double y = r * 0.283881f + g * 0.668433f + b * 0.047685f;
        double z = r * 0.000088f + g * 0.072310f + b * 0.986039f;

        var xyPoint = new Cie1931Point(0.0, 0.0);

        if ((x + y + z) > 0.0)
        {
            // Convert from CIE XYZ to CIE xyY coordinates.
            xyPoint = new Cie1931Point(x / (x + y + z), y / (x + y + z));
        }

        if (gamut.HasValue)
        {
            // The point, adjusted it to the nearest point that is within the gamut of the lamp, if necessary.
            return gamut.Value.NearestContainedPoint(xyPoint);
        }

        return xyPoint;
    }

    /// <summary>
    /// Returns RGB color from Light device.
    /// </summary>
    /// <param name="lightData"></param>
    /// <param name="gamut"></param>
    /// <returns></returns>
    public static RgbColor RgbFromLight(Datum lightData, Cie1931Gamut? gamut)
    {
        ArgumentNullException.ThrowIfNull(lightData);

        // Off or low brightness.
        if (!lightData.PowerState.IsOn || lightData.Dimming.Brightness <= 5)
        {
            return new RgbColor(0, 0, 0);
        }

        return XyToRgb(new Cie1931Point(lightData.Color.ColorCoordinates.X, lightData.Color.ColorCoordinates.Y), gamut);
    }

    public static RgbColor XyToRgb(Cie1931Point point, Cie1931Gamut? gamut)
    {
        if (gamut.HasValue)
        {
            // If the color is outside the lamp's gamut, adjust to the nearest color
            // inside the lamp's gamut.
            point = gamut.Value.NearestContainedPoint(point);
        }

        var philipsWideGamut = new Cie1931Gamut(
            Red: new Cie1931Point(0.700607, 0.299301),
            Green: new Cie1931Point(0.172416, 0.746797),
            Blue: new Cie1931Point(0.135503, 0.039879)
        );

        // Also adjust it to be in the Philips "Wide Gamut" if not already.
        // The wide gamut used for XYZ->RGB conversion does not quite contain all colors
        // all of the hue bulbs support.
        point = philipsWideGamut.NearestContainedPoint(point);

        // Convert from xyY to XYZ coordinates.
        const double y = 1.0; // Luminance
        double x = (y / point.Y) * point.X;
        double z = (y / point.Y) * point.Z;

        // The Philips implementation comments this matrix with "sRGB D65 conversion"
        // However, this is not the XYZ -> RGB conversion matrix for sRGB. Instead
        // the matrix that is the inverse of that in RgbToXY() is used.
        // See comment in RgbToXY() for more info.
        double r = x * 1.656492 - y * 0.354851 - z * 0.255038;
        double g = -x * 0.707196 + y * 1.655397 + z * 0.036152;
        double b = x * 0.051713 - y * 0.121364 + z * 1.011530;

        // Downscale color components so that largest component has an intensity of 1.0,
        // as we can't display colors brighter than that.
        double maxComponent = Math.Max(Math.Max(r, g), b);

        if (maxComponent > 1.0)
        {
            r /= maxComponent;
            g /= maxComponent;
            b /= maxComponent;
        }

        // We now have the (linear) amounts of R, G and B corresponding to the specified XY coordinates.
        // Since displays are non-linear, we must apply a gamma correction to get the pixel value.
        // For example, a pixel red value of 1.0 (255) is more than twice as bright as 0.5 (127).
        // We need to correct for this non-linearity.
        r = Gamma(r);
        g = Gamma(g);
        b = Gamma(b);

        // Philips applies a second round of downscaling here, but that should be unnecessary given
        // gamma returns a value between 0.0 and 1.0 for every input between 0.0 and 1.0.
        return new RgbColor(r, g, b);
    }

    /// <summary>
    /// Converts a gamma-corrected value (e.g. as used in RGB pixel components) to
    /// a linear color intensity level. All values are between 0.0 and 1.0. Used
    /// when converting to XY chroma coordinates.
    /// </summary>
    private static double InverseGamma(double value)
    {
        double result;

        if (value > 0.04045)
        {
            result = Math.Pow((value + 0.055) / (1.0 + 0.055), 2.4);
        }
        else
        {
            result = value / 12.92;
        }

        // The gamma function returns values between 0.0 and 1.0 for all inputs
        // between 0.0 and 1.0, but in case there is a slight rounding error...
        return Bound(result);
    }

    /// <summary>
    /// Converts a linear color intensity level to a gamma-corrected value for display
    /// on a screen. All values are between 0.0 and 1.0. Used when converting to 'RGB'
    /// pixel outputs.
    /// </summary>
    private static double Gamma(double value)
    {
        double result;

        if (value <= 0.0031308)
        {
            result = 12.92 * value;
        }
        else
        {
            result = (1.0 + 0.055) * Math.Pow(value, (1.0 / 2.4)) - 0.055;
        }

        // The gamma function returns values between 0.0 and 1.0 for all inputs
        // between 0.0 and 1.0, but in case there is a slight rounding error...
        return Bound(result);
    }

    /// <summary>
    /// Bounds the specified value to between 0.0 and 1.0.
    /// </summary>
    private static double Bound(double value)
    {
        return Math.Max(0.0, Math.Min(1.0, value));
    }
}