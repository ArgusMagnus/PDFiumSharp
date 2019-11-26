using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
	public sealed class PdfLink : NativeWrapper<FPDF_LINK>
	{
		public PdfPage Page { get; }
		public PdfDestination Destination { get; }
		public PdfAction Action { get; }

		internal PdfLink(PdfPage page, FPDF_LINK handle)
			:base(handle)
		{
			Page = page;
            var dest = PDFium.FPDFLink_GetDest(page.Document.Handle, handle);
            Destination = dest.IsNull ? null : new PdfDestination(page.Document, dest, null);
            var action = PDFium.FPDFLink_GetAction(handle);
            Action = action.IsNull ? null : new PdfAction(page.Document, action);
		}

		public FS_RECTF AnnotationRectangle
		{
			get
			{
				if (PDFium.FPDFLink_GetAnnotRect(Handle, out var rect))
					return rect;
				return new FS_RECTF(float.NaN, float.NaN, float.NaN, float.NaN);
			}
		}
	}
}
