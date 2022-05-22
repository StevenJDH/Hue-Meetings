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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hue_Meetings.Converters;

internal class JsonColorConverter : JsonConverter<Color>
{
    public override bool CanConvert(Type typeToConvert) => typeof(Color).IsAssignableFrom(typeToConvert);

    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? hexColor = reader.GetString();

        if (String.IsNullOrWhiteSpace(hexColor))
        {
            throw new JsonException("Color value is null, empty, or consists only of white-space characters.");
        }

        return ColorTranslator.FromHtml(hexColor);
    }  

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) =>
        writer.WriteStringValue(ColorTranslator.ToHtml(value));
}