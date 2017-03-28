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
}
