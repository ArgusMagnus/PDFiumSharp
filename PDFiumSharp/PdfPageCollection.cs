#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

		public bool Insert(int index, PdfDocument sourceDocument, params int[] srcPageIndices)
		{
			if (index < _pages.Count)
				_pages.Insert(index, null);
			var result = PDFium.FPDF_ImportPages(_doc.Pointer, sourceDocument.Pointer, index, srcPageIndices);
			_pages.InsertRange(index, Enumerable.Repeat<PdfPage>(null, srcPageIndices.Length));
			for (int i = index; i < _pages.Count; i++)
			{
				if (_pages[i] != null)
					_pages[i].Index = i;
			}
			return result;
		}

		public bool Add(PdfDocument sourceDocument, params int[] srcPageIndices)
		{
			return Insert(Count, sourceDocument, srcPageIndices);
		}

		public PdfPage Insert(int index, double width, double height)
		{
			if (index < _pages.Count)
			{
				_pages.Insert(index, null);
				for (int i = index; i < _pages.Count; i++)
				{
					if (_pages[i] != null)
						_pages[i].Index = i;
				}
			}
			var page = PdfPage.New(_doc, index, width, height);
			while (_pages.Count <= index)
				_pages.Add(null);
			_pages[index] = page;
			return page;
		}

		public PdfPage Add(double width, double height)
		{
			return Insert(Count, width, height);
		}

		public void Remove(int index)
		{
			if (index < _pages.Count)
			{
				_pages[index]?.Dispose();
				_pages.RemoveAt(index);
				for (int i = index; i < _pages.Count; i++)
				{
					if (_pages[i] != null)
						_pages[i].Index = i;
				}
			}
			PDFium.FPDFPage_Delete(_doc.Pointer, index);
		}
	}
}
