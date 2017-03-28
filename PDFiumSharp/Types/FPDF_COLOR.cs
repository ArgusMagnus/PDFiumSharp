using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FPDF_COLOR
	{
		public byte A { get; }
		public byte R { get; }
		public byte G { get; }
		public byte B { get; }

		public FPDF_COLOR(byte r, byte g, byte b, byte a = 255)
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}
    }
}
