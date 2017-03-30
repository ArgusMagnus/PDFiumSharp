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
		/// <summary>
		/// Renders the page to a <see cref="BitmapImage"/>
		/// </summary>
		/// <param name="page">The page which is to be rendered.</param>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="rectDest">The destination rectangle in <paramref name="renderTarget"/>.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public static void Render(this PdfPage page, BitmapImage renderTarget, (int left, int top, int width, int height) rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (renderTarget == null)
				throw new ArgumentNullException(nameof(renderTarget));

			(var left, var top, var width, var height) = rectDest;

			byte[] pixelBuffer = new byte[width * height * 4];
			using (var pin = PinnedGCHandle.Pin(pixelBuffer))
			using (var tmpBitamp = new PDFiumBitmap(rectDest.width, rectDest.height, BitmapFormats.FPDFBitmap_BGRA, pin.Pointer, rectDest.width * 4))
			{
				page.Render(tmpBitamp, (0, 0, width, height), orientation, flags);
				for (int y = Math.Max(0, top); y < Math.Min(renderTarget.PixelHeight, top + height); y++)
				{
					int i = ((y - top) * width - left) * 4;
					for (int x = Math.Max(0, left); x < Math.Min(renderTarget.PixelWidth, left+width); x++)
					{
						byte b = pixelBuffer[i++];
						byte g = pixelBuffer[i++];
						byte r = pixelBuffer[i++];
						byte a = pixelBuffer[i++];
						renderTarget.SetPixel(x, y, Color.FromBytes(r, g, b, a));
					}
				}
			}
		}

		/// <summary>
		/// Renders the page to a <see cref="BitmapImage"/>
		/// </summary>
		/// <param name="page">The page which is to be rendered.</param>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public static void Render(this PdfPage page, BitmapImage renderTarget, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(renderTarget, (0, 0, (int)renderTarget.PixelWidth, (int)renderTarget.PixelHeight), orientation, flags);
		}
	}
}
