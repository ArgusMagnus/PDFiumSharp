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
    public sealed class PdfPage : DisposableNativeWrapper<Native.FpdfPageT>
    {
		/// <summary>
		/// Gets the page width (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public double Width => Native.fpdfview.FPDF_GetPageWidth(NativeObject);

		/// <summary>
		/// Gets the page height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public double Height => Native.fpdfview.FPDF_GetPageHeight(NativeObject);

		/// <summary>
		/// Gets the page width and height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public SizeDouble Size
		{
			get
			{
				double width = default, height = default;
				if (Native.fpdfview.FPDF_GetPageSizeByIndex(Document.NativeObject, Index, ref width, ref height) != 0)
					return new(width, height);
				throw new PDFiumException();
			}
		}

		/// <summary>
		/// Gets the page orientation.
		/// </summary>
		public PageOrientations Orientation
		{
			get => (PageOrientations)Native.fpdf_edit.FPDFPageGetRotation(NativeObject);
			set => Native.fpdf_edit.FPDFPageSetRotation(NativeObject, (int)value);
		}

		/// <summary>
		/// Gets the zero-based index of the page in the <see cref="Document"/>
		/// </summary>
		public int Index { get; internal set; } = -1;

		/// <summary>
		/// Gets the <see cref="PdfDocument"/> which contains the page.
		/// </summary>
		public PdfDocument Document { get; }

		//public string Label => PDFium.FPDF_GetPageLabel(Document.Handle, Index);

		PdfPage(PdfDocument doc, Native.FpdfPageT nativeObj, int index)
			: base(nativeObj)
		{
			Document = doc;
			Index = index;
		}

		internal static PdfPage Load(PdfDocument doc, int index) => new(doc, Native.fpdfview.FPDF_LoadPage(doc.NativeObject, index), index);
		internal static PdfPage New(PdfDocument doc, int index, double width, double height) => new(doc, Native.fpdf_edit.FPDFPageNew(doc.NativeObject, index, width, height), index);

		/// <summary>
		/// Renders the page to a <see cref="PDFiumBitmap"/>
		/// </summary>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="rectDest">The destination rectangle in <paramref name="renderTarget"/>.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public void Render(PDFiumBitmap renderTarget, RectangleInt32 rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (renderTarget == null)
				throw new ArgumentNullException(nameof(renderTarget));

			Native.fpdfview.FPDF_RenderPageBitmap(renderTarget.NativeObject, NativeObject, rectDest.Left, rectDest.Top, rectDest.Width, rectDest.Height, (int)orientation, (int)flags);
		}

		/// <summary>
		/// Renders the page to a <see cref="PDFiumBitmap"/>
		/// </summary>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public void Render(PDFiumBitmap renderTarget, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			Render(renderTarget, new(0, 0, renderTarget.Width, renderTarget.Height), orientation, flags);
		}

		public CoordinatesDouble DeviceToPage(RectangleInt32 displayArea, CoordinatesInt32 coordDevice, PageOrientations orientation = PageOrientations.Normal)
		{
			double x = default, y = default;
			Native.fpdfview.FPDF_DeviceToPage(NativeObject, displayArea.Left, displayArea.Top, displayArea.Width, displayArea.Height, (int)orientation, coordDevice.X, coordDevice.Y, ref x, ref y);
			return new(x, y);
		}

		public CoordinatesInt32 PageToDevice(RectangleInt32 displayArea, CoordinatesDouble coordPage, PageOrientations orientation = PageOrientations.Normal)
		{
			int x = default, y = default;
			Native.fpdfview.FPDF_PageToDevice(NativeObject, displayArea.Left, displayArea.Top, displayArea.Width, displayArea.Height, (int)orientation, coordPage.X, coordPage.Y, ref x, ref y);
			return new(x, y);
		}

		public FlattenResults Flatten(FlattenFlags flags) => (FlattenResults)Native.fpdf_flatten.FPDFPageFlatten(NativeObject, (int)flags);

        protected override void Dispose(bool disposing, Native.FpdfPageT nativeObj)
            => Native.fpdfview.FPDF_ClosePage(nativeObj);

        public PdfTextPage GetTextPage() => PdfTextPage.Load(this);

		public IEnumerable<PdfLink> Links
		{
			get
			{
				int idx = 0;
				var nativeObj = Native.FpdfLinkT.__CreateInstance(IntPtr.Zero);
				while (Native.fpdf_doc.FPDFLinkEnumerate(NativeObject, ref idx, nativeObj))
					yield return new(this, Native.FpdfLinkT.__CreateInstance(nativeObj.__Instance));
			}
		}
	}
}
