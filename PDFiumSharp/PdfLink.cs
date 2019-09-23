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
			Destination = new PdfDestination(page.Document, PDFium.FPDFLink_GetDest(page.Document.Handle, handle), null);
			Action = new PdfAction(page.Document, PDFium.FPDFLink_GetAction(handle));
		}
	}
}
