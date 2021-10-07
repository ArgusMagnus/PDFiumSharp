﻿#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Runtime.InteropServices;
using PDFiumSharp.Types;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace PDFiumSharp
{
	/// <summary>
	/// Static class containing the native (imported) PDFium functions.
	/// In case of missing documentation, refer to the <see href="https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4660/public">PDFium header files</see>. 
	/// </summary>
	public static class PDFium
    {
		/// <summary>
		/// Gets a value indicating whether the PDFium library is available.
		/// <c>false</c> is returned if the native libraries could not be
		/// loaded for some reason.
		/// </summary>
		public static bool IsAvailable => Native.fpdfview.IsAvailable;
	}
}
