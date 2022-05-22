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

namespace Hue_Meetings.Components.HueColor;

/// <summary>
/// Represents a color with red, green and blue components.
/// All values are between 0.0 and 1.0.
/// </summary>
public readonly struct RgbColor
{
    public double R { get; }

    public double G { get; }

    public double B { get; }

    /// <summary>
    /// RGB Color
    /// </summary>
    /// <param name="red">Between 0.0 and 1.0 inclusive.</param>
    /// <param name="green">Between 0.0 and 1.0 inclusive.</param>
    /// <param name="blue">Between 0.0 and 1.0 inclusive.</param>
    public RgbColor(double red, double green, double blue)
    {
        R = red switch
        {
            < 0 => 0,
            > 1 => 1,
            _ => red
        };

        G = green switch
        {
            < 0 => 0,
            > 1 => 1,
            _ => green
        };

        B = blue switch
        {
            < 0 => 0,
            > 1 => 1,
            _ => blue
        };
    }

    /// <summary>
    /// RGB Color
    /// </summary>
    /// <param name="red">Between 0 and 255 inclusive.</param>
    /// <param name="green">Between 0 and 255 inclusive.</param>
    /// <param name="blue">Between 0 and 255 inclusive.</param>
    public RgbColor(int red, int green, int blue)
    {
        R = red switch
        {
            < 0 => 0,
            > 255 => 1,
            _ => red / 255.0
        };

        G = green switch
        {
            < 0 => 0,
            > 255 => 1,
            _ => green / 255.0
        };

        B = blue switch
        {
            < 0 => 0,
            > 255 => 1,
            _ => blue / 255.0
        };
    }

    /// <summary>
    /// RGB Color
    /// </summary>
    /// <param name="color">A <see cref="Color"/> instance.</param>
    public RgbColor(Color color) : this(color.R, color.G, color.B)
    {
    }

    /// <summary>
    /// Returns the color as a six-digit hexadecimal string, in the form RRGGBB.
    /// </summary>
    public string ToHex()
    {
        int red = (int)(R * 255.99);
        int green = (int)(G * 255.99);
        int blue = (int)(B * 255.99);

        return $"{red:X2}{green:X2}{blue:X2}";
    }
}