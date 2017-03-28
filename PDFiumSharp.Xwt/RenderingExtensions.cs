using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;

namespace PDFiumSharp
{
    public static class RenderingExtensions
    {
		public static void Render(this PdfPage page, BitmapImage bitmap, int x, int y, int width, int height, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			byte[] pixelBuffer = page.Render(x, y, width, height, BitmapFormats.FPDFBitmap_BGRA, rotate, flags);
			width += x;
			height += y;
			int i = 0;
			for (x = Math.Max(0, x); x < width; x++)
			{
				for (y = Math.Max(0, y); y < height; y++)
				{
					byte b = pixelBuffer[i++];
					byte g = pixelBuffer[i++];
					byte r = pixelBuffer[i++];
					byte a = pixelBuffer[i++];
					bitmap.SetPixel(x, y, Color.FromBytes(r, g, b, a));
				}
			}
		}

		public static void Render(this PdfPage page, BitmapImage bitmap, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(bitmap, 0, 0, (int)bitmap.Width, (int)bitmap.Height, rotate, flags);
		}

		public static void Render(this PdfPage page, out BitmapImage bitmap, RenderingFlags flags = RenderingFlags.None)
		{
			ImageBuilder imageBuilder = new ImageBuilder(Math.Round(page.Width), Math.Round(page.Height));
			bitmap = imageBuilder.ToBitmap(ImageFormat.ARGB32);
			page.Render(bitmap, RotateOptions.DontRotate, flags);
		}
	}
}
