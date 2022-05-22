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
using System.Text;
using System.Threading.Tasks;

namespace Hue_Meetings.Components
{
    public static class ThemeColor
    {
        public static Color PrimaryColor { get; set; }

        public static Color SecondaryColor { get; set; }

        public static IReadOnlyList<string> ColorList { get; } = new List<string>
        {
            "#3F51B5",
            "#009688",
            "#FF5722",
            "#607D8B",
            "#FF9800",
            "#9C27B0",
            "#2196F3",
            "#EA676C",
            "#E41A4A",
            "#5978BB",
            "#018790",
            "#0E3441",
            "#00B0AD",
            "#721D47",
            "#EA4833",
            "#EF937E",
            "#F37521",
            "#A12059",
            "#126881",
            "#8BC240",
            "#364D5B",
            "#C7DC5B",
            "#0094BC",
            "#E4126B",
            "#43B76E",
            "#7BCFE9",
            "#B71C46"
        };

        public static Color ChangeColorBrightness(Color color, double correctionFactor)
        {
            double red = color.R;
            double green = color.G;
            double blue = color.B;

            // Darken color if correction factor is less than 0, otherwise lighten color.
            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}
