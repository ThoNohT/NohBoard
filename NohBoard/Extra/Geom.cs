//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Shapes.cs" company="Prodrive B.V.">
//     Copyright (c) Prodrive B.V. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------

namespace ThoNohT.NohBoard.Extra
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using static System.Math;

    /// <summary>
    /// A class containing geomitry related functionality.
    /// </summary>
    public static class Geom
    {
        /// <summary>
        /// Returns the center of a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to get the center of.</param>
        /// <returns>The point that lies in the center of the rectangle.</returns>
        public static TPoint GetCenter(this Rectangle rect)
        {
            return rect.Location + new Size(rect.Width / 2, rect.Height / 2);
        }

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
            return (float)(rad / PI * 180);
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
        public static float Length(this SizeF speed)
        {
            return (float)Sqrt(Pow(speed.Width, 2) + (float)Pow(speed.Height, 2));
        }

        /// <summary>
        /// Rotates the specified <see cref="SizeF"/> by <paramref name="degrees"/> degrees.
        /// </summary>
        /// <param name="speed">The speed to rotate.</param>
        /// <param name="degrees">The number</param>
        /// <returns>The rotated speed.</returns>
        public static SizeF RotateDegrees(this SizeF speed, int degrees)
        {
            /* A rotation vector in radians is defined as :

              | cos(r) -sin(r) |
              | sin(r)  cos(r) |
            */

            var radians = degrees * PI / 180;
            var inputList = new[] { new PointF(speed.Width, speed.Height) };
            var rotationMatrix = new Matrix(
                (float)Cos(radians), (float)-Sin(radians),
                (float)Sin(radians), (float)Cos(radians),
                0, 0);

            rotationMatrix.TransformVectors(inputList);
            return new SizeF(inputList[0].X, inputList[0].Y);
        }

        /// <summary>
        /// Returns the unit vector of the specified speed.
        /// </summary>
        /// <param name="speed">The speed to get the unit vector of.</param>
        /// <returns>The unit vector.</returns>
        public static SizeF GetUnitVector(this SizeF speed)
        {
            /* The unit vector is calculated as

               v * ||v||
            */

            return speed.Multiply(1 / speed.Length());
        }

        /// <summary>
        /// Projects <paramref name="toProject"/> onto <paramref name="projectOn"/>.
        /// </summary>
        /// <param name="toProject">The speed to project.</param>
        /// <param name="projectOn">The speed to project on.</param>
        /// <returns>The projected speed.</returns>
        public static SizeF ProjectOn(this SizeF toProject, SizeF projectOn)
        {
            /* Projecting is easiest on a unit matrix. Therefore we call u the unit matrix of projectOn.
               The projection matrix is then:

               |  ux^2    ux * uy |
               | ux * uy   uy^2   |
          */

            var unitVector = projectOn.GetUnitVector();
            var projectionMatrix = new Matrix(
                unitVector.Width * unitVector.Width, unitVector.Width * unitVector.Height,
                unitVector.Width * unitVector.Height, unitVector.Height * unitVector.Height,
                0, 0);

            var inputList = new[] { new PointF(toProject.Width, toProject.Height) };
            projectionMatrix.TransformVectors(inputList);
            return new SizeF(inputList[0].X, inputList[0].Y);
        }

        /// <summary>
        /// Multiplies the provided speed with a scalar value.
        /// </summary>
        /// <param name="speed">The speed to multiply.</param>
        /// <param name="scalar">The scalar to multiply with.</param>
        /// <returns>The multiplied speed.</returns>
        public static SizeF Multiply(this SizeF speed, float scalar)
        {
            return new SizeF(speed.Width * scalar, speed.Height * scalar);
        }

        /// <summary>
        /// Returns the length of a speed with two directional components.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <returns>The length of the speed.</returns>
        public static float Length(this Size speed)
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