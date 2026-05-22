/*
Copyright (C) 2016 by Eric Bataille <e.c.p.bataille@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace ThoNohT.NohBoard.Extra
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Presents a 32bpp ARGB bitmap through <c>UpdateLayeredWindow</c> (Microsoft layered-window sample).
    /// </summary>
    public static class LayeredWindowHelper
    {
        private const byte AcSrcOver = 0x00;

        private const byte AcSrcAlpha = 0x01;

        private const int UlwAlpha = 0x02;

        private const uint BiRgb = 0;

        private const uint DibRgbColors = 0;

        /// <summary>
        /// Converts straight-alpha pixels to premultiplied BGRA (required by <c>UpdateLayeredWindow</c> + <c>ULW_ALPHA</c>).
        /// Without this, lowering alpha while keeping RGB at 255 produces a white haze.
        /// </summary>
        public static void PremultiplyAlpha(Bitmap bitmap)
        {
            if (bitmap == null)
                return;

            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb
                && bitmap.PixelFormat != PixelFormat.Format32bppPArgb)
            {
                return;
            }

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                var byteCount = Math.Abs(data.Stride) * bitmap.Height;
                var pixels = new byte[byteCount];
                Marshal.Copy(data.Scan0, pixels, 0, byteCount);

                for (var i = 0; i < byteCount; i += 4)
                {
                    var b = pixels[i];
                    var g = pixels[i + 1];
                    var r = pixels[i + 2];
                    var a = pixels[i + 3];

                    if (a == 0)
                    {
                        pixels[i] = 0;
                        pixels[i + 1] = 0;
                        pixels[i + 2] = 0;
                        continue;
                    }

                    pixels[i] = (byte)(b * a / 255);
                    pixels[i + 1] = (byte)(g * a / 255);
                    pixels[i + 2] = (byte)(r * a / 255);
                }

                Marshal.Copy(pixels, 0, data.Scan0, byteCount);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        /// <summary>
        /// Updates a layered window using a DIB section (preserves per-pixel alpha; <c>GetHbitmap()</c> does not).
        /// </summary>
        public static bool TryUpdateLayeredWindow(IntPtr hwnd, Bitmap bitmap, Point screenDestination)
        {
            if (hwnd == IntPtr.Zero || bitmap == null)
                return false;

            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb
                && bitmap.PixelFormat != PixelFormat.Format32bppPArgb)
            {
                throw new ArgumentException(
                    "Bitmap must be Format32bppArgb or Format32bppPArgb.",
                    nameof(bitmap));
            }

            var width = bitmap.Width;
            var height = bitmap.Height;
            if (width <= 0 || height <= 0)
                return false;

            var bmi = new BitmapInfo();
            bmi.bmiHeader.biSize = (uint)Marshal.SizeOf<BitmapInfoHeader>();
            bmi.bmiHeader.biWidth = width;
            bmi.bmiHeader.biHeight = -height;
            bmi.bmiHeader.biPlanes = 1;
            bmi.bmiHeader.biBitCount = 32;
            bmi.bmiHeader.biCompression = BiRgb;

            var screenDc = GetDC(IntPtr.Zero);
            var memDc = CreateCompatibleDC(screenDc);
            IntPtr dib = IntPtr.Zero;
            IntPtr dibBits = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                dib = CreateDIBSection(screenDc, ref bmi, DibRgbColors, out dibBits, IntPtr.Zero, 0);
                if (dib == IntPtr.Zero || dibBits == IntPtr.Zero)
                    return false;

                if (!CopyPremultipliedPixelsToDib(bitmap, dibBits, width, height))
                    return false;

                oldBitmap = SelectObject(memDc, dib);

                var dest = new NativePoint(screenDestination.X, screenDestination.Y);
                var size = new NativeSize(width, height);
                var source = new NativePoint(0, 0);
                var blend = new BlendFunction
                {
                    BlendOp = AcSrcOver,
                    BlendFlags = 0,
                    SourceConstantAlpha = 255,
                    AlphaFormat = AcSrcAlpha,
                };

                return UpdateLayeredWindow(
                    hwnd,
                    screenDc,
                    ref dest,
                    ref size,
                    memDc,
                    ref source,
                    0,
                    ref blend,
                    UlwAlpha);
            }
            finally
            {
                if (oldBitmap != IntPtr.Zero)
                    SelectObject(memDc, oldBitmap);

                if (dib != IntPtr.Zero)
                    DeleteObject(dib);

                DeleteDC(memDc);
                ReleaseDC(IntPtr.Zero, screenDc);
            }
        }

        private static bool CopyPremultipliedPixelsToDib(Bitmap bitmap, IntPtr dibBits, int width, int height)
        {
            var rect = new Rectangle(0, 0, width, height);
            var src = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                var dstStride = width * 4;
                var rowBytes = width * 4;

                for (var y = 0; y < height; y++)
                {
                    var srcOffset = IntPtr.Add(src.Scan0, y * src.Stride);
                    var dstOffset = IntPtr.Add(dibBits, y * dstStride);
                    CopyRow(dstOffset, srcOffset, rowBytes);
                }

                return true;
            }
            finally
            {
                bitmap.UnlockBits(src);
            }
        }

        private static void CopyRow(IntPtr dest, IntPtr src, int byteCount)
        {
            var buffer = new byte[byteCount];
            Marshal.Copy(src, buffer, 0, byteCount);
            Marshal.Copy(buffer, 0, dest, byteCount);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UpdateLayeredWindow(
            IntPtr hwnd,
            IntPtr hdcDst,
            ref NativePoint pptDst,
            ref NativeSize psize,
            IntPtr hdcSrc,
            ref NativePoint pptSrc,
            int crKey,
            ref BlendFunction pblend,
            int dwFlags);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateDIBSection(
            IntPtr hdc,
            ref BitmapInfo pbmi,
            uint iUsage,
            out IntPtr ppvBits,
            IntPtr hSection,
            uint dwOffset);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [StructLayout(LayoutKind.Sequential)]
        private struct NativePoint
        {
            public int x;
            public int y;

            public NativePoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeSize
        {
            public int cx;
            public int cy;

            public NativeSize(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BlendFunction
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BitmapInfoHeader
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BitmapInfo
        {
            public BitmapInfoHeader bmiHeader;
        }
    }
}
