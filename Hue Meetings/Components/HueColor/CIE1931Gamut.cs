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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hue_Meetings.Components.HueColor;

/// <summary>
/// Represents a gamut with red, green and blue primaries in CIE1931 color space.
/// </summary>
public readonly record struct Cie1931Gamut(Cie1931Point Red, Cie1931Point Green, Cie1931Point Blue)
{
    public bool Contains(Cie1931Point point)
    {
        // Arrangement of points in color space:
        // 
        //   ^             G
        //  y|             
        //   |                  R
        //   |   B         
        //   .------------------->
        //                      x
        //
        return IsBelow(Blue, Green, point) &&
               IsBelow(Green, Red, point) &&
               IsAbove(Red, Blue, point);
    }

    private static bool IsBelow(Cie1931Point a, Cie1931Point b, Cie1931Point point)
    {
        double slope = a.X - b.X == 0 ? 0 : (a.Y - b.Y) / (a.X - b.X);
        double intercept = a.Y - slope * a.X;

        double maxY = point.X * slope + intercept;
        return point.Y <= maxY;
    }

    private static bool IsAbove(Cie1931Point blue, Cie1931Point red, Cie1931Point point)
    {
        double slope = blue.X - red.X == 0 ? 0 : (blue.Y - red.Y) / (blue.X - red.X);
        double intercept = blue.Y - slope * blue.X;

        double minY = point.X * slope + intercept;
        return point.Y >= minY;
    }

    public Cie1931Point NearestContainedPoint(Cie1931Point point)
    {
        if (Contains(point))
        {
            // If this gamut already contains the point, then no adjustment is required.
            return point;
        }

        // Find the closest point on each line in the triangle.
        Cie1931Point pAB = GetClosestPointOnLine(Red, Green, point);
        Cie1931Point pAC = GetClosestPointOnLine(Red, Blue, point);
        Cie1931Point pBC = GetClosestPointOnLine(Green, Blue, point);

        //Get the distances per point and see which point is closer to our Point.
        double dAB = GetDistanceBetweenTwoPoints(point, pAB);
        double dAC = GetDistanceBetweenTwoPoints(point, pAC);
        double dBC = GetDistanceBetweenTwoPoints(point, pBC);

        double lowest = dAB;
        Cie1931Point closestPoint = pAB;

        if (dAC < lowest)
        {
            lowest = dAC;
            closestPoint = pAC;
        }

        if (dBC < lowest)
        {
            closestPoint = pBC;
        }

        return closestPoint;
    }

    private static Cie1931Point GetClosestPointOnLine(Cie1931Point a, Cie1931Point b, Cie1931Point p)
    {
        var ap = new Cie1931Point(p.X - a.X, p.Y - a.Y);
        var ab = new Cie1931Point(b.X - a.X, b.Y - a.Y);

        double ab2 = ab.X * ab.X + ab.Y * ab.Y;
        double apAB = ap.X * ab.X + ap.Y * ab.Y;

        double t = apAB / ab2;

        // Bound to ends of line between A and B.
        t = t switch
        {
            < 0.0f => 0.0f,
            > 1.0f => 1.0f,
            _ => t
        };

        return new Cie1931Point(a.X + ab.X * t, a.Y + ab.Y * t);
    }

    private static double GetDistanceBetweenTwoPoints(Cie1931Point one, Cie1931Point two)
    {
        double dx = one.X - two.X; // Horizontal difference.
        double dy = one.Y - two.Y; // Vertical difference.
        return Math.Sqrt(dx * dx + dy * dy);
    }
}