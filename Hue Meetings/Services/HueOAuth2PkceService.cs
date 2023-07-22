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
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Hue_Meetings.Exceptions;
using Hue_Meetings.Models;

namespace Hue_Meetings.Services
{
    public sealed class HueOAuth2PkceService
    {
        private readonly string _appId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;
        private const string Endpoint = "/v2/oauth2/token";
        private AccessTokenResponse? _lastAccessTokenResponse;
        private string? _state;
        private string? _codeVerifier;

        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId">Identifies the client that is making the request. The value passed in this parameter must exactly match the value you receive from hue. Note that the underscore is not used in the clientid name of this parameter.</param>
        /// <param name="clientSecret">The client secret you have received from Hue when registering for the Hue Remote API.</param>
        /// <param name="appId">Identifies the app that is making the request. The value passed in this parameter must exactly match the value you receive from hue.</param>
        /// <param name="httpClient">Optional <see cref="HttpClient"/> used for testing purposes.</param>
        public HueOAuth2PkceService(string appId, string clientId, string clientSecret, HttpClient? httpClient = null)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            _appId = appId.Trim();
            _clientId = clientId.Trim();
            _clientSecret = clientSecret.Trim();
            
            _httpClient = httpClient ?? new HttpClient
            {
                BaseAddress = new Uri("https://api.meethue.com")
            };
        }

        public void Initialize(AccessTokenResponse? accessTokenResponse)
        {
            if (accessTokenResponse is null)
            {
                return;
            }

            _lastAccessTokenResponse = accessTokenResponse;
            IsInitialized = true;
        }

        /// <summary>
        /// Authorization request
        /// </summary>
        /// <param name="deviceId">The device identifier must be a unique identifier for the app or device accessing the Hue Remote API.</param>
        /// <param name="deviceName">The device name should be the name of the app or device accessing the remote API. The devicename is used in the user's "My Apps" overview in the Hue Account (visualized as: "<app name> on <devicename>"). If not present, deviceid is also used for devicename. The <app name> is the application name you provided to us the moment you requested access to the remote API.</param>
        /// <param name="responseType">The response_type value must be "code".</param>
        /// <param name="codeChallengeMethod"></param>
        /// <returns></returns>
        public string BuildPkceAuthorizeUri(string? deviceName, string deviceId, 
            string responseType = "code", string codeChallengeMethod = "S256")
        {
            if (string.IsNullOrEmpty(responseType))
            {
                throw new ArgumentNullException(nameof(responseType));
            }
            if (string.IsNullOrEmpty(codeChallengeMethod))
            {
                throw new ArgumentNullException(nameof(codeChallengeMethod));
            }

            // Generates state and PKCE values.
            _state = RandomDataBase64Url(32); // Provides any state that might be useful to your application upon receipt of the response. The Hue Authorization Server roundtrips this parameter, so your application receives the same value it sent. To mitigate against cross-site request forgery (CSRF), it is strongly recommended to include an anti-forgery token in the state, and confirm it in the response. One good choice for a state token is a string of 30 or so characters constructed using a high-quality random-number generator.
            _codeVerifier = RandomDataBase64Url(32);
            string codeChallenge = Base64UrlEncodeNoPadding(Sha256(_codeVerifier));

            return $"{_httpClient.BaseAddress}v2/oauth2/authorize?client_id={_clientId}&response_type={responseType}&state={_state}&appid={_appId}&deviceid={deviceId}&devicename={deviceName}&code_challenge={codeChallenge}&code_challenge_method={codeChallengeMethod}";
        }

        public RemoteAuthorizeResponse? ProcessAuthorizeResponse(HttpListenerContext context)
        {
            var result = new RemoteAuthorizeResponse
            {
                Code = context.Request.QueryString.Get("code"),
                State = context.Request.QueryString.Get("state"),
                Error = context.Request.QueryString.Get("error")
            };

            // Check for errors
            if (result.Error != null)
            {
                Output($"OAuth authorization error: {result.Error}.");
                return null;
            }
            if (String.IsNullOrWhiteSpace(result.Code) || String.IsNullOrWhiteSpace(result.State))
            {
                Output($"Malformed authorization response. {context.Request.QueryString}");
                return null;
            }

            // Compares the received state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            if (result.State != _state)
            {
                Output($"Received request with invalid state ({result.State})"); // TODO: remove after testing.
                return null;
            }
            Output("Authorization code: " + result.Code);

            return result;
        }

        /// <summary>
        /// Get an access token
        /// </summary>
        /// <param name="code">Code retrieved using ProcessAuthorizeResponse</param>
        /// <returns></returns>
        public async Task<AccessTokenResponse?> GetToken(string code)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "code", code },
                { "code_verifier", _codeVerifier },
                { "grant_type", "authorization_code" }
            };

            return await AuthenticateAsync(queryParams);
        }

        public async Task<AccessTokenResponse?> RefreshExpiredToken(string refreshToken)
        {
            var queryParams = new Dictionary<string, string> {
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" }
            };

            return await AuthenticateAsync(queryParams);
        }

        private async Task<AccessTokenResponse?> AuthenticateAsync(Dictionary<string, string> queryParams)
        {
            var queryParamContent = new FormUrlEncodedContent(queryParams);

            //Do a failed token request just to get the WWW-Authenticate header.
            var responseTask = await _httpClient.PostAsync(Endpoint, queryParamContent).ConfigureAwait(false);
            var responseString = responseTask.Headers.WwwAuthenticate.ToString();
            responseString = responseString.Replace("Digest ", string.Empty);
            string nonce = GetNonce(responseString);

            if (string.IsNullOrEmpty(nonce))
            {
                return null;
            }

            //Build request
            string response = CalculateDigestResponseHash(_clientId, _clientSecret, nonce, Endpoint);
            string param = $"username=\"{_clientId}\", {responseString}, uri=\"{Endpoint}\", response=\"{response}\"";
            
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Content = queryParamContent,
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Digest", param)
                }
            };

            //Get token
            var accessTokenResponse = await _httpClient.SendAsync(request).ConfigureAwait(false);

            accessTokenResponse.EnsureSuccessStatusCode();

            var accessTokenResponseString = await accessTokenResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var accessToken = JsonSerializer.Deserialize<AccessTokenResponse>(accessTokenResponseString);

            _lastAccessTokenResponse = accessToken;
            IsInitialized = true;
            Output("Access token: " + accessToken.AccessToken); // TODO: remove after testing.

            return accessToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wwwAuthenticateHeader"></param>
        /// <returns>Nonce value from header.</returns>
        private static string GetNonce(string wwwAuthenticateHeader) => wwwAuthenticateHeader.Split('"')[3];

        /// <summary>
        /// Calculate hash for token request
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="nonce"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string CalculateDigestResponseHash(string clientId, string clientSecret, string nonce, string path)
        {
            String HASH1 = GetMd5String($"{clientId}:oauth2_client@api.meethue.com:{clientSecret}");
            String HASH2 = GetMd5String("POST:" + path);
            // Using lowercase x2 hashes or the end result of hashing hashes is different.
            String response = GetMd5String(HASH1 + ":" + nonce + ":" + HASH2);

            return response;
        }

        public static string GetMd5String(string input)
        {
            using var md5 = MD5.Create();

            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            
            // Converts the byte array to hexadecimal string.
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            // Return MD5 hexadecimal hash string.
            return sb.ToString();
        }

        /// <summary>
        /// Refreshes the token if needed.
        /// </summary>
        /// <returns></returns>
        public async Task<AccessTokenResponse?> GetValidToken()
        {
            if (IsInitialized)
            {
                if (_lastAccessTokenResponse.AccessTokenExpireTime() > DateTimeOffset.UtcNow.AddMinutes(-5))
                {
                    return _lastAccessTokenResponse;
                }

                Output("Using refresh token to request a new access token...");

                return await RefreshExpiredToken(_lastAccessTokenResponse.RefreshToken).ConfigureAwait(false);
            }

            IsInitialized = false;
            throw new HueMeetingsException("Unable to get access token. Access token and Refresh token expired.");
        }

        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        private static string RandomDataBase64Url(uint length)
        {
            using var rng = RandomNumberGenerator.Create(); // Provides in the end a RNGCryptoServiceProvider.
            byte[] bytes = new byte[length];

            rng.GetBytes(bytes);

            return Base64UrlEncodeNoPadding(bytes);
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputStirng"></param>
        /// <returns></returns>
        private static byte[] Sha256(string inputStirng)
        {
            using var sha256 = SHA256.Create(); // Updated way of providing SHA256Managed instance.
            byte[] bytes = Encoding.UTF8.GetBytes(inputStirng);

            return sha256.ComputeHash(bytes);
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string Base64UrlEncodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">string to be appended</param>
        private static void Output(string output) => Debug.WriteLine(output);
    }
}