using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;
using System.Runtime.InteropServices;

namespace PDFiumSharp
{
    public sealed class PdfPage : IDisposable
    {
		FPDF_PAGE _ptr;

		internal FPDF_PAGE Pointer
		{
			get
			{
				if (_ptr.IsNull)
					throw new ObjectDisposedException(nameof(PdfPage));
				return _ptr;
			}
		}

		public double Width => PDFium.FPDF_GetPageWidth(Pointer);
		public double Height => PDFium.FPDF_GetPageHeight(Pointer);

		PdfPage(FPDF_PAGE page)
		{
			_ptr = page;
		}

		internal static PdfPage Load(PdfDocument doc, int index) => new PdfPage(PDFium.FPDF_LoadPage(doc.Pointer, index));
		internal static PdfPage New(PdfDocument doc, int index, double width, double height) => new PdfPage(PDFium.FPDFPage_New(doc.Pointer, index, width, height));

		public void Dispose()
		{
			if (!_ptr.IsNull)
			{
				PDFium.FPDF_ClosePage(_ptr);
				_ptr = FPDF_PAGE.Null;
			}
		}

		public void Render(Bitmap bitmap, int x, int y, int width, int height, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			PDFium.FPDF_RenderPageBitmap(bitmap.Pointer, this.Pointer, x, y, width, height, rotate, flags);
		}

		public void Render(Bitmap bitmap, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			Render(bitmap, 0, 0, bitmap.Width, bitmap.Height, rotate, flags);
		}

		public void Render(byte[] pixelBuffer, int x, int y, int width, int height, BitmapFormats format = BitmapFormats.FPDFBitmap_BGRA, int stride = 0, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			if (pixelBuffer == null)
				throw new ArgumentNullException(nameof(pixelBuffer));

			int bytesPerPixel = GetBytesPerPixel(format);
			if (stride < 1)
				stride = width * bytesPerPixel;
			else if (stride < width * bytesPerPixel)
				throw new ArgumentException(nameof(stride));
			if (pixelBuffer.Length < height * stride)
				throw new ArgumentException("Buffer is not large enough.", nameof(pixelBuffer));

			using (var pin = PinnedGCHandle.Pin(pixelBuffer))
			using (var bitmap = new Bitmap(width, height, format, pin.Pointer, stride))
			{
				PDFium.FPDF_RenderPageBitmap(bitmap.Pointer, this.Pointer, x, y, width, height, rotate, flags);
			}
		}

		public byte[] Render(int x, int y, int width, int height, BitmapFormats format = BitmapFormats.FPDFBitmap_BGRA, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			int bytesPerPixel = GetBytesPerPixel(format);
			byte[] pixelBuffer = new byte[width * height * bytesPerPixel];
			Render(pixelBuffer, x, y, width, height, format, 0, rotate, flags);
			return pixelBuffer;
		}

		public void Render(byte[] pixelBuffer, BitmapFormats format = BitmapFormats.FPDFBitmap_BGRA, int stride = 0, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			Render(pixelBuffer, 0, 0, (int)Math.Round(Width), (int)Math.Round(Height), format, stride, rotate, flags);
		}

		public byte[] Render(BitmapFormats format = BitmapFormats.FPDFBitmap_BGRA, int stride = 0, RotateOptions rotate = RotateOptions.DontRotate, RenderingFlags flags = RenderingFlags.None)
		{
			return Render(0, 0, (int)Math.Round(Width), (int)Math.Round(Height), format, rotate, flags);
		}

		static int GetBytesPerPixel(BitmapFormats format)
		{
			if (format == BitmapFormats.FPDFBitmap_BGR)
				return 3;
			else if (format == BitmapFormats.FPDFBitmap_BGRA || format == BitmapFormats.FPDFBitmap_BGRx)
				return 4;
			else if (format == BitmapFormats.FPDFBitmap_Gray)
				return 1;
			else
				throw new ArgumentOutOfRangeException(nameof(format));
		}
	}
}
