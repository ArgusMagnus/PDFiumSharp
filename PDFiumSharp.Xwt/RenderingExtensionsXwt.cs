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
using Xwt;
using Xwt.Drawing;

namespace PDFiumSharp
{
    public static class RenderingExtensionsXwt
    {
		public static void Render(this PdfPage page, BitmapImage bitmap, (int left, int top, int width, int height) rectDest, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			(var left, var top, var width, var height) = rectDest;

			byte[] pixelBuffer = new byte[width * height * 4];
			using (var pin = PinnedGCHandle.Pin(pixelBuffer))
			using (var tmpBitamp = new PDFiumBitmap(rectDest.width, rectDest.height, BitmapFormats.FPDFBitmap_BGRA, pin.Pointer, rectDest.width * 4))
			{
				page.Render(tmpBitamp, (0, 0, width, height), rotation, flags);
				for (int y = Math.Max(0, top); y < Math.Min(bitmap.PixelHeight, top + height); y++)
				{
					int i = ((y - top) * width - left) * 4;
					for (int x = Math.Max(0, left); x < Math.Min(bitmap.PixelWidth, left+width); x++)
					{
						byte b = pixelBuffer[i++];
						byte g = pixelBuffer[i++];
						byte r = pixelBuffer[i++];
						byte a = pixelBuffer[i++];
						bitmap.SetPixel(x, y, Color.FromBytes(r, g, b, a));
					}
				}
			}
		}

		public static void Render(this PdfPage page, BitmapImage bitmap, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(bitmap, (0, 0, (int)bitmap.PixelWidth, (int)bitmap.PixelHeight), rotation, flags);
		}
	}
}
