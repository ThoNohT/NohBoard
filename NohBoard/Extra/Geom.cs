//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Shapes.cs" company="Prodrive B.V.">
//     Copyright (c) Prodrive B.V. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------

namespace ThoNohT.NohBoard.Extra
{
    using System.Drawing;
    using static System.Math;

    /// <summary>
    /// A class containing geomitry related functionality.
    /// </summary>
    public static class Geom
    {
        /// <summary>
        /// Converts the parameters for a circle to a rectangle containing the circle.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>The containing rectangle.</returns>
        public static Rectangle CircleToRectangle(TPoint center, int radius)
        {
            return new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
        }

        /// <summary>
        /// Translates a point according the circular definition of a distance.
        /// </summary>
        /// <param name="start">The point to translate.</param>
        /// <param name="distance">The distance to translate the point.</param>
        /// <param name="angle">The angle to translate the point at.</param>
        /// <returns>The translated point.</returns>
        public static TPoint CircularTranslate(this TPoint start, int distance, float angle)
        {
            return new TPoint(start.X + distance * Cos(angle), start.Y + distance * Sin(angle));
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="rad">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static float RadToDeg(float rad)
        {
            return (float)(rad / (2 * PI) * 360);
        }

        /// <summary>
        /// Returns the angle in radians of a speed with two directional components.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <returns>The angle of the speed.</returns>
        public static float GetAngle(this SizeF speed)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator : This just prevents division by 0.
            if (speed.Width == 0)
                return (float)(speed.Height > 0 ? 0 : PI);

            return (speed.Width < 0 ? 1 : 0) * (float)PI + (float)Atan(speed.Height / speed.Width);
        }

        /// <summary>
        /// Returns the length of a speed with two directional components.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <returns>The length of the speed.</returns>
        public static float GetLength(this SizeF speed)
        {
            return (float)Sqrt(Pow(speed.Width, 2) + (float)Pow(speed.Height, 2));
        }

        /// <summary>
        /// Returns the length of a speed with two directional components.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <returns>The length of the speed.</returns>
        public static float GetLength(this Size speed)
        {
            return (float)Sqrt(Pow(speed.Width, 2) + (float)Pow(speed.Height, 2));
        }

        /// <summary>
        /// Returns a new rectangle that is translated to the origin.
        /// </summary>
        /// <param name="rect">The rectangle to translate.</param>
        /// <returns>The translated rectangle.</returns>
        public static Rectangle ToOrigin(this Rectangle rect)
        {
            return new Rectangle(0, 0, rect.Width, rect.Height);
        }
    }
}