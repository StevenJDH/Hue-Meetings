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

// Ignore Spelling: Auth Pkce

using Hue_Meetings.Services;
using Moq;
using NUnit.Framework;

namespace Hue_Meetings.Tests.Services;

[TestFixture]
public class HueOAuth2PkceServiceTests
{
    [SetUp]
    public void Setup()
    {
        // Method intentionally left empty.
    }

    [TestCase, Description("Should return valid MD5 hash for literal string.")]
    public void Should_ReturnValidMd5Hash_ForLiteralString()
    {
        string expectedHash = "ed076287532e86365e841e92bfc50d8c";
        string hash = HueOAuth2PkceService.GetMd5String("Hello World!");
        Assert.That(hash, Is.EqualTo(expectedHash));
    }
}