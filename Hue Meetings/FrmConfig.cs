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
using Hue_Meetings.Components.HueColor;
using Hue_Meetings.Models;
using Hue_Meetings.Services;
using Hue_Meetings.Services.Interfaces;
using Hue_Meetings.Services.v2;

namespace Hue_Meetings;

public partial class FrmConfig : Form
{
    private readonly AppConfiguration _config;

    public FrmConfig()
    {
        InitializeComponent();
        _config = AppConfiguration.Instance;
            
        // Make color preview labels transparent over new parent.
        lblAvailable.Location =
            pbAvailableColor.PointToClient(lblAvailable.Parent.PointToScreen(lblAvailable.Location));
        lblBusy.Location =
            pbBusyColor.PointToClient(lblBusy.Parent.PointToScreen(lblBusy.Location));

        lblAvailable.Parent = pbAvailableColor;
        lblBusy.Parent = pbBusyColor;
    }

    private void LoadTheme()
    {
        foreach (Control button in this.Controls)
        {
            if (button.GetType() == typeof(Button))
            {
                Button btn = (Button)button;
                btn.BackColor = ThemeColor.PrimaryColor;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            }
        }

        // TODO: you could using primary and secondary colors to set the forecolor here for labels.
    }

    private void FrmConfig_Load(object sender, EventArgs e)
    {
        LoadTheme();

        if (_config.Settings.AccessToken is null)
        {
            chkRemote.Enabled = false;
            lblVpnNotice.Visible = true;
        }

        chkRemote.Checked = _config.Settings.UseRemote;
        pbAvailableColor.BackColor = _config.Settings.AvailableColor;
        pbBusyColor.BackColor = _config.Settings.InMeetingColor;
        colorDialog1.Color = pbBusyColor.BackColor;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    private async void btnFind_Click(object sender, EventArgs e)
    {
        HueApiService apiService;

        if (chkRemote.CheckState == CheckState.Unchecked)
        {
            var locator = new LocalBridgeLocatorService();
            var bridges = await locator.GetBridgesAsync();

            if (!bridges.Any())
            {
                MessageBox.Show("No bridges found or too many requests, in which case, try again later. ",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            apiService = new HueLocalApiService(bridges.First().InternalIpAddress);
        }
        else
        {
            var oAuth2PkceService =
                new HueOAuth2PkceService(_config.Settings.AppId, _config.Settings.ClientId, _config.Settings.ClientSecret);

            oAuth2PkceService.Initialize(_config.Settings.AccessToken);

            var accessTokenResponse = await oAuth2PkceService.GetValidToken();

            if (accessTokenResponse != _config.Settings.AccessToken)
            {
                _config.Settings.AccessToken = accessTokenResponse;
                _config.SaveSettings();
            }

            apiService = new HueRemoteApiService(accessTokenResponse);
        }

        apiService.Initialize(_config.Settings.BridgeUsername);

        var lights = await apiService.GetLightsAsync();

        cmbLights.Items.Clear();

        foreach (var light in lights)
        {
            cmbLights.Items.Add(light.Metadata.Name);
        }

        if (cmbLights.Items.Count > 0)
        {
            cmbLights.SelectedIndex = 0;
        }
    }

    private void btnAvailableColorPicker_Click(object sender, EventArgs e)
    {
        if (colorDialog1.ShowDialog() == DialogResult.OK)
        {
            pbAvailableColor.BackColor = colorDialog1.Color;
        }
    }

    private void btnBusyColorPicker_Click(object sender, EventArgs e)
    {
        if (colorDialog1.ShowDialog() == DialogResult.OK)
        {
            pbBusyColor.BackColor = colorDialog1.Color;
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (cmbLights.Items.Count > 0)
        {
            // TODO: Change to light ID to support name changes.
            _config.Settings.SelectedLightName = cmbLights.Text;
        }

        _config.Settings.UseRemote = chkRemote.Checked;
        _config.Settings.AvailableColor = pbAvailableColor.BackColor;
        _config.Settings.InMeetingColor = pbBusyColor.BackColor;
        _config.SaveSettings();
    }
}