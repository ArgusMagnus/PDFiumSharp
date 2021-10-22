using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
    public sealed class PdfLink : NativeWrapper<Native.FpdfLinkT>
	{
		public PdfPage Page { get; }
		public PdfDestination? Destination { get; }
		public PdfAction? Action { get; }

		internal PdfLink(PdfPage page, Native.FpdfLinkT nativeObj)
			:base(nativeObj)
		{
			Page = page;
            Native.FpdfDestT dest;
            lock (page.Document.NativeObject) { dest = Native.fpdf_doc.FPDFLinkGetDest(page.Document.NativeObject, nativeObj); }
			Destination = dest == null ? null : new PdfDestination(page.Document, dest, string.Empty);
            Native.FpdfActionT action;
            lock (page.Document.NativeObject) { action = Native.fpdf_doc.FPDFLinkGetAction(nativeObj); }
            Action = action == null ? null : new PdfAction(page.Document, action);
		}

		public RectangleSingle AnnotationRectangle
		{
			get
			{
				RectangleSingle result = default;
				using Native.FS_RECTF_ rect = new(ref result);
                lock (Page.Document.NativeObject)
                {
                    if (Native.fpdf_doc.FPDFLinkGetAnnotRect(NativeObject, rect))
                        return result;
                }
				return new(float.NaN, float.NaN, float.NaN, float.NaN);
			}
		}
	}
}
