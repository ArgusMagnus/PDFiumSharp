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
                Native.FpdfBookmarkT nativeObj;
                lock (Document.NativeObject) { nativeObj = Native.fpdf_doc.FPDFBookmarkGetFirstChild(Document.NativeObject, NativeObject); }
                while (nativeObj != null)
                {
                    yield return new PdfBookmark(Document, nativeObj);
                    lock (Document.NativeObject) { nativeObj = Native.fpdf_doc.FPDFBookmarkGetNextSibling(Document.NativeObject, nativeObj); }
                }
            }
        }

        public string Title { get { lock (Document.NativeObject) { return Native.fpdf_doc.FPDFBookmarkGetTitle(NativeObject); } } }

        public PdfDestination Destination
        {
            get
            {
                Native.FpdfDestT handle;
                lock (Document.NativeObject) { handle = Native.fpdf_doc.FPDFBookmarkGetDest(Document.NativeObject, NativeObject); }
                return new(Document, handle, string.Empty);
            }
        }

        public PdfAction Action
        {
            get
            {
                Native.FpdfActionT handle;
                lock (Document.NativeObject) { handle = Native.fpdf_doc.FPDFBookmarkGetAction(NativeObject); }
                return new(Document, handle);
            }
        }

        internal PdfBookmark(PdfDocument doc, Native.FpdfBookmarkT handle)
            : base(handle)
        {
            Document = doc;
        }
    }
}
