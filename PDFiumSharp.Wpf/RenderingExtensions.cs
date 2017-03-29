using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PDFiumSharp
{
	public static class RenderingExtensions
	{
		public static void Render(this PdfPage page, WriteableBitmap bitmap, (int left, int top, int width, int height) rectDest, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			if (rectDest.left >= bitmap.PixelWidth || rectDest.top >= bitmap.PixelHeight)
				return;

			var bitmapFormat = GetBitmapFormat(bitmap.Format);
			bitmap.Lock();
			using (var tmpBitmap = new PDFiumBitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmapFormat, bitmap.BackBuffer, bitmap.BackBufferStride))
			{
				page.Render(tmpBitmap, rectDest, rotation, flags);
			}

			if (rectDest.left < 0)
			{
				rectDest.width += rectDest.left;
				rectDest.left = 0;
			}
			if (rectDest.top < 0)
			{
				rectDest.height += rectDest.top;
				rectDest.top = 0;
			}
			rectDest.width = Math.Min(rectDest.width, bitmap.PixelWidth);
			rectDest.height = Math.Min(rectDest.height, bitmap.PixelHeight);
			bitmap.AddDirtyRect(new Int32Rect(rectDest.left, rectDest.top, rectDest.width, rectDest.height));
			bitmap.Unlock();
		}

		public static void Render(this PdfPage page, WriteableBitmap bitmap, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(bitmap, (0, 0, bitmap.PixelWidth, bitmap.PixelHeight), rotation, flags);
		}

		static BitmapFormats GetBitmapFormat(PixelFormat pixelFormat)
		{
			if (pixelFormat == PixelFormats.Bgra32)
				return BitmapFormats.FPDFBitmap_BGRA;
			if (pixelFormat == PixelFormats.Bgr32)
				return BitmapFormats.FPDFBitmap_BGRx;
			if (pixelFormat == PixelFormats.Bgr24)
				return BitmapFormats.FPDFBitmap_BGR;
			if (pixelFormat == PixelFormats.Gray8)
				return BitmapFormats.FPDFBitmap_Gray;
			throw new NotSupportedException($"Pixel format {pixelFormat} is not supported.");
		}
	}
}
