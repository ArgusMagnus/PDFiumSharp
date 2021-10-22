using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PDFiumSharp.Extensions;

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
		public static void Render(this PdfPage page, WriteableBitmap renderTarget, RectangleInt32 rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (renderTarget == null)
				throw new ArgumentNullException(nameof(renderTarget));

			if (rectDest.Left >= renderTarget.PixelWidth || rectDest.Top >= renderTarget.PixelHeight)
				return;

			var bitmapFormat = GetBitmapFormat(renderTarget.Format);
			renderTarget.Lock();
			using (var tmpBitmap = new PDFiumBitmap(renderTarget.PixelWidth, renderTarget.PixelHeight, bitmapFormat, renderTarget.BackBuffer, renderTarget.BackBufferStride))
			{
				page.Render(tmpBitmap, rectDest, orientation, flags);
			}

			var (left, top, _, _, width, height) = rectDest;

			if (left < 0)
			{
				width += left;
				left = 0;
			}
			if (top < 0)
			{
				height += top;
				top = 0;
			}

			width = Math.Min(width, renderTarget.PixelWidth);
			height = Math.Min(height, renderTarget.PixelHeight);

			renderTarget.AddDirtyRect(new Int32Rect(left, top, width, height));
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
			page.Render(renderTarget, new(0, 0, renderTarget.PixelWidth, renderTarget.PixelHeight, true), orientation, flags);
		}

		public static ImageSource CreateImageSource(this PdfPage page, int width, int height, bool withAlpha = true, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			using (var memoryBitmap = new PDFiumBitmap(width, height, withAlpha))
			{
				page.Render(memoryBitmap);
				var bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.DecodePixelWidth = width;
				bitmap.DecodePixelHeight = height;
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.StreamSource = memoryBitmap.AsBmpStream();
				bitmap.EndInit();
				bitmap.StreamSource = null;
				bitmap.Freeze();
				return bitmap;
			}
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

    namespace WpfExtensions
    {
        public static class Int32RectExtensions
        {
            public static Int32Rect ToInt32Rect(this in RectangleInt32 rect)
            {
                var size = rect.Size;
                return new(rect.Left, rect.Top, size.Width, size.Height);
            }

            public static RectangleInt32 ToRectangleInt32(this Int32Rect rect)
                => new RectangleInt32(rect.X, rect.Y, rect.Width, rect.Height, true);
        }
    }
}
