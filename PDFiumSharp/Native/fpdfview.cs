using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp.Native
{
    public static partial class fpdfview
    {
        /// <summary>
        /// Gets a value indicating whether the PDFium library is available.
        /// <c>false</c> is returned if the native libraries could not be
        /// loaded for some reason.
        /// </summary>
        public static bool IsAvailable { get; } = Initialize();

        static bool Initialize()
        {
            try { Native.fpdfview.FPDF_InitLibrary(); }
            catch { return false; }
            return true;
        }

        public static FpdfDestT FPDF_GetNamedDest(FpdfDocumentT document, int index, out string name)
        {
            name = default;
            int length = default;
            if (FPDF_GetNamedDest(document, index, IntPtr.Zero, ref length) == null || length < 1)
                return default;

            var buffer = new byte[length];
            using (var pin = PinnedGCHandle.Pin(buffer))
            {
                var dest = FPDF_GetNamedDest(document, index, pin.Pointer, ref length);
                if (dest != null)
                    name = Encoding.Unicode.GetString(buffer, 0, (int)length - 2);
                return dest;
            }
        }
    }
}
