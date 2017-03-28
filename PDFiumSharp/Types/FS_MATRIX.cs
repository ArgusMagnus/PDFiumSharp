using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	[StructLayout(LayoutKind.Sequential)]
    public class FS_MATRIX
    {
		public float A { get; }
		public float B { get; }
		public float C { get; }
		public float D { get; }
		public float E { get; }
		public float F { get; }

		public FS_MATRIX(float a, float b, float c, float d, float e, float f)
		{
			A = a;
			B = b;
			C = c;
			D = d;
			E = e;
			F = f;
		}
	}
}
