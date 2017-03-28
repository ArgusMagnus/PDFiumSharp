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

namespace PDFiumSharp.Types
{
	public enum FPDF_ERR : uint
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

	public static class FPDF_ERR_Extension
	{
		public static string GetDescription(this FPDF_ERR error)
		{
			switch(error)
			{
				case FPDF_ERR.SUCCESS: return "No error.";
				case FPDF_ERR.UNKNOWN: return "Unkown error.";
				case FPDF_ERR.FILE: return "File not found or could not be opened.";
				case FPDF_ERR.FORMAT:return "File not in PDF format or corrupted.";
				case FPDF_ERR.PASSWORD:return "Password required or incorrect password.";
				case FPDF_ERR.SECURITY:return "Unsupported security scheme.";
				case FPDF_ERR.PAGE:return "Page not found or content error.";
				case FPDF_ERR.XFALOAD:return "Load XFA error.";
				case FPDF_ERR.XFALAYOUT:return "Layout XFA error.";
				default: return $"{error} (No description available).";
			}
		}
	}
}
