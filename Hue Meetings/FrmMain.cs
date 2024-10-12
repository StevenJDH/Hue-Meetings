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

using System.Diagnostics;
using AudioSwitcher.AudioApi.CoreAudio;
using Hue_Meetings.Components;

namespace Hue_Meetings;

public partial class FrmMain : Form
{
    private const int CP_DISABLED_CLOSE_BUTTON = 0x200;
    private readonly AppConfiguration _config;
    private bool _startupVisible;

    private Button? _currentButton;
    private readonly Random _random;
    private int _tempIndex;
    private Form? _activeForm;

    public FrmMain()
    {
        InitializeComponent();

        _config = AppConfiguration.Instance;
        _startupVisible = false;
        _random = new Random();

        this.Icon = notifyIcon1.Icon;
    }

    private Color SelectThemeColor()
    {
        int index = _random.Next(ThemeColor.ColorList.Count);
        while (_tempIndex == index)
        {
            index = _random.Next(ThemeColor.ColorList.Count);
        }

        _tempIndex = index;
        string color = ThemeColor.ColorList[index];
        return ColorTranslator.FromHtml(color);
    }

    private void ActivateButton(object? btnSender)
    {
        if (btnSender is null)
        {
            return;
        }

        if (_currentButton == (Button) btnSender)
        {
            return;
        }

        DisableButton();
        Color color = SelectThemeColor();
        _currentButton = (Button) btnSender;
        _currentButton.BackColor = color;
        _currentButton.ForeColor = Color.White;
        _currentButton.Font = new Font("Segoe UI", 12.5F, FontStyle.Regular, GraphicsUnit.Point);
        panel3.BackColor = color;
        panel2.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
        ThemeColor.PrimaryColor = color;
        ThemeColor.SecondaryColor = ThemeColor.ChangeColorBrightness(color, -0.3);
        btnTitleExit.Visible = true;
    }

    private void DisableButton()
    {
        foreach (Control previousBtn in panel1.Controls)
        {
            if (previousBtn.GetType() == typeof(Button))
            {
                previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                previousBtn.ForeColor = Color.Gainsboro;
                previousBtn.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            }
        }
    }

    private void OpenChildForm(Form childForm, object btnSender)
    {
        if (_activeForm is not null)
        {
            _activeForm.Close();
        }

        ActivateButton(btnSender);
        _activeForm = childForm;
        childForm.TopLevel = false;
        childForm.FormBorderStyle = FormBorderStyle.None;
        childForm.Dock = DockStyle.Fill;
        panelBody.Controls.Add(childForm);
        panelBody.Tag = childForm;
        childForm.BringToFront();
        childForm.Show();
        lblTitle.Text = childForm.Text;
    }

    private void mnuRegister_Click(object sender, EventArgs e)
    {
        using var frm = new FrmRegister();
        frm.ShowDialog();
    }

    private void mnuOptions_Click(object sender, EventArgs e)
    {
        using var frm = new FrmConfig();
        frm.ShowDialog();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        bool result = await Task.Run(() =>
        {
            CoreAudioDevice device = new CoreAudioController().DefaultCaptureCommunicationsDevice;
            Debug.WriteLine($"=============FullName={device.FullName}");
            Debug.WriteLine($"=============DeviceType={device.DeviceType}");
            Debug.WriteLine($"=============InterfaceName={device.InterfaceName}");
            Debug.WriteLine($"=============IsDefaultDevice={device.IsDefaultDevice}");
            Debug.WriteLine($"=============IsMuted={device.IsMuted}");
            Debug.WriteLine($"=============Name={device.Name}");
            // Has ActiveSessionsAsyc option, but still block UI.
            var activeSessions = device.SessionController.ActiveSessions();
            return activeSessions.Any();
        });

        label1.Text = result ? "I am busy." : "I am NOT busy.";
    }

    /// <summary>
    /// Disables the close button in the title bar of the form because the form's closing event only
    /// runs if it was opened at least once, and we can't have two places wrapping things up.
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams param = base.CreateParams;
            param.ClassStyle |= CP_DISABLED_CLOSE_BUTTON;
            return param;
        }
    }

    /// <summary>
    /// Checks to see if a particular form is open, but does not include hidden forms.
    /// </summary>
    /// <param name="form">Form to check for</param>
    /// <returns>True if open, false if not</returns>
    /// <example>
    /// <code>
    /// if (IsFormOpen(typeof(FrmMain)))
    /// {
    ///    // ....
    /// }
    /// </code>
    /// </example>
    public bool IsFormOpen(Type form)
    {
        foreach (Form openForm in Application.OpenForms)
        {
            if (openForm.GetType().Name == form.Name && openForm.Visible)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Overrides the defaults to allow the main form to start hidden. This method is used by the framework,
    /// we do not use it directly. Instead, control it via the <see cref="_startupVisible"/> instance
    /// variable. Then once things are loaded, set this variable to true as well as the form's Visible
    /// property before trying to call show on it. Once show is called, the form's load event will be raised.
    /// </summary>
    /// <param name="value">True for the default and false for hidden startup</param>
    protected override void SetVisibleCore(bool value)
    {
        base.SetVisibleCore(_startupVisible ? value : _startupVisible);
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
            
    }

    private void mnuManage_Click(object sender, EventArgs e)
    {
        if (IsFormOpen(typeof(FrmMain)))
        {
            return;
        }

        // Resets form state that enables the form to load to system tray directly on first load.
        _startupVisible = true;
        this.Visible = _startupVisible;

        // Ensures selection window appears as the active window.
        this.Activate();
    }

    private void notifyIcon1_DoubleClick(object sender, EventArgs e)
    {
        mnuManage_Click(this, EventArgs.Empty);
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        notifyIcon1.Visible = false;

        // Workaround to ensures that the Application.Run() message loop in Program.cs exits when
        // form was never shown. If Close() is used in this scenario, process will remain open.
        Application.Exit();
    }

    private void mnuExit_Click(object sender, EventArgs e)
    {
        exitToolStripMenuItem_Click(this, EventArgs.Empty);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        OpenChildForm(new FrmRegister(), sender);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        OpenChildForm(new FrmConfig(), sender);
    }

    private void button4_Click(object sender, EventArgs e)
    {
        ActivateButton(sender);
    }

    private void button5_Click(object sender, EventArgs e)
    {
        ActivateButton(sender);
    }

    private void button6_Click(object sender, EventArgs e)
    {
        ActivateButton(sender);
    }

    private void btnTitleExit_Click(object sender, EventArgs e)
    {
        if (_activeForm is not null)
        {
            _activeForm.Close();
        }

        Reset();
    }

    private void Reset()
    {
        DisableButton();
        lblTitle.Text = "Main";
        panel3.BackColor = Color.FromArgb(0, 150, 136);
        panel2.BackColor = Color.FromArgb(39, 39, 58);
        _currentButton = null;
        btnTitleExit.Visible = false;
    }
}