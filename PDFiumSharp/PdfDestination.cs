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
using System.Runtime.InteropServices;

namespace PDFiumSharp
{
    public sealed class PdfDestination : NativeWrapper<FPDF_DEST>
    {
		public PdfDocument Document { get; }
		public string Name { get; }

		public int PageIndex => PDFium.FPDFDest_GetDestPageIndex(Document.Handle, Handle);

		public (float X, float Y, float Zoom) LocationInPage
		{
			get
			{
				if (!PDFium.FPDFDest_GetLocationInPage(Handle, out bool hasX, out bool hasY, out bool hasZ, out float x, out float y, out float z))
					return ((hasX ? x : float.NaN), (hasY ? y : float.NaN), (hasZ ? z : float.NaN));
				return (float.NaN, float.NaN, float.NaN);
			}
		}


		public View GetView() => new View(Handle);

		internal PdfDestination(PdfDocument doc, FPDF_DEST handle, string name)
			:base(handle)
		{
			Document = doc;
			Name = name;
		}

		public enum ViewFitTypes
		{
			Unkown = 0,
			XYZ,
			Fit,
			FitHorizontal,
			FitVertical,
			FitR,
			FitB,
			FitBH,
			FitBV
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public readonly struct View
		{
			readonly float _x;
			readonly float _y;
			readonly float _x2;
			readonly float _y2;

			public float X => _x;
			public float Y => _y;
			public float Z => _x2;
			public float Left => _x;
			public float Top => _y;
			public float Right => _x2;
			public float Bottom => _y2;
			public float Width => Right - Left;
			public float Height => Bottom - Top;

			public ViewFitTypes Type { get; }

			internal View(FPDF_DEST handle)
			{
				_x = float.NaN;
				_y = float.NaN;
				_x2 = float.NaN;
				_y2 = float.NaN;
				Type = (ViewFitTypes)PDFium.FPDFDest_GetView(handle, out var count, ref _x);
			}
		}
	}
}
