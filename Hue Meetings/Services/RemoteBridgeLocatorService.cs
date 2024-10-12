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
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Hue_Meetings.Exceptions;
using Hue_Meetings.Models;
using Hue_Meetings.Services.Interfaces;

namespace Hue_Meetings.Services
{
    public sealed class RemoteBridgeLocatorService : IBridgeLocator
    {
        private const string BridgeUrl = "https://api.meethue.com/v2/bridges";
        private readonly string _accessToken;
        private readonly HttpClient _httpClient;
        

        public RemoteBridgeLocatorService(AccessTokenResponse accessTokenResponse, HttpClient? httpClient = null)
        {
            _accessToken = accessTokenResponse.AccessToken;
            _httpClient = httpClient ?? CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", _accessToken)
                }
            };
        }

        /// <summary>
        /// Gets the bridge ID registered by the user. When a user has linked a bridge to an account on www.meethue.com the bridge will appear on this interface.  
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Bridge>> GetBridgesAsync()
        {
            var response = await _httpClient.GetAsync(BridgeUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                return Enumerable.Empty<Bridge>();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var bridges = JsonSerializer.Deserialize<List<Bridge>>(jsonResponse);

            return bridges ?? Enumerable.Empty<Bridge>();
        }
    }
}