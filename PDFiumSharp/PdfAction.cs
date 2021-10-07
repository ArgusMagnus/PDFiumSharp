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
    public sealed class PdfAction : NativeWrapper<Native.FpdfActionT>
    {
        public PdfDocument Document { get; }

        public ActionTypes Type => (ActionTypes)Native.fpdf_doc.FPDFActionGetType(NativeObject);

        public PdfDestination Destination => new PdfDestination(Document, Native.fpdf_doc.FPDFActionGetDest(Document.NativeObject, NativeObject), null);

        public string FilePath => Native.fpdf_doc.FPDFActionGetFilePath(NativeObject);

        public Uri Uri => new Uri(Native.fpdf_doc.FPDFActionGetURIPath(Document.NativeObject, NativeObject));

        internal PdfAction(PdfDocument doc, Native.FpdfActionT nativeObj)
            : base(nativeObj)
        {
            Document = doc;
        }
    }
}
