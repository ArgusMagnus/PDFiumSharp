using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
	public sealed class PdfTextPage : DisposableNativeWrapper<Native.FpdfTextpageT>
	{
		public PdfPage Page { get; }

		int _charCount = -2;
		public int CharCount => _charCount > -2 ? _charCount : (_charCount = Native.fpdf_text.FPDFTextCountChars(NativeObject));

		PdfTextPage(PdfPage page, Native.FpdfTextpageT textPage)
			: base(textPage)
		{
			Page = page;
		}

		internal static PdfTextPage Load(PdfPage page) => new(page, Native.fpdf_text.FPDFTextLoadPage(page.NativeObject));

        protected override void Dispose(bool disposing, Native.FpdfTextpageT nativeObj)
            => Native.fpdf_text.FPDFTextClosePage(NativeObject);

        public string GetBoundedText(in RectangleDouble bounds)
            => Native.fpdf_text.FPDFTextGetBoundedText(NativeObject, bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);

        public string GetText(int startIndex = 0, int count = -1) => Native.fpdf_text.FPDFTextGetText(NativeObject, startIndex, count < 0 ? CharCount : count);

		public bool TryGetCharOrigin(int index, out CoordinatesDouble result)
		{
			if (index < 0 || index >= CharCount)
				throw new IndexOutOfRangeException();

            result = default;
			double x = default, y = default;
            if (!Native.fpdf_text.FPDFTextGetCharOrigin(NativeObject, index, ref x, ref y))
                return false;
			result = new(x, y);
            return true;
		}

        public bool TryGetCharBox(int index, out RectangleDouble result)
        {
            if (index < 0 || index >= CharCount)
                throw new IndexOutOfRangeException();

            result = default;
			double left = default, right = default, bottom = default, top = default;
            if (!Native.fpdf_text.FPDFTextGetCharBox(NativeObject, index, ref left, ref right, ref bottom, ref top))
                return false;
            result = new(left, top, right, bottom);
            return true;
        }
	}
}
