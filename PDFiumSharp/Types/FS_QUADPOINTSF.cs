using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	[StructLayout(LayoutKind.Sequential)]
    public struct FS_QUADPOINTSF
    {
		public float X1 { get; }
		public float Y1 { get; }
		public float X2 { get; }
		public float Y2 { get; }
		public float X3 { get; }
		public float Y3 { get; }
		public float X4 { get; }
		public float Y4 { get; }

		public FS_QUADPOINTSF(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
			X3 = x3;
			Y3 = y3;
			X4 = x4;
			Y4 = y4;
		}
	}
}
