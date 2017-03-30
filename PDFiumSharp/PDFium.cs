#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Runtime.InteropServices;
using PDFiumSharp.Types;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace PDFiumSharp
{
	/// <summary>
	/// Static class containing the native (imported) PDFium functions.
	/// In case of missing documentation, refer to the <see href="https://pdfium.googlesource.com/pdfium/+/master/public">PDFium header files</see>. 
	/// </summary>
	public static partial class PDFium
    {
		/// <summary>
		/// Gets a value indicating whether the PDFium library is available.
		/// <c>false</c> is returned if the native libraries could not be
		/// loaded for some reason.
		/// </summary>
		public static bool IsAvailable { get; }

		static PDFium()
		{
			IsAvailable = Initialize();
		}

		static bool Initialize()
		{
			try { PDFium.FPDF_InitLibrary(); }
			catch { return false; }
			return true;
		}

		delegate uint GetStringHandler(ref byte buffer, uint length);

		static string GetUtf16String(GetStringHandler handler)
		{
			byte b = 0;
			uint length = handler(ref b, 0);
			var buffer = new byte[length];
			handler(ref buffer[0], length);
			return Encoding.Unicode.GetString(buffer, 0, (int)length - 2);
		}

		static string GetAsciiString(GetStringHandler handler)
		{
			byte b = 0;
			uint length = handler(ref b, 0);
			var buffer = new byte[length];
			handler(ref buffer[0], length);
			return Encoding.ASCII.GetString(buffer, 0, (int)length - 1);
		}

		static string GetUtf8String(GetStringHandler handler)
		{
			byte b = 0;
			uint length = handler(ref b, 0);
			var buffer = new byte[length];
			handler(ref buffer[0], length);
			return Encoding.UTF8.GetString(buffer, 0, (int)length - 1);
		}

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h

		/// <summary>
		/// Loads a PDF document from memory.
		/// </summary>
		public static FPDF_DOCUMENT FPDF_LoadDocument(byte[] data, string password = null)
		{
			return FPDF_LoadMemDocument(ref data[0], data.Length, password);
		}

		/// <summary>
		/// Loads a PDF document from a stream.
		/// </summary>
		public static FPDF_DOCUMENT FPDF_LoadDocument(Stream stream, string password = null) => FPDF_LoadDocument(stream, (int)(stream.Length - stream.Position), password);

		/// <summary>
		/// Loads a PDF document from '<paramref name="length"/>' bytes read from a stream.
		/// </summary>
		public static FPDF_DOCUMENT FPDF_LoadDocument(Stream stream, int length, string password = null)
		{
			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length));
			var start = stream.Position;
			byte[] data = null;
			FPDF_FILEREAD fileRead = new FPDF_FILEREAD(length, (ignore, position, buffer, size) =>
			{
				stream.Position = start + position;
				if (data == null || data.Length < size)
					data = new byte[size];
				if (stream.Read(data, 0, size) != size)
					return false;
				Marshal.Copy(data, 0, buffer, size);
				return true;
			});
			return FPDF_LoadCustomDocument(fileRead, password);
		}

		//public static string FPDF_VIEWERREF_GetName(FPDF_DOCUMENT document, string key)
		//{
		//	byte b = 0;
		//	uint length = FPDF_VIEWERREF_GetName(document, key, ref b, 0);
		//	if (length == 0)
		//		return null;
		//	var buffer = new byte[length];
		//	FPDF_VIEWERREF_GetName(document, key, ref buffer[0], length);
		//	return Encoding.UTF8.GetString(buffer);
		//}

		/// <summary>
		/// Get the named destination by index.
		/// </summary>
		/// <param name="document">Handle to a document.</param>
		/// <param name="index">The index of a named destination.</param>
		/// <returns>
		/// The destination handle and name for a given index, or (<see cref="FPDF_DEST.Null"/>, <c>null</c>)
		/// if there is no named destination corresponding to <paramref name="index"/>.
		/// </returns>
		public static (FPDF_DEST Destination, string Name) FPDF_GetNamedDest(FPDF_DOCUMENT document, int index)
		{
			FPDF_GetNamedDest(document, index, IntPtr.Zero, out int length);
			if (length < 1)
				return (FPDF_DEST.Null, null);
			var buffer = new byte[length];
			var dest = FPDF_GetNamedDest(document, index, ref buffer[0], ref length);
			if (length < 1)
				return (FPDF_DEST.Null, null);
			return (dest, Encoding.Unicode.GetString(buffer, 0, (int)length - 2));
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_doc.h

		/// <summary>
		/// Get the title of <paramref name="bookmark"/>.
		/// </summary>
		/// <param name="bookmark">Handle to the bookmark.</param>
		/// <returns>The title of the bookmark.</returns>
		public static string FPDFBookmark_GetTitle(FPDF_BOOKMARK bookmark)
		{
			return GetUtf16String((ref byte buffer, uint length) => FPDFBookmark_GetTitle(bookmark, ref buffer, length));
		}

		/// <summary>
		/// Gets the file path of a <see cref="FPDF_ACTION"/> of type <see cref="ActionTypes.RemoteGoTo"/> or <see cref="ActionTypes.Launch"/>.
		/// </summary>
		/// <param name="action">Handle to the action. Must be of type <see cref="ActionTypes.RemoteGoTo"/> or <see cref="ActionTypes.Launch"/>.</param>
		/// <returns>The file path of <paramref name="action"/>.</returns>
		public static string FPDFAction_GetFilePath(FPDF_ACTION action)
		{
			return GetUtf16String((ref byte buffer, uint length) => FPDFAction_GetFilePath(action, ref buffer, length));
		}

		/// <summary>
		/// Gets URI path of a <see cref="FPDF_ACTION"/> of type <see cref="ActionTypes.Uri"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="action">Handle to the action. Must be of type <see cref="ActionTypes.Uri"/>.</param>
		/// <returns>The URI path of <paramref name="action"/>.</returns>
		public static string FPDFAction_GetURIPath(FPDF_DOCUMENT document, FPDF_ACTION action)
		{
			return GetAsciiString((ref byte buffer, uint length) => FPDFAction_GetURIPath(document, action, ref buffer, length));
		}

		/// <summary>
		/// Enumerates all the link annotations in <paramref name="page"/>.
		/// </summary>
		/// <param name="page">Handle to the page.</param>
		/// <returns>All the link annotations in <paramref name="page"/>.</returns>
		public static IEnumerable<FPDF_LINK> FPDFLink_Enumerate(FPDF_PAGE page)
		{
			int pos = 0;
			while (FPDFLink_Enumerate(page, ref pos, out var link))
				yield return link;
		}

		/// <summary>
		/// Get meta-data <paramref name="tag"/> content from <paramref name="document"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="tag">
		/// The tag to retrieve. The tag can be one of:
		/// Title, Author, Subject, Keywords, Creator, Producer,
		/// CreationDate, or ModDate.
		/// </param>
		/// <returns>The meta-data.</returns>
		/// <remarks>
		/// For detailed explanations of these tags and their respective
		/// values, please refer to PDF Reference 1.6, section 10.2.1,
		/// 'Document Information Dictionary'.
		/// </remarks>
		/// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference</seealso>
		public static string FPDF_GetMetaText(FPDF_DOCUMENT document, string tag)
		{
			return GetUtf16String((ref byte buffer, uint length) => FPDF_GetMetaText(document, tag, ref buffer, length));
		}

		/// <summary>
		/// Get meta-data <paramref name="tag"/> content from <paramref name="document"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="tag">The tag to retrieve.</param>
		/// <returns>The meta-data.</returns>
		/// <remarks>
		/// For detailed explanations of these tags and their respective
		/// values, please refer to PDF Reference 1.6, section 10.2.1,
		/// 'Document Information Dictionary'.
		/// </remarks>
		/// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference</seealso>
		public static string FPDF_GetMetaText(FPDF_DOCUMENT document, MetadataTags tag) => FPDF_GetMetaText(document, tag.ToString());

		/// <summary>
		/// Get the page label for <paramref name="page_index"/> from <paramref name="document"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="page_index">The zero-based index of the page.</param>
		/// <returns>The page label.</returns>
		public static string FPDF_GetPageLabel(FPDF_DOCUMENT document, int page_index)
		{
			return GetUtf16String((ref byte buffer, uint length) => FPDF_GetPageLabel(document, page_index, ref buffer, length));
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_ppo.h

		/// <summary>
		/// Imports pages from <paramref name="src_doc"/> to <paramref name="dest_doc"/>
		/// </summary>
		/// <param name="index">Zero-based index of where the imported pages should be inserted in the destination document.</param>
		/// <param name="srcPageIndices">Zero-based indices of the pages to import in the source document</param>
		public static bool FPDF_ImportPages(FPDF_DOCUMENT dest_doc, FPDF_DOCUMENT src_doc, int index, params int[] srcPageIndices)
		{
			string pageRange = null;
			if (srcPageIndices != null && srcPageIndices.Length > 0)
				pageRange = string.Join(",", srcPageIndices.Select(p => (p + 1).ToString(CultureInfo.InvariantCulture)));
			return FPDF_ImportPages(dest_doc, src_doc, pageRange, index);
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_save.h

		/// <summary>
		/// Saves a PDF document to a stream.
		/// </summary>
		/// <param name="version">
		/// The new PDF file version of the saved file.
		/// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
		/// </param>
		public static bool FPDF_SaveAsCopy(FPDF_DOCUMENT document, Stream stream, SaveFlags flags, int version = 0)
		{
			byte[] buffer = null;
			FPDF_FILEWRITE fileWrite = new FPDF_FILEWRITE((ignore, data, size) =>
			{
				if (buffer == null || buffer.Length < size)
					buffer = new byte[size];
				Marshal.Copy(data, buffer, 0, size);
				stream.Write(buffer, 0, size);
				return true;
			});

			if (version >= 10)
				return FPDF_SaveWithVersion(document, fileWrite, flags, version);
			else
				return FPDF_SaveAsCopy(document, fileWrite, flags);
		}

		#endregion

	}
}
