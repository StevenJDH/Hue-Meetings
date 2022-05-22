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
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Hue_Meetings.Exceptions;
using Hue_Meetings.Models;
using Hue_Meetings.Services.Interfaces;

namespace Hue_Meetings.Services
{
    public sealed class LocalBridgeLocatorService : IBridgeLocator
    {
        private const string NuPnPUrl = "https://discovery.meethue.com";
        private readonly HttpClient _httpClient;

        public LocalBridgeLocatorService(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
                CheckCertificateRevocationList = false,
                // This is required to use the bridge's self-signed certificate.
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                        cert?.Issuer.Equals("CN=GTS CA 1D4, O=Google Trust Services LLC, C=US") ?? false
            };

            return new HttpClient(httpClientHandler, disposeHandler: true);
        }

        public async Task<IEnumerable<Bridge>> GetBridgesAsync()
        {
            var response = await _httpClient.GetAsync(NuPnPUrl).ConfigureAwait(false);

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