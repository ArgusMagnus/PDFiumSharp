using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
	public sealed class PdfPageCollection : IDisposable, IEnumerable<PdfPage>
	{
		readonly PdfDocument _doc;
		readonly List<PdfPage> _pages;

		internal PdfPageCollection(PdfDocument doc)
		{
			_doc = doc;
			_pages = new List<PdfPage>(PDFium.FPDF_GetPageCount(doc.Pointer));
		}

		public int Count => PDFium.FPDF_GetPageCount(_doc.Pointer);

		public PdfPage this[int index]
		{
			get
			{
				if (index >= _pages.Count)
				{
					int count = this.Count;
					if (index >= count)
						throw new ArgumentOutOfRangeException(nameof(index));
					while (_pages.Count < count)
						_pages.Add(null);
				}

				if (_pages[index] == null)
					_pages[index] = PdfPage.Load(_doc, index);
				return _pages[index];
			}
		}

		public void Dispose()
		{
			foreach (var page in _pages)
				page?.Dispose();
			_pages.Clear();
		}

		IEnumerator<PdfPage> IEnumerable<PdfPage>.GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
				yield return this[i];
		}

		public bool Insert(PdfDocument sourceDocument, int index, params int[] srcPageIndices)
		{
			return PDFium.FPDF_ImportPages(_doc.Pointer, sourceDocument.Pointer, index, srcPageIndices);
		}

		public bool Add(PdfDocument sourceDocument, params int[] srcPageIndices)
		{
			return Insert(sourceDocument, Count, srcPageIndices);
		}

		public PdfPage Insert(int index, double width, double height)
		{
			return PdfPage.New(_doc, index, width, height);
		}

		public PdfPage Add(double width, double height)
		{
			return Insert(Count, width, height);
		}

		public void Remove(int index)
		{
			if (index < _pages.Count)
				_pages[index]?.Dispose();
			PDFium.FPDFPage_Delete(_doc.Pointer, index);
		}
	}
}
