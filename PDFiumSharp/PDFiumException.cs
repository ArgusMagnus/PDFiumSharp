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
