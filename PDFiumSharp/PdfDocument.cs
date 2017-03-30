#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
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

		/// <summary>
		/// Handle which can be used with the native <see cref="PDFium"/> functions.
		/// </summary>
		public FPDF_DOCUMENT Handle
		{
			get
			{
				if (_ptr.IsNull)
					throw new ObjectDisposedException(nameof(PdfDocument));
				return _ptr;
			}
		}

		/// <summary>
		/// Gets the pages in the current <see cref="PdfDocument"/>.
		/// </summary>
		public PdfPageCollection Pages { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="PdfDocument"/> was already closed.
		/// </summary>
		public bool IsDisposed => _ptr.IsNull;

		/// <summary>
		/// Gets the PDF file version. File version: 14 for 1.4, 15 for 1.5, ...
		/// </summary>
		public int FileVersion { get { PDFium.FPDF_GetFileVersion(Handle, out int fileVersion); return fileVersion; } }

		/// <summary>
		/// Gets the revision of the security handler.
		/// </summary>
		/// <seealso cref="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference: Table 21</seealso>
		public int SecurityHandlerRevision => PDFium.FPDF_GetSecurityHandlerRevision(Handle);

		public DocumentPermissions Permissions => PDFium.FPDF_GetDocPermissions(Handle);

		PdfDocument(FPDF_DOCUMENT doc)
		{
			if (doc.IsNull)
				throw new PDFiumException();
			_ptr = doc;
			Pages = new PdfPageCollection(this);
		}

		/// <summary>
		/// Creates a new <see cref="PdfDocument"/>.
		/// <see cref="Close"/> must be called in order to free unmanaged resources.
		/// </summary>
		public PdfDocument()
			: this(PDFium.FPDF_CreateNewDocument()) { }

		/// <summary>
		/// Loads a <see cref="PdfDocument"/> from the file system.
		/// <see cref="Close"/> must be called in order to free unmanaged resources.
		/// </summary>
		/// <param name="fileName">Filepath of the PDF file to load.</param>
		public PdfDocument(string fileName, string password = null)
			: this(PDFium.FPDF_LoadDocument(fileName, password)) { }

		/// <summary>
		/// Loads a <see cref="PdfDocument"/> from memory.
		/// <see cref="Close"/> must be called in order to free unmanaged resources.
		/// </summary>
		/// <param name="data">Byte array containing the bytes of the PDF document to load.</param>
		public PdfDocument(byte[] data, string password = null)
			: this(PDFium.FPDF_LoadDocument(data, password)) { }

		/// <summary>
		/// Loads a <see cref="PdfDocument"/> from '<paramref name="length"/>' bytes read from a <paramref name="stream"/>.
		/// <see cref="Close"/> must be called in order to free unmanaged resources.
		/// </summary>
		public PdfDocument(Stream stream, int length, string password = null)
			: this(PDFium.FPDF_LoadDocument(stream, length, password)) { }

		/// <summary>
		/// Loads a <see cref="PdfDocument"/> from a <paramref name="stream"/>.
		/// <see cref="Close"/> must be called in order to free unmanaged resources.
		/// </summary>
		public PdfDocument(Stream stream, string password = null)
			: this(PDFium.FPDF_LoadDocument(stream, password)) { }

		/// <summary>
		/// Closes the <see cref="PdfDocument"/> and frees unmanaged resources.
		/// </summary>
		public void Close()
		{
			if (!_ptr.IsNull)
			{
				((IDisposable)Pages).Dispose();
				PDFium.FPDF_CloseDocument(_ptr);
				_ptr = FPDF_DOCUMENT.Null;
			}
		}

		void IDisposable.Dispose() => Close();

		/// <summary>
		/// Saves the <see cref="PdfDocument"/> to a <paramref name="stream"/>.
		/// </summary>
		/// <param name="version">
		/// The new PDF file version of the saved file.
		/// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
		/// </param>
		public bool Save(Stream stream, SaveFlags flags = SaveFlags.None, int version = 0)
		{
			return PDFium.FPDF_SaveAsCopy(Handle, stream, flags, version);
		}

		/// <summary>
		/// Saves the <see cref="PdfDocument"/> to the file system.
		/// </summary>
		/// <param name="version">
		/// The new PDF file version of the saved file.
		/// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
		/// </param>
		public bool Save(string filename, SaveFlags flags = SaveFlags.None, int version = 0)
		{
			using (var stream = new FileStream(filename, FileMode.Create))
				return Save(stream, flags, version);
		}
	}
}
