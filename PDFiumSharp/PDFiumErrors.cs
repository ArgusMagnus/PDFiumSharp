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
	public enum PDFiumErrors : uint
	{
		/// <summary>
		/// No error.
		/// </summary>
		SUCCESS = 0,

		/// <summary>
		/// Unknown error.
		/// </summary>
		UNKNOWN = 1,

		/// <summary>
		/// File not found or could not be opened.
		/// </summary>
		FILE = 2,

		/// <summary>
		/// File not in PDF format or corrupted.
		/// </summary>
		FORMAT = 3,

		/// <summary>
		/// Password required or incorrect password.
		/// </summary>
		PASSWORD = 4,

		/// <summary>
		/// Unsupported security scheme.
		/// </summary>
		SECURITY = 5,

		/// <summary>
		/// Page not found or content error.
		/// </summary>
		PAGE = 6,

		/// <summary>
		/// Load XFA error.
		/// </summary>
		XFALOAD = 7,

		/// <summary>
		/// Layout XFA error.
		/// </summary>
		XFALAYOUT = 8
	}

	public static class PDFiumErrorsExtensions
	{
		public static string GetDescription(this PDFiumErrors error)
		{
			switch(error)
			{
				case PDFiumErrors.SUCCESS: return "No error.";
				case PDFiumErrors.UNKNOWN: return "Unkown error.";
				case PDFiumErrors.FILE: return "File not found or could not be opened.";
				case PDFiumErrors.FORMAT:return "File not in PDF format or corrupted.";
				case PDFiumErrors.PASSWORD:return "Password required or incorrect password.";
				case PDFiumErrors.SECURITY:return "Unsupported security scheme.";
				case PDFiumErrors.PAGE:return "Page not found or content error.";
				case PDFiumErrors.XFALOAD:return "Load XFA error.";
				case PDFiumErrors.XFALAYOUT:return "Layout XFA error.";
				default: return $"{error} (No description available).";
			}
		}
	}
}
