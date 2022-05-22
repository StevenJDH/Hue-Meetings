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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hue_Meetings.Components;
using Hue_Meetings.Exceptions;
using Hue_Meetings.Models;
using Hue_Meetings.Services;
using Hue_Meetings.Services.Interfaces;
using Hue_Meetings.Services.v1;

namespace Hue_Meetings;

public partial class FrmRegister : Form
{
    private const string ApplicationName = "hue-meetings";
    private readonly AppConfiguration _config;
    private readonly WebAuthenticationBroker _wab;

    public FrmRegister()
    {
        InitializeComponent();

        _config = AppConfiguration.Instance;
        _wab = new WebAuthenticationBroker();
        _wab.WabStatusChanged += Wab_StatusChanged;
    }

    private void LoadTheme()
    {
        foreach (Control button in this.Controls)
        {
            if (button.GetType() == typeof(Button))
            {
                Button btn = (Button) button;
                btn.BackColor = ThemeColor.PrimaryColor;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            }
        }

        // TODO: you could using primary and secondary colors to set the forecolor here for labels.
    }

    private void FrmRegister_Load(object sender, EventArgs e)
    {
        LoadTheme();
        lblBridgeUser.Text = $"This will add a user called '{ApplicationName}#{Environment.MachineName}' to your bridge.";

        int port;

        if (_config.Settings.CallbackPort != 0)
        {
            port = _config.Settings.CallbackPort;
        }
        else
        {
            port = WebAuthenticationBroker.GetRandomUnusedPort();
            _config.Settings.CallbackPort = port;
            _config.SaveSettings();
        }

        txtAppId.Text = _config.Settings.AppId;
        txtClientId.Text = _config.Settings.ClientId;
        txtClientSecret.Text = _config.Settings.ClientSecret;
        txtCallback.Text = $"http://localhost:{port}/callback/oauth/"; // Ending slash is required by the listener.
        btnGrantAccess.Enabled = _config.Settings.AccessToken is null;
    }

    private async void btnFind_Click(object sender, EventArgs e)
    {
        cmbBridges.Items.Clear();
        cmbUsers.Items.Clear();

        IBridgeLocator locator;

        if (chkRemote.CheckState == CheckState.Unchecked)
        {
            // TODO: handle TaskCanceledException like var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
            locator = new LocalBridgeLocatorService();
        }
        else
        {
            if (_config.Settings.AccessToken is null)
            {
                MessageBox.Show("Please grant access when connected via a VPN before proceeding.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var oAuth2PkceService =
                new HueOAuth2PkceService(txtAppId.Text, txtClientId.Text, txtClientSecret.Text);

            oAuth2PkceService.Initialize(_config.Settings.AccessToken);
            var accessTokenResponse = await oAuth2PkceService.GetValidToken();

            if (accessTokenResponse != _config.Settings.AccessToken)
            {
                _config.Settings.AccessToken = accessTokenResponse;
                _config.SaveSettings();
            }

            if (accessTokenResponse == null)
            {
                return;
            }

            locator = new RemoteBridgeLocatorService(accessTokenResponse);
        }

        try
        {
            var bridges = await locator.GetBridgesAsync();

            foreach (var bridge in bridges)
            {
                // TODO: Check if Philips Hue has really removed Internal IP on remote responses.
                cmbBridges.Items.Add($"{bridge.BridgeId} [{bridge.InternalIpAddress}]");
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or HueMeetingsException)
        {
            MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        if (cmbBridges.Items.Count > 0)
        {
            cmbBridges.SelectedIndex = 0;
        }
        else
        {
            MessageBox.Show("No bridges found or too many requests, in which case, try again later. ",
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }

    private async void btnGrantAccess_Click(object sender, EventArgs e)
    {
        var oAuth2PkceService = new HueOAuth2PkceService(txtAppId.Text, txtClientId.Text, txtClientSecret.Text);
        string authorizeUri = oAuth2PkceService.BuildPkceAuthorizeUri(ApplicationName, Environment.MachineName);

        // TODO: remove after testing.
        Debug.WriteLine($"authorizeUri: {authorizeUri}");

        var context = await _wab.AuthenticateAsync(authorizeUri, txtCallback.Text);

        await _wab.WriteResponseMessageAsync("Please return to the app.", context.Response);
        this.Activate();

        // Parse and process response.
        var result = oAuth2PkceService.ProcessAuthorizeResponse(context);

        if (result is null)
        {
            return;
        }

        // Starts the code exchange at the Token Endpoint.
        var accessTokenResponse = await oAuth2PkceService.GetToken(result.Code);

        _config.Settings.AppId = txtAppId.Text.Trim();
        _config.Settings.ClientId = txtClientId.Text.Trim();
        _config.Settings.ClientSecret = txtClientSecret.Text.Trim();
        _config.Settings.AccessToken = accessTokenResponse;
        _config.SaveSettings();

        btnGrantAccess.Enabled = false;
    }

    private async void cmbBridges_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbUsers.Items.Clear();

        if (_config.Settings.BridgeUsername is null)
        {
            btnRegister.Enabled = true;
            return;
        }

        var legacyApiService = await GetHueApiService();
        var whiteList = await legacyApiService.GetWhiteListAsync();

        foreach (var entry in whiteList)
        {
            cmbUsers.Items.Add($"{entry.Name} [{entry.Id}]");
        }

        bool userFound = false;
        
        for (int i = 0; i < cmbUsers.Items.Count; i++)
        {
            if (cmbUsers.Items[i].ToString().StartsWith($"{ApplicationName}#{Environment.MachineName}"))
            {
                cmbUsers.SelectedIndex = i;
                userFound = true;
                break;
            }
        }

        btnRegister.Enabled = !userFound;
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        String user = cmbUsers.Text.Split('[')[0].Trim();

        if (MessageBox.Show($"Are you sure you want to delete the [{user}] entry?", Application.ProductName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
            return;
        }

        var apiService = await GetHueApiService();
        bool response = await apiService.DeleteWhiteListEntryAsync(cmbUsers.Text.Split('[', ']')[1]);

        switch (response)
        {
            case true when chkRemote.CheckState == CheckState.Unchecked:
                MessageBox.Show($"Don't forget to click 'Find' to refresh the list after deleting [{user}] via the website.", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
            case true:
                MessageBox.Show($"Entry [{user}] was successfully deleted.", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                cmbBridges_SelectedIndexChanged(this, EventArgs.Empty);
                break;
            default:
                MessageBox.Show($"Entry [{user}] could not be deleted.", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                break;
        }
    }

    private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnDelete.Enabled = !String.IsNullOrWhiteSpace(cmbUsers.Text);
    }

    private void lnkGenerateAccess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            Process.Start("https://developers.meethue.com/add-new-hue-remote-api-app");
        }
        catch (Exception ex)
        {
            // Consuming exceptions.
            Debug.WriteLine(ex.Message);
        }
    }

    private async void btnRegister_Click(object sender, EventArgs e)
    {
        // TODO: Create process to say in case config was lost and a user was already registered, it must be deleted first and registered. This scenario can be determined by seeing that it is in the list, but the value in the config is missing.
        HueLegacyApiService legacyApiService = new HueLegacyLocalApiService(cmbBridges.Text.Split('[', ']')[1]);

        if (MessageBox.Show("Have you pressed the link button on the Philips Hue bridge?", Application.ProductName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
            return;
        }

        try
        {
            string? appKey = await legacyApiService.RegisterWhiteListEntryAsync(ApplicationName, Environment.MachineName);
            _config.Settings.SelectedBridgeId = cmbBridges.Text[..16];
            _config.Settings.BridgeUsername = appKey;
            _config.SaveSettings();
            cmbBridges_SelectedIndexChanged(this, EventArgs.Empty);
        }
        catch (HueMeetingsException ex)
        {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task<HueLegacyApiService?> GetHueApiService()
    {
        if (chkRemote.CheckState == CheckState.Unchecked)
        {
            HueLegacyApiService legacyApiService = new HueLegacyLocalApiService(cmbBridges.Text.Split('[', ']')[1]); // IP
            legacyApiService.Initialize(_config.Settings.BridgeUsername);
            return legacyApiService;
        }

        if (_config.Settings.AccessToken is null)
        {
            MessageBox.Show("Please grant access when connected via a VPN before proceeding.",
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return null;
        }

        var oAuth2PkceService = new HueOAuth2PkceService(txtAppId.Text, txtClientId.Text, txtClientSecret.Text);

        oAuth2PkceService.Initialize(_config.Settings.AccessToken);

        var accessTokenResponse = await oAuth2PkceService.GetValidToken();

        if (accessTokenResponse != _config.Settings.AccessToken)
        {
            _config.Settings.AccessToken = accessTokenResponse;
            _config.SaveSettings();
        }

        return new HueLegacyRemoteApiService(accessTokenResponse);
    }

    private void Wab_StatusChanged(object? sender, WabStatusChangedEventArgs e) =>
        Debug.WriteLine($"{e.Status}{(e.ErrorMessage != null ? $" {e.ErrorMessage}" : "")}");
}