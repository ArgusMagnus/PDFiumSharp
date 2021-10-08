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
    public sealed class PdfDestination : NativeWrapper<Native.FpdfDestT>
    {
		public PdfDocument Document { get; }
		public string Name { get; }

		public int PageIndex => Native.fpdf_doc.FPDFDestGetDestPageIndex(Document.NativeObject, NativeObject);

		public (float X, float Y, float Zoom) LocationInPage
		{
			get
			{
				bool hasX = default, hasY = default, hasZ = default;
				float x = default, y = default, z = default;
				if (!Native.fpdf_doc.FPDFDestGetLocationInPage(NativeObject, ref hasX, ref hasY, ref hasZ, ref x, ref y, ref z))
					return ((hasX ? x : float.NaN), (hasY ? y : float.NaN), (hasZ ? z : float.NaN));
				return (float.NaN, float.NaN, float.NaN);
			}
		}


		public View GetView() => new View(NativeObject);

		internal PdfDestination(PdfDocument doc, Native.FpdfDestT nativeObj, string name)
			:base(nativeObj)
		{
			Document = doc;
			Name = name;
		}

		public enum ViewFitTypes
		{
			Unkown = 0,
			XYZ,
			/// <summary>
			/// Fit the entire page within the window both vertically and horizontally.
			/// </summary>
			Fit,
			/// <summary>
			/// Fit the entire width of the page within the window.
			/// </summary>
			FitHorizontal,
			/// <summary>
			/// Fit the entire height of the page within the window.
			/// </summary>
			FitVertical,
			FitR,
			/// <summary>
			/// Fit the bounding box within the window both vertically and horizontally.
			/// </summary>
			FitBoundingBox,
			/// <summary>
			/// Fit the entire width of the bounding box within the window.
			/// </summary>
			FitBoundingBoxHorizontal,
			/// <summary>
			/// Fit the entire height of the bounding box within the window.
			/// </summary>
			FitBoundingBoxVertical
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

			internal View(Native.FpdfDestT nativeObj)
			{
				_x = float.NaN;
				_y = float.NaN;
				_x2 = float.NaN;
				_y2 = float.NaN;
				uint count = default;
				Type = (ViewFitTypes)Native.fpdf_doc.FPDFDestGetView(nativeObj, ref count, ref _x);
			}

            public View(float left, float top, float right, float bottom, ViewFitTypes type)
            {
                _x = left;
                _y = top;
                _x2 = right;
                _y2 = bottom;
                Type = type;
            }
        }
	}
}
