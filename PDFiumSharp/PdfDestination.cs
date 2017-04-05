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
    public sealed class PdfDestination : NativeWrapper<FPDF_DEST>
    {
		public PdfDocument Document { get; }
		public string Name { get; }

		public int PageIndex => PDFium.FPDFDest_GetPageIndex(Document.Handle, Handle);

		public (float X, float Y, float Zoom) LocationInPage
		{
			get
			{
				if (!PDFium.FPDFDest_GetLocationInPage(Handle, out bool hasX, out bool hasY, out bool hasZ, out float x, out float y, out float z))
					return ((hasX ? x : float.NaN), (hasY ? y : float.NaN), (hasZ ? z : float.NaN));
				return (float.NaN, float.NaN, float.NaN);
			}
		}

		internal PdfDestination(PdfDocument doc, FPDF_DEST handle, string name)
			:base(handle)
		{
			Document = doc;
			Name = name;
		}
	}
}
