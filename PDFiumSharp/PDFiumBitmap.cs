#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;
using System.IO;
using PDFiumSharp.BMP;
using System.Runtime.InteropServices;

namespace PDFiumSharp
{
	/// <summary>
	/// A bitmap to which a <see cref="PdfPage"/> can be rendered.
	/// </summary>
    public sealed class PDFiumBitmap : NativeWrapper<FPDF_BITMAP>
    {
		public int Width => PDFium.FPDFBitmap_GetWidth(Handle);
		public int Height => PDFium.FPDFBitmap_GetHeight(Handle);
		public int Stride => PDFium.FPDFBitmap_GetStride(Handle);
		public IntPtr Scan0 => PDFium.FPDFBitmap_GetBuffer(Handle);
		public BitmapFormats Format { get; }
		public int BytesPerPixel => GetBytesPerPixel(Format);

		PDFiumBitmap(FPDF_BITMAP bitmap, BitmapFormats format)
			: base(bitmap)
		{
			if (bitmap.IsNull)
				throw new PDFiumException();
			GetBytesPerPixel(format);
			Format = format;
		}

		static int GetBytesPerPixel(BitmapFormats format)
		{
			if (format == BitmapFormats.FPDFBitmap_BGR)
				return 3;
			if (format == BitmapFormats.FPDFBitmap_BGRA || format == BitmapFormats.FPDFBitmap_BGRx)
				return 4;
			if (format == BitmapFormats.FPDFBitmap_Gray)
				return 1;
			throw new ArgumentOutOfRangeException(nameof(format));
		}

		/// <summary>
		/// Creates a new <see cref="PDFiumBitmap"/>. Unmanaged memory is allocated which must
		/// be freed by calling <see cref="Dispose"/>.
		/// </summary>
		/// <param name="width">The width of the new bitmap.</param>
		/// <param name="height">The height of the new bitmap.</param>
		/// <param name="hasAlpha">A value indicating wheter the new bitmap has an alpha channel.</param>
		/// <remarks>
		/// A bitmap created with this overload always uses 4 bytes per pixel.
		/// Depending on <paramref name="hasAlpha"/> the <see cref="Format"/> is then either
		/// <see cref="BitmapFormats.FPDFBitmap_BGRA"/> or <see cref="BitmapFormats.FPDFBitmap_BGRx"/>.
		/// </remarks>
		public PDFiumBitmap(int width, int height, bool hasAlpha)
			: this(PDFium.FPDFBitmap_Create(width, height, hasAlpha), hasAlpha ? BitmapFormats.FPDFBitmap_BGRA : BitmapFormats.FPDFBitmap_BGRx) { }

		/// <summary>
		/// Creates a new <see cref="PDFiumBitmap"/> using memory allocated by the caller.
		/// The caller is responsible for freeing the memory and that the adress stays
		/// valid during the lifetime of the returned <see cref="PDFiumBitmap"/>.
		/// </summary>
		/// <param name="width">The width of the new bitmap.</param>
		/// <param name="height">The height of the new bitmap.</param>
		/// <param name="format">The format of the new bitmap.</param>
		/// <param name="scan0">The adress of the memory block which holds the pixel values.</param>
		/// <param name="stride">The number of bytes per image row.</param>
		public PDFiumBitmap(int width, int height, BitmapFormats format, IntPtr scan0, int stride)
			: this(PDFium.FPDFBitmap_CreateEx(width, height, format, scan0, stride), format) { }

		/// <summary>
		/// Fills a rectangle in the <see cref="PDFiumBitmap"/> with <paramref name="color"/>.
		/// The pixel values in the rectangle are replaced and not blended.
		/// </summary>
		public void FillRectangle(int left, int top, int width, int height, FPDF_COLOR color)
		{
			PDFium.FPDFBitmap_FillRect(Handle, left, top, width, height, color);
		}

		/// <summary>
		/// Fills the whole <see cref="PDFiumBitmap"/> with <paramref name="color"/>.
		/// The pixel values in the rectangle are replaced and not blended.
		/// </summary>
		/// <param name="color"></param>
		public void Fill(FPDF_COLOR color) => FillRectangle(0, 0, Width, Height, color);

		/// <summary>
		/// Saves the <see cref="PDFiumBitmap"/> in the <see href="https://en.wikipedia.org/wiki/BMP_file_format">BMP</see> file format.
		/// </summary>
		public void Save(Stream stream, double dpiX = 0, double dpiY = 0)
		{
			const double MetersPerInch = 0.0254;
			if (Format == BitmapFormats.FPDFBitmap_Gray || Format == BitmapFormats.FPDFBitmap_BGRx)
				throw new NotSupportedException($"Bitmap format {Format} is not yet supported.");

			int ppmX = (int)Math.Round(dpiX / MetersPerInch);
			int ppmY = (int)Math.Round(dpiY / MetersPerInch);

			uint pixelArrayOffset = BmpFileHeader.Size + BitmapInfoHeader.Size;
			ushort bpp = (ushort)(BytesPerPixel * 8);
			uint bmpStride = ((bpp * (uint)Width + 31) / 32) * 4;
			uint pixelArraySize = (uint)bmpStride * (uint)Height;

			var fileHeader = new BmpFileHeader(pixelArrayOffset + pixelArraySize, pixelArrayOffset);
			var infoHeader = new BitmapInfoHeader(Width, Height, bpp, ppmX, ppmY);

			fileHeader.Write(stream);
			infoHeader.Write(stream);
			var buffer = new byte[bmpStride];
			for (int y = Height - 1; y >= 0; y--)
			{
				IntPtr ptr = Scan0 + y * Stride;
				Marshal.Copy(ptr, buffer, 0, Stride);
				stream.Write(buffer, 0, buffer.Length);
			}
		}

		protected override void Dispose(FPDF_BITMAP handle)
		{
			PDFium.FPDFBitmap_Destroy(handle);
		}
	}
}
