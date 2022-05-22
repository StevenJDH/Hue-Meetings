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
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hue_Meetings.Exceptions;
using Hue_Meetings.Models;

namespace Hue_Meetings.Services.v1
{
    public abstract class HueLegacyApiService
    {
        protected string? _bridgeUser;
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// Indicates the HueClient is initialized with an AppKey
        /// </summary>
        public bool IsInitialized { get; protected set; }

        protected HueLegacyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual void Initialize(string bridgeUser)
        {
            _bridgeUser = bridgeUser ?? throw new ArgumentNullException(nameof(bridgeUser));
            IsInitialized = true;
        }

        /// <summary>
        /// Check if the HueClient is initialized
        /// </summary>
        protected void CheckInitialized() // TODO: move to child if not needed by remote, and set to private.
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Hue Local API Service is not initialized. First register a user or call Initialize.");
            }
        }

        /// <summary>
        /// Register your <paramref name="appName"/> and <paramref name="deviceName"/> at the Hue Bridge.
        /// </summary>
        /// <param name="deviceName">The name of the device.</param>
        /// <param name="appName">The name of your app.</param>
        /// <returns><c>true</c> if success, <c>false</c> if the link button hasn't been pressed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="appName"/> or <paramref name="deviceName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="appName"/> or <paramref name="deviceName"/> aren't long enough, are empty or contains spaces.</exception>
        public async Task<string?> RegisterWhiteListEntryAsync(string appName, string deviceName) // TODO: may need to be a static method as there is no key to initialize ctor.
        {
            string fullName = $"{appName}#{deviceName}";

            var payload = new JsonObject // Don't need 'generateclientkey' field.
            {
                ["devicetype"] = fullName
            };

            var content = new StringContent(payload.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _httpClient.PostAsync("", content).ConfigureAwait(false);
            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var jsonNode = JsonNode.Parse(jsonResponse);

            if (jsonNode is not JsonArray)
            {
                throw new HueMeetingsException(jsonResponse);
            }

            if (jsonNode[0]!.AsObject().TryGetPropertyValue("error", out var error))
            {
                if (error!["type"]!.GetValue<int>() == 101) // link button not pressed
                {
                    throw new LinkButtonNotPressedException("Link button was not pressed.");
                }
                throw new HueMeetingsException(error["description"]?.GetValue<string>());
            }

            return jsonNode[0]!["success"]!["username"]?.GetValue<string>();
        }


        /// <summary>
        /// Deletes a whitelist entry
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> DeleteWhiteListEntryAsync(string bridgeUser) // Alternative: https://account.meethue.com/apps
        {
            var response = await _httpClient.DeleteAsync($"0/config/whitelist/{bridgeUser}").ConfigureAwait(false);
            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var jsonNode = JsonNode.Parse(jsonResponse);

            if (jsonNode is not JsonArray)
            {
                throw new HueMeetingsException(jsonResponse);
            }

            if (jsonNode[0]!.AsObject().TryGetPropertyValue("error", out var error))
            {
                // Hue gives back errors in an array for this request
                if (error!["type"]!.GetValue<int>() == 3) // entry not available
                {
                    return false;
                }
                throw new HueMeetingsException(error["description"]?.GetValue<string>() ?? "Unknown error when deleting user.");
            }

            return jsonNode[0]!.AsObject().ContainsKey("success");
        }

        public virtual async Task<bool> EnableLinkButtonAsync()
        {
            var payload = new JsonObject
            {
                ["linkbutton"] = true
            };

            var content = new StringContent(payload.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _httpClient.PutAsync("0/config", content).ConfigureAwait(false);
            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var jsonNode = JsonNode.Parse(jsonResponse);

            if (jsonNode is not JsonArray)
            {
                throw new HueMeetingsException(jsonResponse);
            }

            return jsonNode[0]!.AsObject().ContainsKey("success");
        }

        /// <summary>
        /// Asynchronously gets the whitelist with the bridge.
        /// </summary>
        /// <returns>An enumerable of <see cref="WhiteList"/>s registered with the bridge.</returns>
        public virtual async Task<IEnumerable<WhiteList>?> GetWhiteListAsync()
        {
            var config = await GetConfigAsync().ConfigureAwait(false);
            return config?.WhiteList.Select(entry => entry.Value).ToList();
        }

        /// <summary>
        /// Get bridge config
        /// </summary>
        /// <returns>BridgeConfig object</returns>
        public virtual async Task<BridgeConfig?> GetConfigAsync()
        {
            string jsonResponse = await _httpClient.GetStringAsync($"{_bridgeUser}/config").ConfigureAwait(false);
            var jsonNode = JsonNode.Parse(jsonResponse);

            if (jsonNode is not JsonObject)
            {
                return null;
            }

            var config = JsonSerializer.Deserialize<BridgeConfig>(jsonResponse);

            if (config == null)
            {
                return config;
            }

            // Adds whitelist IDs so it can be used later if needed.
            foreach (var (key, value) in config.WhiteList)
            {
                value.Id = key;
            }

            return config;
        }

        /// <summary>
        /// Sends a URL to the operating system to have it open in the default web browser.
        /// </summary>
        /// <param name="url">URL of website to open.</param>
        protected void OpenWebsite(string url)
        {
            try
            {
                // UseShellExecute is false on .NET/Core, so we set it like .NET Framework to avoid Win32Exception.
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // Consuming exceptions.
                Debug.WriteLine(ex.Message);
            }
        }
    }
}