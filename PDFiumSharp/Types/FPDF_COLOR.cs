#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	[StructLayout(LayoutKind.Explicit)]
	public struct FPDF_COLOR
	{
		[FieldOffset(0)]
		readonly byte _a;
		[FieldOffset(1)]
		readonly byte _r;
		[FieldOffset(2)]
		readonly byte _g;
		[FieldOffset(3)]
		readonly byte _b;
		[FieldOffset(0)]
		readonly uint _argb;

		public byte A => _a;
		public byte R => _b;
		public byte G => _g;
		public byte B => _b;
		public int ARGB => unchecked((int)_argb);

		public FPDF_COLOR(byte r, byte g, byte b, byte a = 255)
		{
			_argb = 0;
			_a = a;
			_r = r;
			_g = g;
			_b = b;
		}

		public FPDF_COLOR(int argb)
		{
			_a = 0;
			_r = 0;
			_g = 0;
			_b = 0;
			_argb = unchecked((uint)argb);
		}

		FPDF_COLOR(uint argb)
		{
			_a = 0;
			_r = 0;
			_g = 0;
			_b = 0;
			_argb = argb;
		}

		public static implicit operator FPDF_COLOR(uint argb) => new FPDF_COLOR(argb);
	}
}
