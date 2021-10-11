using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace PDFiumSharp.Native
{
    public static partial class fpdfview
    {
        /// <summary>
        /// Gets a value indicating whether the PDFium library is available.
        /// <c>false</c> is returned if the native libraries could not be
        /// loaded for some reason.
        /// </summary>
        internal static bool IsAvailable { get; }

        static fpdfview()
        {
            IsAvailable = Initialize();

            static bool Initialize()
            {
                try { FPDF_InitLibrary(); }
                catch { return false; }
                return true;
            }
        }

        public static bool FPDF_GetNamedDest(FpdfDocumentT document, int index, [MaybeNullWhen(false)] out FpdfDestT dest, [MaybeNullWhen(false)] out string name)
        {
            name = default;
            dest = default;
            int length = default;
            if (FPDF_GetNamedDest(document, index, IntPtr.Zero, ref length) == null || length < 1)
                return false;

            var buffer = new byte[length];
            using (var pin = PinnedGCHandle.Pin(buffer))
            {
                dest = FPDF_GetNamedDest(document, index, pin.Pointer, ref length);
                if (dest != null)
                    name = Encoding.Unicode.GetString(buffer, 0, (int)length - 2);
                return name != null;
            }
        }
    }
}
