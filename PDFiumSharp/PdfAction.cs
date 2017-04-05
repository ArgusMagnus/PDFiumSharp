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
    public sealed class PdfAction : NativeWrapper<FPDF_ACTION>
    {
		public PdfDocument Document { get; }

		public ActionTypes Type => PDFium.FPDFAction_GetType(Handle);

		public PdfDestination Destination => new PdfDestination(Document, PDFium.FPDFAction_GetDest(Document.Handle, Handle), null);

		public string FilePath => PDFium.FPDFAction_GetFilePath(Handle);

		public Uri Uri => new Uri(PDFium.FPDFAction_GetURIPath(Document.Handle, Handle));

		internal PdfAction(PdfDocument doc, FPDF_ACTION actionHandle)
			:base(actionHandle)
		{
			Document = doc;
		}
    }
}
