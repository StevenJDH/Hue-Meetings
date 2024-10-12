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
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hue_Meetings.Models;

namespace Hue_Meetings.Components;

sealed class AppConfiguration
{
    private readonly string _configPath;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Constructs a new <see cref="AppConfiguration"/> instance to manage application data.
    /// </summary>
    private AppConfiguration()
    {
        _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ASC-C", "Hue Meetings", "HueMeetingsConfig.json");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyProperties = true,
            WriteIndented = true
        };

        try
        {
            LoadSettings();
        }
        catch (IOException ex)
        {
            MessageBox.Show($"{ex.GetType().Name}: {ex.Message}",
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Process.GetCurrentProcess().Kill();
        }
    }

    /// <summary>
    /// Gets a singleton instance of <see cref="AppConfiguration"/>.
    /// </summary>
    public static AppConfiguration Instance { get; } = new();

    public AppSettings Settings { get; private set; } = null!;

    /// <summary>
    /// Sets up the configuration file and loads any settings used by the application.
    /// </summary>
    private void LoadSettings()
    {
        if (File.Exists(_configPath))
        {
            string jsonData = File.ReadAllText(_configPath);

            // Any trouble reading the configuration will just use the defaults.
            try
            {
                Settings = JsonSerializer.Deserialize<AppSettings>(jsonData, _jsonOptions) ?? throw new JsonException();
            }
            catch (Exception ex) when (ex is JsonException or ArgumentException)
            {
                ResetSettings(createBackup: true);
                MessageBox.Show("Error: The configuration has been reset due to corrupt settings.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("A new configuration file will be created for first time use.",
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            ResetSettings(createBackup: false);

            MessageBox.Show("All done! You are now ready to start using the program.",
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// Resets the application settings and creates an optional backup if an existing configuration
    /// file exists.
    /// </summary>
    /// <param name="createBackup">
    /// Set to <see langword="true" /> to create backup, <see langword="false" /> if not needed.
    /// </param>
    private void ResetSettings(bool createBackup)
    {
        if (createBackup && File.Exists(_configPath))
        {
            File.Copy(_configPath, $"{_configPath}_{DateTime.Now:yyyy-MM-dd_HHmmss}.bak", overwrite: true);
        }

        Settings = new AppSettings
        {
            AvailableColor = Color.Green,
            InMeetingColor = Color.Red,
            AppId = "hue_meetings"
        };

        SaveSettings();
    }

    /// <summary>
    /// Saves the application settings to a configuration file.
    /// </summary>
    public void SaveSettings()
    {
        string jsonData = JsonSerializer.Serialize(Settings, _jsonOptions);

        // Builds any missing folders in path where the configuration will be stored.
        Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);
        // Saves the configuration to profile.
        File.WriteAllText(_configPath, jsonData);
    }
}