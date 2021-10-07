using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PDFiumSharp.Native
{
    public static partial class fpdf_text
    {
        public static unsafe string FPDFTextGetBoundedText(FpdfTextpageT text_page, double left, double top, double right, double bottom)
            => Utils.GetUtf16String((IntPtr buffer, int length) => FPDFTextGetBoundedText(text_page, left, top, right, bottom, ref Unsafe.AsRef<ushort>(buffer.ToPointer()), length), sizeof(ushort), false);

        public static string FPDFTextGetText(FpdfTextpageT text_page, int start_index, int count)
        {
            var buffer = new byte[sizeof(ushort) * (count + 1)];
            int length = FPDFTextGetText(text_page, start_index, count, ref Unsafe.As<byte, ushort>(ref buffer[0]));
            return Encoding.Unicode.GetString(buffer, 0, (length - 1) * sizeof(ushort));
        }
    }
}
