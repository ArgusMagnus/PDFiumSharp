using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Native
{
    public partial class FPDF_FILEWRITE_
    {
        public delegate bool FileWriteBlockHandler(IntPtr data, int size);

        public FPDF_FILEWRITE_(FileWriteBlockHandler writeBlock)
            :this()
        {
            Version = 1;
            WriteBlock = (ignore, data, size) => writeBlock(data, checked((int)size)) ? 1 : 0;
        }

        public static FPDF_FILEWRITE_ FromStream(Stream stream)
        {
            byte[]? buffer = null;
            return new FPDF_FILEWRITE_((data, size) =>
            {
                if (buffer == null || buffer.Length < size)
                    buffer = new byte[size];
                Marshal.Copy(data, buffer, 0, size);
                stream.Write(buffer, 0, size);
                return true;
            });
        }
    }
}
