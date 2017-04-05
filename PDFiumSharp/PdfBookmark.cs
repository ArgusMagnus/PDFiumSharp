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

namespace PDFiumSharp
{
    public sealed class PdfBookmark : NativeWrapper<FPDF_BOOKMARK>
    {
		public PdfDocument Document { get; }

		public IEnumerable<PdfBookmark> Children
		{
			get
			{
				FPDF_BOOKMARK handle = PDFium.FPDFBookmark_GetFirstChild(Document.Handle, Handle);
				while (!handle.IsNull)
				{
					yield return new PdfBookmark(Document, handle);
					handle = PDFium.FPDFBookmark_GetNextSibling(Document.Handle, handle);
				}
			}
		}

		public string Title => PDFium.FPDFBookmark_GetTitle(Handle);

		public PdfDestination Destination => new PdfDestination(Document, PDFium.FPDFBookmark_GetDest(Document.Handle, Handle), null);

		public PdfAction Action => new PdfAction(Document, PDFium.FPDFBookmark_GetAction(Handle));

		internal PdfBookmark(PdfDocument doc, FPDF_BOOKMARK handle)
			: base(handle)
		{
			Document = doc;
		}
	}
}
