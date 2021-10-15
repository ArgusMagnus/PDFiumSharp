using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

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


        public static unsafe bool FPDFLinkEnumerate(global::PDFiumSharp.Native.FpdfPageT page, ref int start_pos, [MaybeNullWhen(false)] out global::PDFiumSharp.Native.FpdfLinkT link_annot)
        {
            var __arg0 = page is null ? IntPtr.Zero : page.__Instance;
            fixed (int* __start_pos1 = &start_pos)
            {
                var __arg1 = __start_pos1;
                var ____arg2 = IntPtr.Zero;
                var __arg2 = new IntPtr(&____arg2);
                var __ret = __Internal.FPDFLinkEnumerate(__arg0, __arg1, __arg2);
                link_annot = __ret ? FpdfLinkT.__CreateInstance(____arg2) : null;
                return __ret;
            }
        }
    }
}
