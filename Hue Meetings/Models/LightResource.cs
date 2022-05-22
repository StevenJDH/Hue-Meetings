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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hue_Meetings.Models;

public record LightResource
{
    [JsonPropertyName("errors")]
    public ErrorResponse[]? Errors { get; init; }

    [JsonPropertyName("data")]
    public Datum[] Data { get; init; } = null!;
}

public record Datum
{
    [JsonPropertyName("color")]
    public DeviceColor Color { get; init; } = null!;

    [JsonPropertyName("dimming")]
    public Dimming Dimming { get; init; } = null!;

    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("metadata")]
    [Obsolete("Deprecated: Use metadata on /resource/device level.")] // TODO: Update metadata support.
    public Metadata Metadata { get; init; } = null!;

    [JsonPropertyName("on")]
    public On PowerState { get; init; } = null!;

    [JsonPropertyName("type")]
    public string DeviceType { get; init; } = null!;
}

public record DeviceColor
{
    [JsonPropertyName("gamut")]
    public Gamut? ColorGamut { get; init; }

    /// <summary>
    /// The gamut types supported by hue:
    /// - A Gamut of early Philips color-only products.
    /// - B Limited gamut of first Hue color products.
    /// – C Richer color gamut of Hue white and color ambiance products.
    /// – Other color gamut of non-hue products with non-hue gamuts resp w/o gamut.
    /// </summary>
    [JsonPropertyName("gamut_type")]
    public string ColorGamutType { get; init; } = null!;

    [JsonPropertyName("xy")]
    public Coordinates ColorCoordinates { get; init; } = null!;
}

/// <summary>
/// Color gamut of color bulb. Some bulbs do not properly return the Gamut information. In this case this is not present.
/// </summary>
public record Gamut
{
    [JsonPropertyName("blue")]
    public Coordinates Blue { get; init; } = null!;

    [JsonPropertyName("green")]
    public Coordinates Green { get; init; } = null!;

    [JsonPropertyName("red")]
    public Coordinates Red { get; init; } = null!;
}

public record Coordinates
{
    [JsonPropertyName("x")]
    public double X { get; init; }

    [JsonPropertyName("y")]
    public double Y { get; init; }
}

public record Dimming
{
    [JsonPropertyName("brightness")]
    public float Brightness { get; init; }

    [JsonPropertyName("min_dim_level")]
    public double MinDimLevel { get; init; }
}

public record Metadata
{
    [JsonPropertyName("archetype")]
    public string Archetype { get; init; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
}

public record On
{
    [JsonPropertyName("on")]
    public bool IsOn { get; init; }
}