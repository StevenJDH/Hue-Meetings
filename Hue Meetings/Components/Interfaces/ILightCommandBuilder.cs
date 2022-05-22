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

namespace Hue_Meetings.Components.Interfaces;

public interface ILightCommandBuilder
{
    ILightCommandBuilder WithColorCoordinates(Coordinates colorCoordinates);

    ILightCommandBuilder WithBrightness(float brightness);

    ILightCommandBuilder SwitchOn();

    ILightCommandBuilder SwitchOff();

    ILightCommandBuilder WithColor(Color color, Gamut? colorGamut);

    LightCommand Build();
}