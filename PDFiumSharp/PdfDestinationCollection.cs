using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
    public sealed class PdfDestinationCollection
    {
		readonly PdfDocument _doc;

		/// 
		public int Count => PDFium.FPDF_CountNamedDests(_doc.Handle);

		internal PdfDestinationCollection(PdfDocument doc)
		{
			_doc = doc;
		}

		public PdfDestination this[string name]
		{
			get
			{
				var handle = PDFium.FPDF_GetNamedDestByName(_doc.Handle, name);
				return handle.IsNull ? null : new PdfDestination(handle, name);
			}
		}

		public PdfDestination this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException(nameof(index));
				(var handle, var name) = PDFium.FPDF_GetNamedDest(_doc.Handle, index);
				return handle.IsNull ? null : new PdfDestination(handle, name);
			}
		}
    }
}
