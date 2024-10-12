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

using Hue_Meetings.Exceptions;
using NUnit.Framework;

namespace Hue_Meetings.Tests.Exceptions;

[TestFixture]
public class LinkButtonNotPressedExceptionTests
{
    [TestCase, Description("Should have default message and no cause when thrown without args.")]
    public void Should_HaveDefaultMessageAndNoCause_When_ThrownWithoutArgs()
    {
        Assert.Throws(Is.TypeOf<LinkButtonNotPressedException>()
                .And.InstanceOf<HueMeetingsException>()
                .And.Message.EqualTo("Link button on the Philips Hue bridge was not pressed within 30 seconds.")
                .And.InnerException.Null,
            () => throw new LinkButtonNotPressedException());
    }

    [TestCase, Description("Should have custom message and no cause when thrown with string arg.")]
    public void Should_HaveCustomMessageAndNoCause_When_ThrownWithStringArg()
    {
        Assert.Throws(Is.TypeOf<LinkButtonNotPressedException>()
                .And.InstanceOf<HueMeetingsException>()
                .And.Message.EqualTo("This is a test.")
                .And.InnerException.Null,
            () => throw new LinkButtonNotPressedException("This is a test."));
    }

    [TestCase, Description("Should have custom message and cause when thrown with all args.")]
    public void Should_HaveCustomMessageAndCause_When_ThrownWithAllArgs()
    {
        var ex = new HueMeetingsException("It was me!");
            
        Assert.Throws(Is.TypeOf<LinkButtonNotPressedException>()
                .And.InstanceOf<HueMeetingsException>()
                .And.Message.EqualTo("This is a test.")
                .And.InnerException.InstanceOf<HueMeetingsException>()
                .And.InnerException.Message.EqualTo("It was me!"),
            () => throw new LinkButtonNotPressedException("This is a test.", ex));
    }
}