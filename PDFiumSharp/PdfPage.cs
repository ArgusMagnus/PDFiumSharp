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
using System.Runtime.InteropServices;

namespace PDFiumSharp
{
    public sealed class PdfPage : IDisposable
    {
		FPDF_PAGE _ptr;

		/// <summary>
		/// Handle which can be used with the native <see cref="PDFium"/> functions.
		/// </summary>
		public FPDF_PAGE Handle
		{
			get
			{
				if (_ptr.IsNull)
					throw new ObjectDisposedException(nameof(PdfPage));
				return _ptr;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="PdfPage"/> was already unloaded.
		/// </summary>
		public bool IsDisposed => _ptr.IsNull;

		/// <summary>
		/// Gets the page width (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public double Width => PDFium.FPDF_GetPageWidth(Handle);

		/// <summary>
		/// Gets the page height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public double Height => PDFium.FPDF_GetPageHeight(Handle);

		/// <summary>
		/// /// <summary>
		/// Gets the page width and height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		/// </summary>
		public (double Width, double Height) Size
		{
			get
			{
				if (PDFium.FPDF_GetPageSizeByIndex(Document.Handle, Index, out var width, out var height))
					return (width, height);
				throw new PDFiumException();
			}
		}

		/// <summary>
		/// Gets the page orientation.
		/// </summary>
		public PageOrientations Orientation
		{
			get => PDFium.FPDFPage_GetRotation(Handle);
			set => PDFium.FPDFPage_SetRotation(Handle, value);
		}

		/// <summary>
		/// Gets the zero-based index of the page in the <see cref="Document"/>
		/// </summary>
		public int Index { get; internal set; } = -1;

		/// <summary>
		/// Gets the <see cref="PdfDocument"/> which contains the page.
		/// </summary>
		public PdfDocument Document { get; }

		PdfPage(PdfDocument doc, FPDF_PAGE page, int index)
		{
			if (page.IsNull)
				throw new PDFiumException();
			Document = doc;
			_ptr = page;
			Index = index;
		}

		internal static PdfPage Load(PdfDocument doc, int index) => new PdfPage(doc, PDFium.FPDF_LoadPage(doc.Handle, index), index);
		internal static PdfPage New(PdfDocument doc, int index, double width, double height) => new PdfPage(doc, PDFium.FPDFPage_New(doc.Handle, index, width, height), index);

		void IDisposable.Dispose()
		{
			if (!_ptr.IsNull)
			{
				PDFium.FPDF_ClosePage(_ptr);
				_ptr = FPDF_PAGE.Null;
			}
		}

		/// <summary>
		/// Renders the page to a <see cref="PDFiumBitmap"/>
		/// </summary>
		public void Render(PDFiumBitmap bitmap, (int left, int top, int width, int height) rectDest, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			PDFium.FPDF_RenderPageBitmap(bitmap.Handle, this.Handle, rectDest.left, rectDest.top, rectDest.width, rectDest.height, rotation, flags);
		}

		/// <summary>
		/// Renders the page to a <see cref="PDFiumBitmap"/>
		/// </summary>
		public void Render(PDFiumBitmap bitmap, PageOrientations rotation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			Render(bitmap, (0, 0, bitmap.Width, bitmap.Height), rotation, flags);
		}
	}
}
