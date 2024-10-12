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

public record Bridge([property: JsonPropertyName("id")] string BridgeId,
                     [property: JsonPropertyName("internalipaddress")] string? InternalIpAddress)
{
    /// <summary>
    /// Provides the Bridge Id, which is based on the MAC, and the internal IP if available for a
    /// more readable object name.
    /// </summary>
    /// <returns>Object name in the form of "Bridge 001a223b445c66ff: 192.168.1.50".</returns>
    public sealed override string ToString()
    {
        // Philips Hue removed Internal IP on remote responses, so we add when available.
        String internalIp = InternalIpAddress is null ? "" : $": {InternalIpAddress}";
        return $"Bridge {BridgeId}{internalIp}";
    }
}