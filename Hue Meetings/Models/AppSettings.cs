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
using Hue_Meetings.Converters;

namespace Hue_Meetings.Models;

public sealed class AppSettings
{
    public string? SelectedBridgeId { get; set; }

    public string? SelectedLightName { get; set; }

    [JsonConverter(typeof(JsonColorConverter))]
    public Color AvailableColor { get; set; }

    [JsonConverter(typeof(JsonColorConverter))]
    public Color InMeetingColor { get; set; }

    public string? BridgeUsername { get; set; }

    public bool UseRemote { get; set; }

    public string? AppId { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public int CallbackPort { get; set; }

    public AccessTokenResponse? AccessToken { get; set; }
}