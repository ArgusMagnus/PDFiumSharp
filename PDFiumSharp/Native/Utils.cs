using System;
using System.Text;

namespace PDFiumSharp.Native
{
    static class Utils
    {
        public delegate int GetStringHandler(IntPtr buffer, int length);

        public static unsafe string GetUtf16String(GetStringHandler handler, int lengthUnit, bool lengthIncludesTerminator)
        {
            int length = handler(IntPtr.Zero, 0);
            if (length == 0)
                return string.Empty;
            var buffer = new byte[length * lengthUnit];
            using (var pin = PinnedGCHandle.Pin(buffer))
                handler(pin.Pointer, length);
            length *= lengthUnit;
            if (lengthIncludesTerminator)
                length -= 2;
            return Encoding.Unicode.GetString(buffer, 0, length);
        }

        public static string GetAsciiString(GetStringHandler handler, bool lengthIncludesTerminator)
        {
            int length = handler(IntPtr.Zero, 0);
            if (length == 0)
                return string.Empty;
            var buffer = new byte[length];
            using (var pin = PinnedGCHandle.Pin(buffer))
                handler(pin.Pointer, length);
            if (lengthIncludesTerminator)
                length--;
            return Encoding.ASCII.GetString(buffer, 0, (int)length - 1);
        }

        public static string GetUtf8String(GetStringHandler handler, bool lengthIncludesTerminator)
        {
            int length = handler(IntPtr.Zero, 0);
            if (length == 0)
                return string.Empty;
            var buffer = new byte[length];
            using (var pin = PinnedGCHandle.Pin(buffer))
                handler(pin.Pointer, length);
            if (lengthIncludesTerminator)
                length--;
            return Encoding.UTF8.GetString(buffer, 0, (int)length - 1);
        }
    }
}
