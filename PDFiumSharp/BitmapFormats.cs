using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
    public enum BitmapFormats : int
    {
		/// <summary>
		/// Gray scale bitmap, one byte per pixel.
		/// </summary>
		FPDFBitmap_Gray = 1,

		/// <summary>
		/// 3 bytes per pixel, byte order: blue, green, red.
		/// </summary>
		FPDFBitmap_BGR = 2,

		/// <summary>
		/// 4 bytes per pixel, byte order: blue, green, red, unused.
		/// </summary>
		FPDFBitmap_BGRx = 3,

		/// <summary>
		/// 4 bytes per pixel, byte order: blue, green, red, alpha.
		/// </summary>
		FPDFBitmap_BGRA = 4
	}
}
