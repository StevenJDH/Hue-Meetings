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

using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hue_Meetings.Exceptions;
using Hue_Meetings.Models;

namespace Hue_Meetings.Services.v2
{
    public abstract class HueApiService
    {
        protected string? _bridgeUser;
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// Indicates the HueClient is initialized with an AppKey
        /// </summary>
        public bool IsInitialized { get; protected set; }

        protected HueApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual void Initialize(string bridgeUser)
        {
            _bridgeUser = bridgeUser ?? throw new ArgumentNullException(nameof(bridgeUser));
            _httpClient.DefaultRequestHeaders.Add("hue-application-key", _bridgeUser);
            IsInitialized = true;
        }

        /// <summary>
        /// Checks to see if the Hue API Service is initialized.
        /// </summary>
        protected void CheckInitialized()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Hue API Service is not initialized. First register a user or call Initialize.");
            }
        }

        /// <summary>
        /// Asynchronously gets all lights registered with the bridge.
        /// </summary>
        /// <returns>An enumerable of <see cref="LightResource"/>s registered with the bridge.</returns>
        public async Task<IEnumerable<Datum>> GetLightsAsync()
        {
            CheckInitialized();
            var response = await _httpClient.GetAsync("light").ConfigureAwait(false);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var lights = JsonSerializer.Deserialize<LightResource>(jsonResponse);

            // TODO: This doesn't cover none hue errors like when missing username header. maybe we can skip all of this and just throw exception.
            if (lights is null || !response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Error: {(lights?.Errors is not null ? lights.Errors[0].Description : "API request failed.")}");
                return Enumerable.Empty<Datum>();
            }

            if (lights.Data is null || lights.Data.Length == 0)
            {
                return Enumerable.Empty<Datum>();
            }
            
            return lights.Data;
        }

        /// <summary>
        /// Asynchronously retrieves an individual light.
        /// </summary>
        /// <param name="id">The light's Id.</param>
        /// <returns>The <see cref="LightResource"/> if found, <c>null</c> if not.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="id"/> is empty or a blank string.</exception>
        public async Task<Datum?> GetLightAsync(string id)
        {
            CheckInitialized();

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync($"light/{id}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HueMeetingsException(ex.Message);
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var light = JsonSerializer.Deserialize<LightResource>(jsonResponse);

            return light?.Data?[0];
        }

        /// <summary>
        /// Send a light command
        /// </summary>
        /// <param name="command">Compose a new lightCommand()</param>
        /// <param name="lightId">Id of light to target for command.</param>
        /// <returns></returns>
        public async Task<bool> SendLightCommandAsync(LightCommand command, string lightId)
        {
            CheckInitialized();
            string jsonCommand = JsonSerializer.Serialize(command, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(jsonCommand, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _httpClient.PutAsync($"light/{lightId}", content).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}
