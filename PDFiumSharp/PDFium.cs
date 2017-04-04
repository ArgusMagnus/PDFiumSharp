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

		delegate int GetStringHandler(ref byte buffer, int length);

		static string GetUtf16String(GetStringHandler handler, int lengthUnit, bool lengthIncludesTerminator)
		{
			byte b = 0;
			int length = handler(ref b, 0);
			var buffer = new byte[length * lengthUnit];
			handler(ref buffer[0], length);
			length *= lengthUnit;
			if (lengthIncludesTerminator)
				length -= 2;
			return Encoding.Unicode.GetString(buffer, 0, length);
		}

		static string GetAsciiString(GetStringHandler handler)
		{
			byte b = 0;
			int length = handler(ref b, 0);
			var buffer = new byte[length];
			handler(ref buffer[0], length);
			return Encoding.ASCII.GetString(buffer, 0, (int)length - 1);
		}

		static string GetUtf8String(GetStringHandler handler)
		{
			byte b = 0;
			int length = handler(ref b, 0);
			var buffer = new byte[length];
			handler(ref buffer[0], length);
			return Encoding.UTF8.GetString(buffer, 0, (int)length - 1);
		}

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h

		/// <summary>
		/// Loads a PDF document from memory.
		/// </summary>
		/// <param name="data">The data to load the document from.</param>
		/// <param name="index">The index of the first byte to be copied from <paramref name="data"/>.</param>
		/// <param name="count">The number of bytes to copy from <paramref name="data"/> or a negative value to copy all bytes.</param>
		public static FPDF_DOCUMENT FPDF_LoadDocument(byte[] data, int index = 0, int count = -1, string password = null)
		{
			if (count < 0)
				count = data.Length - index;
			return FPDF_LoadMemDocument(ref data[index], count, password);
		}

		/// <summary>
		/// Loads a PDF document from '<paramref name="count"/>' bytes read from a stream.
		/// </summary>
		/// <param name="count">
		/// The number of bytes to read from the <paramref name="stream"/>.
		/// If the value is equal to or smaller than 0, the stream is read to the end.
		/// </param>
		public static FPDF_DOCUMENT FPDF_LoadDocument(Stream stream, int count = 0, string password = null)
		{
			return FPDF_LoadCustomDocument(FPDF_FILEREAD.FromStream(stream, count), password);
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
		/// <seealso cref="PdfDestinationCollection[int]"/>
		/// <seealso cref="PdfDocument.Destinations"/>
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
			return GetUtf16String((ref byte buffer, int length) => (int)FPDFBookmark_GetTitle(bookmark, ref buffer, (uint)length), sizeof(byte), true);
		}

		/// <summary>
		/// Gets the file path of a <see cref="FPDF_ACTION"/> of type <see cref="ActionTypes.RemoteGoTo"/> or <see cref="ActionTypes.Launch"/>.
		/// </summary>
		/// <param name="action">Handle to the action. Must be of type <see cref="ActionTypes.RemoteGoTo"/> or <see cref="ActionTypes.Launch"/>.</param>
		/// <returns>The file path of <paramref name="action"/>.</returns>
		/// <seealso cref="PdfAction.FilePath"/>
		public static string FPDFAction_GetFilePath(FPDF_ACTION action)
		{
			return GetUtf16String((ref byte buffer, int length) => (int)FPDFAction_GetFilePath(action, ref buffer, (uint)length), sizeof(byte), true);
		}

		/// <summary>
		/// Gets URI path of a <see cref="FPDF_ACTION"/> of type <see cref="ActionTypes.Uri"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="action">Handle to the action. Must be of type <see cref="ActionTypes.Uri"/>.</param>
		/// <returns>The URI path of <paramref name="action"/>.</returns>
		/// <seealso cref="PdfAction.Uri"/>
		public static string FPDFAction_GetURIPath(FPDF_DOCUMENT document, FPDF_ACTION action)
		{
			return GetAsciiString((ref byte buffer, int length) => (int)FPDFAction_GetURIPath(document, action, ref buffer, (uint)length));
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
		/// <seealso cref="PdfDocument.GetMetaText(MetadataTags)"/>
		public static string FPDF_GetMetaText(FPDF_DOCUMENT document, string tag)
		{
			return GetUtf16String((ref byte buffer, int length) => (int)FPDF_GetMetaText(document, tag, ref buffer, (uint)length), sizeof(byte), true);
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
		/// <seealso cref="PdfDocument.GetMetaText(MetadataTags)"/>
		public static string FPDF_GetMetaText(FPDF_DOCUMENT document, MetadataTags tag) => FPDF_GetMetaText(document, tag.ToString());

		/// <summary>
		/// Get the page label for <paramref name="page_index"/> from <paramref name="document"/>.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="page_index">The zero-based index of the page.</param>
		/// <returns>The page label.</returns>
		/// <seealso cref="PdfPage.Label"/>
		//public static string FPDF_GetPageLabel(FPDF_DOCUMENT document, int page_index)
		//{
		//	return GetUtf16String((ref byte buffer, int length) => (int)FPDF_GetPageLabel(document, page_index, ref buffer, (uint)length), sizeof(byte), true);
		//}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_edit.h

		/// <summary>
		/// Insert <paramref name="page_obj"/> into <paramref name="page"/>.
		/// </summary>
		/// <param name="page">Handle to a page.</param>
		/// <param name="page_obj">Handle to a page object. The <paramref name="page_obj"/> will be automatically freed.</param>
		public static void FPDFPage_InsertObject(FPDF_PAGE page, ref FPDF_PAGEOBJECT page_obj)
		{
			FPDFPage_InsertObject(page, page_obj);
			page_obj = FPDF_PAGEOBJECT.Null;
		}

		/// <summary>
		/// Load an image from a JPEG image file and then set it into <paramref name="image_object"/>.
		/// </summary>
		/// <param name="loadedPages">All loaded pages, may be <c>null</c>.</param>
		/// <param name="image_object">Handle to an image object.</param>
		/// <param name="stream">Stream which provides access to an JPEG image.</param>
		/// <param name="count">The number of bytes to read from <paramref name="stream"/> or 0 to read to the end.</param>
		/// <param name="inline">
		/// If <c>true</c>, this function loads the JPEG image inline, so the image
		/// content is copied to the file. This allows <paramref name="stream"/>
		/// to be closed after this function returns.
		/// </param>
		/// <returns><c>true</c> on success.</returns>
		/// <remarks>
		/// The image object might already have an associated image, which is shared and
		/// cached by the loaded pages. In that case, we need to clear the cached image
		/// for all the loaded pages. Pass <paramref name="loadedPages"/> to this API
		/// to clear the image cache. If the image is not previously shared, <c>null</c> is a
		/// valid <paramref name="loadedPages"/> value.
		/// </remarks>
		public static bool FPDFImageObj_LoadJpegFile(FPDF_PAGE[] loadedPages, FPDF_PAGEOBJECT image_object, Stream stream, int count = 0, bool inline = true)
		{
			if (inline)
				return FPDFImageObj_LoadJpegFileInline(ref loadedPages[0], loadedPages.Length, image_object, FPDF_FILEREAD.FromStream(stream, count));
			else
				return FPDFImageObj_LoadJpegFile(ref loadedPages[0], loadedPages.Length, image_object, FPDF_FILEREAD.FromStream(stream, count));
		}

		/// <summary>
		/// Set <paramref name="bitmap"/> to <paramref name="image_object"/>.
		/// </summary>
		/// <param name="loadedPages">All loaded pages, may be <c>null</c>.</param>
		/// <param name="image_object">Handle to an image object.</param>
		/// <param name="bitmap">Handle of the bitmap.</param>
		/// <returns><c>true</c> on success.</returns>
		public static bool FPDFImageObj_SetBitmap(FPDF_PAGE[] loadedPages, FPDF_PAGEOBJECT image_object, FPDF_BITMAP bitmap)
		{
			return FPDFImageObj_SetBitmap(ref loadedPages[0], loadedPages.Length, image_object, bitmap);
		}

		/// <summary>
		/// Returns a font object loaded from a stream of data. The font is loaded
		/// into the document. The caller does not need to free the returned object.
		/// </summary>
		/// <param name="document">Handle to the document.</param>
		/// <param name="cid">A value specifying if the font is a CID font or not.</param>
		/// <param name="data">The data, which will be copied by the font object.</param>
		/// <param name="index">The index of the first byte to be copied from <paramref name="data"/>.</param>
		/// <param name="count">The number of bytes to copy from <paramref name="data"/> or a negative value to copy all bytes.</param>
		/// <returns>Returns NULL on failure.</returns>
		public static FPDF_FONT FPDFText_LoadFont(FPDF_DOCUMENT document, FontTypes font_type, bool cid, byte[] data, int index = -1, int count = 0)
		{
			if (count < 0)
				count = data.Length - index;
			return FPDFText_LoadFont(document, ref data[index], (uint)count, font_type, cid);
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
		/// <seealso cref="PDFium.FPDF_SaveAsCopy(FPDF_DOCUMENT, FPDF_FILEWRITE, SaveFlags)"/>
		/// <seealso cref="PDFium.FPDF_SaveWithVersion(FPDF_DOCUMENT, FPDF_FILEWRITE, SaveFlags, int)"/>
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

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_structtree.h

		/// <summary>
		/// Get the alternative text for a given element.
		/// </summary>
		/// <param name="struct_element">Handle to the struct element.</param>
		/// <returns>The alternative text for <paramref name="struct_element"/>.</returns>
		public static string FPDF_StructElement_GetAltText(FPDF_STRUCTELEMENT struct_element)
		{
			return GetUtf16String((ref byte buffer, int length) => (int)FPDF_StructElement_GetAltText(struct_element, ref buffer, (uint)length), sizeof(byte), true);
		}

		#endregion

		#region https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_text.h

		public static string FPDFText_GetText(FPDF_TEXTPAGE text_page, int start_index, int count)
		{
			var buffer = new byte[2 * (count + 1)];
			int length = FPDFText_GetText(text_page, start_index, count, ref buffer[0]);
			return Encoding.Unicode.GetString(buffer, 0, (length - 1) * 2);
		}

		public static string FPDFText_GetBoundedText(FPDF_TEXTPAGE text_page, double left, double top, double right, double bottom)
		{
			return GetUtf16String((ref byte buffer, int length) => FPDFText_GetBoundedText(text_page, left, top, right, bottom, ref buffer, length), sizeof(ushort), false);
		}

		public static string FPDFLink_GetURL(FPDF_PAGELINK link_page, int link_index)
		{
			return GetUtf16String((ref byte buffer, int length) => FPDFLink_GetURL(link_page, link_index, ref buffer, length), sizeof(ushort), true);
		}

		#endregion
	}
}
