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
	[StructLayout(LayoutKind.Sequential)]
	public struct FS_RECTF
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
