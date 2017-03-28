#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
	[Flags]
	public enum RenderingFlags : int
	{
		None = 0,

		/// <summary>
		/// Set if annotations are to be rendered.
		/// </summary>
		Annotations = 0x01,

		/// <summary>
		/// Set if using text rendering optimized for LCD display.
		/// </summary>
		LcdText = 0x02,

		/// <summary>
		/// Don't use the native text output available on some platforms
		/// </summary>
		NoNativeText = 0x04,

		/// <summary>
		/// Grayscale output.
		/// </summary>
		Grayscale = 0x08,

		/// <summary>
		/// Set if you want to get some debug info.
		/// </summary>
		DebugInfo = 0x80,

		/// <summary>
		/// Set if you don't want to catch exceptions.
		/// </summary>
		DontCatch = 0x100,

		/// <summary>
		/// Limit image cache size.
		/// </summary>
		LimitImageCache = 0x200,

		/// <summary>
		/// Always use halftone for image stretching.
		/// </summary>
		ForceHalftone = 0x400,

		/// <summary>
		/// Render for printing.
		/// </summary>
		Printing = 0x800,

		/// <summary>
		/// Set to disable anti-aliasing on text.
		/// </summary>
		NoSmoothText = 0x1000,

		/// <summary>
		/// Set to disable anti-aliasing on images.
		/// </summary>
		NoSmoothImage = 0x2000,

		/// <summary>
		/// Set to disable anti-aliasing on paths.
		/// </summary>
		NoSmoothPath = 0x4000,

		/// <summary>
		/// Set whether to render in a reverse Byte order, this flag is only used when rendering to a bitmap.
		/// </summary>
		ReverseByteOrder = 0x10
	}
}
