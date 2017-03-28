using System;
using System.Collections.Generic;
using System.Text;
using PDFiumSharp.Types;
using System.IO;
using System.Linq;

namespace PDFiumSharp
{
    public sealed class PdfDocument : IDisposable
    {
		FPDF_DOCUMENT _ptr = FPDF_DOCUMENT.Null;

		public FPDF_DOCUMENT Pointer
		{
			get
			{
				if (_ptr.IsNull)
					throw new ObjectDisposedException(nameof(PdfDocument));
				return _ptr;
			}
		}

		public PdfPageCollection Pages { get; }
		public int FileVersion { get { PDFium.FPDF_GetFileVersion(Pointer, out int fileVersion); return fileVersion; } }

		PdfDocument(FPDF_DOCUMENT doc)
		{
			if (doc.IsNull)
				throw new PDFiumException();
			_ptr = doc;
			Pages = new PdfPageCollection(this);
		}

		public PdfDocument()
			: this(PDFium.FPDF_CreateNewDocument()) { }

		public PdfDocument(string fileName, string password = null)
			: this(PDFium.FPDF_LoadDocument(fileName, password)) { }

		public PdfDocument(byte[] data, string password = null)
			: this(PDFium.FPDF_LoadDocument(data, password)) { }

		public PdfDocument(Stream stream, int length, string password = null)
			: this(PDFium.FPDF_LoadDocument(stream, length, password)) { }

		public PdfDocument(Stream stream, string password = null)
			: this(PDFium.FPDF_LoadDocument(stream, password)) { }

		public void Close()
		{
			if (!_ptr.IsNull)
			{
				Pages.Dispose();
				PDFium.FPDF_CloseDocument(_ptr);
				_ptr = FPDF_DOCUMENT.Null;
			}
		}

		void IDisposable.Dispose() => Close();

		public bool Save(Stream stream, SaveFlags flags = SaveFlags.None, int version = 0)
		{
			return PDFium.FPDF_SaveAsCopy(Pointer, stream, flags, version);
		}

		public bool Save(string filename, SaveFlags flags = SaveFlags.None, int version = 0)
		{
			using (var stream = new FileStream(filename, FileMode.Create))
				return Save(stream, flags, version);
		}
	}
}
