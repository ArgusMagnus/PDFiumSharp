using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
    public sealed class PdfDestination : NativeWrapper<FPDF_DEST>
    {
		public string Name { get; }

		internal PdfDestination(FPDF_DEST handle, string name)
			:base(handle)
		{
			Name = name;
		}

		protected override void Dispose(FPDF_DEST handle)
		{
		}
	}
}
