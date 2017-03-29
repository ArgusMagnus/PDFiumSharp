#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PDFiumSharp
{
	public static class RenderingExtensionsGdiPlus
	{
		public static void Render(this PdfPage page, Bitmap bitmap, (int left, int top, int width, int height) rectDest, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			var format = GetBitmapFormat(bitmap);
			var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
			using (var tmp = new PDFiumBitmap(bitmap.Width, bitmap.Height, format, data.Scan0, data.Stride))
				page.Render(tmp, rectDest, rotation, flags);
			bitmap.UnlockBits(data);
		}

		public static void Render(this PdfPage page, Bitmap bitmap, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(bitmap, (0, 0, bitmap.Width, bitmap.Height), rotation, flags);
		}

		static BitmapFormats GetBitmapFormat(System.Drawing.Bitmap bitmap)
		{
			if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
				return BitmapFormats.FPDFBitmap_BGR;
			if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
				return BitmapFormats.FPDFBitmap_BGRA;
			if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
				return BitmapFormats.FPDFBitmap_BGRx;
			throw new NotSupportedException($"Pixel format {bitmap.PixelFormat} is not supported.");
		}
	}
}
