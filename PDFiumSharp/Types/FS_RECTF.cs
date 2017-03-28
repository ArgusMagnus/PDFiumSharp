using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	[StructLayout(LayoutKind.Sequential)]
	public class FS_RECTF
    {
		public float Left { get; }
		public float Top { get; }
		public float Right { get; }
		public float Bottom { get; }

		public FS_RECTF(float left, float top, float right, float bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
	}
}
