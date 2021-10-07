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

namespace PDFiumSharp
{
    public sealed class PdfDestinationCollection : IEnumerable<PdfDestination>
    {
		readonly PdfDocument _doc;

		/// 
		public int Count => checked((int)Native.fpdfview.FPDF_CountNamedDests(_doc.NativeObject));

		internal PdfDestinationCollection(PdfDocument doc)
		{
			_doc = doc;
		}

		public PdfDestination this[string name]
		{
			get
			{
				var dest = Native.fpdfview.FPDF_GetNamedDestByName(_doc.NativeObject, name);
				return dest == null ? null : new PdfDestination(_doc, dest, name);
			}
		}

		public PdfDestination this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException(nameof(index));
				var dest = Native.fpdfview.FPDF_GetNamedDest(_doc.NativeObject, index, out var name);
				return dest == null ? null : new PdfDestination(_doc, dest, name);
			}
		}

		IEnumerator<PdfDestination> IEnumerable<PdfDestination>.GetEnumerator()
		{
			int count = Count;
			for (int i = 0; i < count; i++)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			int count = Count;
			for (int i = 0; i < count; i++)
				yield return this[i];
		}
	}
}
