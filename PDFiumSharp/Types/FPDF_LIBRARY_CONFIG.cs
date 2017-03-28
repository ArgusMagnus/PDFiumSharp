using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	[StructLayout(LayoutKind.Sequential)]
    public struct FPDF_LIBRARY_CONFIG
    {
		readonly int _version;
		readonly IntPtr _userFontPaths;
		readonly IntPtr _v8Isolate;
		readonly uint _v8EmbedderSlot;

		public int Version => _version;
    }
}
