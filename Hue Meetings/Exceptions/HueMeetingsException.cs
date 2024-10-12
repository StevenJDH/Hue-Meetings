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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hue_Meetings.Exceptions;

/// <summary>
/// Indicates that a general Hue API fault has occurred.
/// </summary>
[Serializable]
public class HueMeetingsException : Exception
{
    private static readonly string DefaultMessage = "A general fault was encountered.";

    public HueMeetingsException() : base(DefaultMessage)
    {}

    public HueMeetingsException(string? message) : base(message)
    {}

    public HueMeetingsException(string? message, Exception? innerException) : base(message, innerException)
    {}

    protected HueMeetingsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {}
}