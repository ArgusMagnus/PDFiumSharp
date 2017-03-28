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
using PDFiumSharp.Types;

namespace PDFiumSharp
{
	public sealed class PDFiumException : Exception
	{
		public PDFiumException()
			: base($"PDFium Error: {PDFium.FPDF_GetLastError().GetDescription()}") { }
	}
}
