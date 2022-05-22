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
using Hue_Meetings.Components.HueColor;
using Hue_Meetings.Components.Interfaces;
using Hue_Meetings.Models;

namespace Hue_Meetings.Components;

public sealed class LightCommandBuilder : ILightCommandBuilder
{
    private readonly LightCommand _lightCommand;

    private LightCommandBuilder() => _lightCommand = new LightCommand();

    public static LightCommandBuilder Create() => new();

    public ILightCommandBuilder WithColorCoordinates(Coordinates colorCoordinates)
    {
        ArgumentNullException.ThrowIfNull(colorCoordinates);

        _lightCommand.Color = new DeviceColorCommand
        {
            ColorCoordinates = colorCoordinates
        };

        return this;
    }

    public ILightCommandBuilder WithBrightness(float brightness)
    {
        _lightCommand.Dimming = new DimmingCommand
        {
            Brightness = brightness
        };

        return this;
    }

    public ILightCommandBuilder SwitchOn()
    {
        _lightCommand.PowerState = new On
        {
            IsOn = true
        };

        return this;
    }

    public ILightCommandBuilder SwitchOff()
    {
        _lightCommand.PowerState = new On
        {
            IsOn = false
        };

        return this;
    }

    public ILightCommandBuilder WithColor(Color color, Gamut? colorGamut)
    {
        ArgumentNullException.ThrowIfNull(color);

        if (colorGamut is null)
        {
            return this;
        }

        var rgbColor = new RgbColor(color);

        var redPoint = new Cie1931Point(colorGamut.Red.X, colorGamut.Red.Y);
        var greenPoint = new Cie1931Point(colorGamut.Green.X, colorGamut.Green.Y);
        var bluePoint = new Cie1931Point(colorGamut.Blue.X, colorGamut.Blue.Y);

        var gamut = new Cie1931Gamut(redPoint, greenPoint, bluePoint);
        var point = HueColorConverter.RgbToXy(rgbColor, gamut);

        WithColorCoordinates(new Coordinates
        {
            X = point.X,
            Y = point.Y
        });

        return this;
    }

    public LightCommand Build() => _lightCommand;
}