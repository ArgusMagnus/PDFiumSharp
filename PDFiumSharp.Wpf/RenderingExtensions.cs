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
		public static void Render(this PdfPage page, WriteableBitmap bitmap, int x, int y, int width, int height, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			if (x >= bitmap.PixelWidth || y >= bitmap.PixelHeight)
				return;

			var bitmapFormat = GetBitmapFormat(bitmap.Format);
			bitmap.Lock();
			using (var tmpBitmap = new Bitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmapFormat, bitmap.BackBuffer, bitmap.BackBufferStride))
			{
				page.Render(tmpBitmap, x, y, width, height, rotation, flags);
			}

			if (x < 0)
			{
				width += x;
				x = 0;
			}
			if (y < 0)
			{
				height += y;
				y = 0;
			}
			width = Math.Min(width, bitmap.PixelWidth);
			height = Math.Min(height, bitmap.PixelHeight);
			bitmap.AddDirtyRect(new Int32Rect(x, y, width, height));
			bitmap.Unlock();
		}

		public static void Render(this PdfPage page, WriteableBitmap bitmap, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(bitmap, 0, 0, bitmap.PixelWidth, bitmap.PixelHeight, rotation, flags);
		}

		public static void Render(this PdfPage page, out WriteableBitmap bitmap, RenderingFlags flags = RenderingFlags.None)
		{
			bitmap = new WriteableBitmap((int)page.Width, (int)page.Height, 72, 72, PixelFormats.Bgra32, null);
			page.Render(bitmap, PageOrientations.Normal, flags);
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
