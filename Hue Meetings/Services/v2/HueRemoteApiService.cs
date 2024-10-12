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

using System.Net.Http.Headers;
using Hue_Meetings.Models;
using Hue_Meetings.Services.v1;

namespace Hue_Meetings.Services.v2
{
    public sealed class HueRemoteApiService : HueApiService, IDisposable
    {
        public HueRemoteApiService(AccessTokenResponse accessTokenResponse, HttpClient? httpClient = null) 
            : base(httpClient ?? CreateHttpClient(accessTokenResponse))
        {
        }

        private static HttpClient CreateHttpClient(AccessTokenResponse accessTokenResponse)
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://api.meethue.com/route/clip/v2/resource/"),
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResponse.AccessToken)
                }
            };
        }

        /// <summary>
        /// Releases any unmanaged resources and disposes of the managed resources used
        /// by the <see cref="HueLegacyRemoteApiService"/>.
        /// </summary>
        public void Dispose() => _httpClient.Dispose();
    }
}