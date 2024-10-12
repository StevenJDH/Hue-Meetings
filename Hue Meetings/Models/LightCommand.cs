/*
 * This file is part of Hue Meetings <https://github.com/StevenJDH/Hue-Meetings>.
 * Copyright (C) 2022-2024 Steven Jenkins De Haro.
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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hue_Meetings.Models;

/// <summary>
/// Mutable root record to accommodate the <see cref="Components.LightCommandBuilder"/> requirements.
/// </summary>
public record LightCommand
{
    [JsonPropertyName("color")]
    public DeviceColorCommand? Color { get; set; }

    [JsonPropertyName("dimming")]
    public DimmingCommand? Dimming { get; set; }

    /// <summary>
    /// Sets whether the light is on or Off.
    /// </summary>
    [JsonPropertyName("on")]
    public On? PowerState { get; set; }
}

public record DeviceColorCommand
{
    /// <summary>
    /// Sets the colors based on CIE 1931 Color coordinates.
    /// </summary>
    [JsonPropertyName("xy")]
    public Coordinates ColorCoordinates { get; init; } = null!;
}

public record DimmingCommand
{
    /// <summary>
    /// Sets the brightness percentage. Value cannot be 0, so writing 0 changes it to lowest possible brightness.
    /// </summary>
    [JsonPropertyName("brightness")]
    public float Brightness { get; init; }
}