using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
    public sealed class Bitmap : IDisposable
    {
		FPDF_BITMAP _ptr;
		readonly BitmapFormats _format;

		internal FPDF_BITMAP Pointer
		{
			get
			{
				if (_ptr.IsNull)
					throw new ObjectDisposedException(nameof(Bitmap));
				return _ptr;
			}
		}

		public int Width => PDFium.FPDFBitmap_GetWidth(Pointer);
		public int Height => PDFium.FPDFBitmap_GetHeight(Pointer);
		public int Stride => PDFium.FPDFBitmap_GetStride(Pointer);
		public IntPtr Scan0 => PDFium.FPDFBitmap_GetBuffer(Pointer);

		Bitmap(FPDF_BITMAP bitmap, BitmapFormats format)
		{
			_ptr = bitmap;
			_format = format;
		}

		public Bitmap(int width, int height, bool hasAlpha)
			: this(PDFium.FPDFBitmap_Create(width, height, hasAlpha), hasAlpha ? BitmapFormats.FPDFBitmap_BGRA : BitmapFormats.FPDFBitmap_BGRx) { }

		public Bitmap(int width, int height, BitmapFormats format, IntPtr scan0, int stride)
			: this(PDFium.FPDFBitmap_CreateEx(width, height, format, scan0, stride), format) { }

		public void Dispose()
		{
			if (!_ptr.IsNull)
			{
				PDFium.FPDFBitmap_Destroy(_ptr);
				_ptr = FPDF_BITMAP.Null;
			}
		}
    }
}
