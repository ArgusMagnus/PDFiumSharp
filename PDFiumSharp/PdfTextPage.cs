using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
	public sealed class PdfTextPage : NativeWrapper<FPDF_TEXTPAGE>
	{
		public PdfPage Page { get; }

		int _charCount = -2;
		public int CharCount => _charCount > -2 ? _charCount : (_charCount = PDFium.FPDFText_CountChars(Handle));

		PdfTextPage(PdfPage page, FPDF_TEXTPAGE textPage)
			: base(textPage)
		{
			if (textPage.IsNull)
				throw new PDFiumException();
			Page = page;
		}

		internal static PdfTextPage Load(PdfPage page) => new PdfTextPage(page, PDFium.FPDFText_LoadPage(page.Handle));

		protected override void Dispose(FPDF_TEXTPAGE handle)
		{
			PDFium.FPDFText_ClosePage(handle);
		}

		public string GetBoundedText(double left, double top, double right, double bottom)
			=> PDFium.FPDFText_GetBoundedText(Handle, left, top, right, bottom);

		public string GetText(int startIndex = 0, int count = -1) => PDFium.FPDFText_GetText(Handle, startIndex, count < 0 ? CharCount : count);

		public (double X, double Y) GetCharOrigin(int index)
		{
			if (index < 0 || index >= CharCount)
				throw new IndexOutOfRangeException();

			if (PDFium.FPDFText_GetCharOrigin(Handle, index, out var x, out var y))
				return (x, y);
			return (double.NaN, double.NaN);
		}
	}
}
