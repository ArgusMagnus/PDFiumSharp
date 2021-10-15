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
using System.Diagnostics.CodeAnalysis;

namespace PDFiumSharp
{
    public sealed class PdfDestinationCollection : IEnumerable<PdfDestination>
    {
		readonly PdfDocument _doc;

		public int Count { get { lock (_doc.NativeObject) { return checked((int)Native.fpdfview.FPDF_CountNamedDests(_doc.NativeObject)); } } }

		internal PdfDestinationCollection(PdfDocument doc)
		{
			_doc = doc;
		}

		public PdfDestination this[string name] => TryGetDestination(name, out var value) ? value : throw new KeyNotFoundException();

		public PdfDestination this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException(nameof(index));
                Native.FpdfDestT? dest;
                string? name;
                lock (_doc.NativeObject)
                {
                    if (!Native.fpdfview.FPDF_GetNamedDest(_doc.NativeObject, index, out dest, out name))
                        throw new KeyNotFoundException();
                }
				return new(_doc, dest, name);
			}
		}

        public bool TryGetDestination(string name, [MaybeNullWhen(false)] out PdfDestination value)
        {
            value = default;
            Native.FpdfDestT dest;
            lock (_doc.NativeObject) { dest = Native.fpdfview.FPDF_GetNamedDestByName(_doc.NativeObject, name); }
            if (dest == null)
                return false;
            value = new(_doc, dest, name);
            return true;
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
