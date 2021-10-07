using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PDFiumSharp.Native
{
    public static partial class fpdf_doc
    {
        public static string FPDF_GetMetaText(FpdfDocumentT document, string tag)
            => Utils.GetUtf16String((IntPtr buffer, int length) => (int)FPDF_GetMetaText(document, tag, buffer, (uint)length), sizeof(byte), true);

        public static string FPDFBookmarkGetTitle(FpdfBookmarkT bookmark)
            => Utils.GetUtf16String((IntPtr buffer, int length) => (int)FPDFBookmarkGetTitle(bookmark, buffer, (uint)length), sizeof(byte), true);

        public static string FPDFActionGetFilePath(FpdfActionT action)
            => Utils.GetUtf8String((IntPtr buffer, int length) => (int)FPDFActionGetFilePath(action, buffer, (uint)length), true);

        public static string FPDFActionGetURIPath(FpdfDocumentT document, FpdfActionT action)
            => Utils.GetAsciiString((IntPtr buffer, int length) => (int)FPDFActionGetURIPath(document, action, buffer, (uint)length), true);
    }
}
