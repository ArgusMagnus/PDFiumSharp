using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
	public sealed class PdfTextPage : NativeWrapper<Native.FpdfTextpageT>
	{
		public PdfPage Page { get; }

		int _charCount = -2;
		public int CharCount => _charCount > -2 ? _charCount : (_charCount = Native.fpdf_text.FPDFTextCountChars(NativeObject));

		PdfTextPage(PdfPage page, Native.FpdfTextpageT textPage)
			: base(textPage)
		{
			Page = page;
		}

		internal static PdfTextPage Load(PdfPage page) => new PdfTextPage(page, Native.fpdf_text.FPDFTextLoadPage(page.NativeObject));

		public void Dispose()
		{
			if (SetNativeObjectToNull(out var nativeObject))
				Native.fpdf_text.FPDFTextClosePage(NativeObject);
		}

		public string GetBoundedText(RectangleDouble bounds)
			=> Native.fpdf_text.FPDFTextGetBoundedText(NativeObject, bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);

		public string GetText(int startIndex = 0, int count = -1) => Native.fpdf_text.FPDFTextGetText(NativeObject, startIndex, count < 0 ? CharCount : count);

		public CoordinatesDouble GetCharOrigin(int index)
		{
			if (index < 0 || index >= CharCount)
				throw new IndexOutOfRangeException();

			double x = default, y = default;
			if (Native.fpdf_text.FPDFTextGetCharOrigin(NativeObject, index, ref x, ref y))
				return new(x, y);
			return new(double.NaN, double.NaN);
		}

        public RectangleDouble GetCharBox(int index)
        {
            if (index < 0 || index >= CharCount)
                throw new IndexOutOfRangeException();

			double left = default, right = default, bottom = default, top = default;
            if (Native.fpdf_text.FPDFTextGetCharBox(NativeObject, index, ref left, ref right, ref bottom, ref top))
                return new(left, top, right, bottom);
            return new(double.NaN, double.NaN, double.NaN, double.NaN);
        }
	}
}
