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
    public sealed class PdfBookmark : NativeWrapper<Native.FpdfBookmarkT>
    {
        public PdfDocument Document { get; }

        public IEnumerable<PdfBookmark> Children
        {
            get
            {
                var nativeObj = Native.fpdf_doc.FPDFBookmarkGetFirstChild(Document.NativeObject, NativeObject);
                while (nativeObj != null)
                {
                    yield return new PdfBookmark(Document, nativeObj);
                    nativeObj = Native.fpdf_doc.FPDFBookmarkGetNextSibling(Document.NativeObject, nativeObj);
                }
            }
        }

        public string Title => Native.fpdf_doc.FPDFBookmarkGetTitle(NativeObject);

        public PdfDestination Destination => new(Document, Native.fpdf_doc.FPDFBookmarkGetDest(Document.NativeObject, NativeObject), string.Empty);

        public PdfAction Action => new(Document, Native.fpdf_doc.FPDFBookmarkGetAction(NativeObject));

        internal PdfBookmark(PdfDocument doc, Native.FpdfBookmarkT handle)
            : base(handle)
        {
            Document = doc;
        }
    }
}
