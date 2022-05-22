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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hue_Meetings.Components;

public sealed class Connection
{
    [Flags]
    private enum ConnectionStates
    {
        INTERNET_CONNECTION_MODEM = 0x1,
        INTERNET_CONNECTION_LAN = 0x2,
        INTERNET_CONNECTION_PROXY = 0x4,
        INTERNET_RAS_INSTALLED = 0x10,
        INTERNET_CONNECTION_OFFLINE = 0x20,
        INTERNET_CONNECTION_CONFIGURED = 0x40
    }

    [DllImport("wininet.dll", CharSet = CharSet.Auto)]
    private static extern bool InternetGetConnectedState(ref ConnectionStates lpdwFlags, int dwReserved);

    /// <summary>
    /// Checks to see if the system has an active connection to the Internet.
    /// </summary>
    /// <returns>Connected or not result.</returns>
    public static bool IsInternetAvailable()
    {
        ConnectionStates connectionState = 0;
        return InternetGetConnectedState(ref connectionState, 0);
    }
}