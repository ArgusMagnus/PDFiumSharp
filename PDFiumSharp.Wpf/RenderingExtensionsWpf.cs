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
	public static class RenderingExtensionsWpf
	{
		/// <summary>
		/// Renders the page to a <see cref="WriteableBitmap"/>
		/// </summary>
		/// <param name="page">The page which is to be rendered.</param>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="rectDest">The destination rectangle in <paramref name="renderTarget"/>.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public static void Render(this PdfPage page, WriteableBitmap renderTarget, (int left, int top, int width, int height) rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (renderTarget == null)
				throw new ArgumentNullException(nameof(renderTarget));

			if (rectDest.left >= renderTarget.PixelWidth || rectDest.top >= renderTarget.PixelHeight)
				return;

			var bitmapFormat = GetBitmapFormat(renderTarget.Format);
			renderTarget.Lock();
			using (var tmpBitmap = new PDFiumBitmap(renderTarget.PixelWidth, renderTarget.PixelHeight, bitmapFormat, renderTarget.BackBuffer, renderTarget.BackBufferStride))
			{
				page.Render(tmpBitmap, rectDest, orientation, flags);
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
			rectDest.width = Math.Min(rectDest.width, renderTarget.PixelWidth);
			rectDest.height = Math.Min(rectDest.height, renderTarget.PixelHeight);
			renderTarget.AddDirtyRect(new Int32Rect(rectDest.left, rectDest.top, rectDest.width, rectDest.height));
			renderTarget.Unlock();
		}

		/// <summary>
		/// Renders the page to a <see cref="WriteableBitmap"/>
		/// </summary>
		/// <param name="page">The page which is to be rendered.</param>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public static void Render(this PdfPage page, WriteableBitmap renderTarget, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(renderTarget, (0, 0, renderTarget.PixelWidth, renderTarget.PixelHeight), orientation, flags);
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
