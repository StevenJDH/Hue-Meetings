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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Hue_Meetings.Models;

namespace Hue_Meetings.Components;

/// <summary>
/// Handles Authorization Code Flow (3-legged OAuth2) with PKCE locally for remote Hue APIs.
/// </summary>
internal class WebAuthenticationBroker
{
    private readonly HttpListener _http;

    /// <summary>
    /// Gets status updates when authenticating with the WAB service.
    /// </summary>
    public event EventHandler<WabStatusChangedEventArgs>? WabStatusChanged;

    public WebAuthenticationBroker() => _http = new HttpListener();

    public async Task<HttpListenerContext> AuthenticateAsync(string authorizeUri, string redirectUri)
    {
        RaiseEvent($"redirect URI: {redirectUri}");
        // Creates an HttpListener to listen for requests on the redirect URI.
        _http.Prefixes.Add(redirectUri);
        RaiseEvent("Listening...");
        _http.Start();

        // Opens request in the browser.
        try
        {
            // UseShellExecute is false on .NET/Core, so we set it like .NET Framework to avoid Win32Exception.
            Process.Start(new ProcessStartInfo(authorizeUri) { UseShellExecute = true });
            // Waits for the OAuth authorization response.
            return await _http.GetContextAsync();
        }
        catch (Exception ex)
        {
            _http.Stop();
            RaiseEvent("HTTP server stopped due to an error.", ex.Message);
            throw;
        }
    }

    public async Task WriteResponseMessageAsync(string message, HttpListenerResponse response)
    {
        // Sends an HTTP response to the browser.
        string responseString = $"<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>{message}</body></html>";
        var buffer = Encoding.UTF8.GetBytes(responseString);

        response.ContentLength64 = buffer.Length;

        var responseOutput = response.OutputStream;
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await responseOutput.WriteAsync(buffer, cts.Token);
        responseOutput.Close();
        _http.Stop();
        RaiseEvent("HTTP server stopped.");
    }

    public static int GetRandomUnusedPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    /// <summary>
    /// A wrapper to raise events for WAB related status changes.
    /// </summary>
    /// <param name="status">Status message for event.</param>
    /// <param name="error">Optional error message for event.</param>
    public void RaiseEvent(string status, string? error = null) => OnWabStatusChanged(new WabStatusChangedEventArgs
    {
        Status = status,
        ErrorMessage = error
    });

    /// <summary>
    /// Event to raise for WAB status changes when communicating with a server.
    /// </summary>
    /// <param name="e">Status details to pass with event.</param>
    protected virtual void OnWabStatusChanged(WabStatusChangedEventArgs e) => WabStatusChanged?.Invoke(this, e);
}