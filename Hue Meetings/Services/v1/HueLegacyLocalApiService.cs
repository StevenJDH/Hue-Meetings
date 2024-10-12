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

using System.Security.Authentication;
using Hue_Meetings.Models;

namespace Hue_Meetings.Services.v1
{
    public sealed class HueLegacyLocalApiService : HueLegacyApiService, IDisposable
    {
        public HueLegacyLocalApiService(string bridgeIpAddress, HttpClient? httpClient = null) 
            : base(httpClient ?? CreateHttpClient(bridgeIpAddress))
        {
        }

        private static HttpClient CreateHttpClient(string bridgeIpAddress)
        {
            var httpClientHandler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
                CheckCertificateRevocationList = false,
                // This is required to use the bridge's self-signed certificate.
                ServerCertificateCustomValidationCallback =
                    (_, cert, _, _) =>
                        cert?.Issuer.Equals("CN=root-bridge, O=Philips Hue, C=NL") ?? false
            };

            return new HttpClient(httpClientHandler, disposeHandler: true)
            {
                BaseAddress = new Uri($"https://{bridgeIpAddress}/api/")
            };
        }

        [Obsolete("Removed from Local Bridge API. Use https://account.meethue.com/homes.")]
        public override async Task<bool> DeleteWhiteListEntryAsync(string bridgeUser)
        {
            return await Task.Run(() =>
            {
                OpenWebsite("https://account.meethue.com/homes"); // Seems https://account.meethue.com/apps no longer works.
                return true;
            });
        }

        [Obsolete("Removed from Local Bridge API. Use the hardware 'link' button on the Philips Hue Bridge.")]
        public override async Task<bool> EnableLinkButtonAsync() => await Task.Run(() => false);

        public override async Task<IEnumerable<WhiteList>?> GetWhiteListAsync()
        {
            CheckInitialized();
            return await base.GetWhiteListAsync();
        }

        public override async Task<BridgeConfig?> GetConfigAsync()
        {
            CheckInitialized();
            return await base.GetConfigAsync();
        }

        /// <summary>
        /// Releases any unmanaged resources and disposes of the managed resources used
        /// by the <see cref="HueLegacyLocalApiService"/>.
        /// </summary>
        public void Dispose() => _httpClient.Dispose();
    }
}