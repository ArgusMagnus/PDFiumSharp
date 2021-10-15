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
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace PDFiumSharp
{
    public sealed class PdfDocument : DisposableNativeWrapper<Native.FpdfDocumentT>
    {
        //int _pageCount = -1;
        //public int PageCount => _pageCount > -1 ? _pageCount : (_pageCount = PDFium.FPDF_GetPageCount(Handle));

        //public PdfPage GetPage(int index) => PdfPage.Load(this, index);

        /// <summary>
        /// Gets the pages in the current <see cref="PdfDocument"/>.
        /// </summary>
        public PdfPageCollection Pages { get; }

        public PdfDestinationCollection Destinations { get; }

        /// <summary>
        /// Gets the PDF file version. File version: 14 for 1.4, 15 for 1.5, ...
        /// </summary>
        public int FileVersion { get { int v = 0; lock (NativeObject) { Native.fpdfview.FPDF_GetFileVersion(NativeObject, ref v); } return v; } }

        /// <summary>
        /// Gets the revision of the security handler.
        /// </summary>
        /// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference: Table 21</seealso>
        public int SecurityHandlerRevision { get { lock (NativeObject) { return Native.fpdfview.FPDF_GetSecurityHandlerRevision(NativeObject); } } }

        public DocumentPermissions Permissions { get { lock (NativeObject) { return (DocumentPermissions)Native.fpdfview.FPDF_GetDocPermissions(NativeObject); } } }

        public bool PrintPrefersScaling { get { lock (NativeObject) { return Native.fpdfview.FPDF_VIEWERREF_GetPrintScaling(NativeObject); } } }

        public int PrintCopyCount { get { lock (NativeObject) { return Native.fpdfview.FPDF_VIEWERREF_GetNumCopies(NativeObject); } } }

        public Native.FPDF_DUPLEXTYPE_ DuplexType { get { lock (NativeObject) { return Native.fpdfview.FPDF_VIEWERREF_GetDuplex(NativeObject); } } }

        public IEnumerable<PdfBookmark> Bookmarks
        {
            get
            {
                Native.FpdfBookmarkT handle;
                lock (NativeObject) { handle = Native.fpdf_doc.FPDFBookmarkGetFirstChild(NativeObject, null); }
                while (handle != null)
                {
                    yield return new PdfBookmark(this, handle);
                    lock (NativeObject) { handle = Native.fpdf_doc.FPDFBookmarkGetNextSibling(NativeObject, handle); }
                }
            }
        }

        public PageModes PageMode { get { lock (NativeObject) { return (PageModes)Native.fpdf_ext.FPDFDocGetPageMode(NativeObject); } } }

        PdfDocument(Native.FpdfDocumentT doc)
            : base(doc)
        {
            Pages = new PdfPageCollection(this);
            Destinations = new PdfDestinationCollection(this);
        }

        /// <summary>
        /// Creates a new <see cref="PdfDocument"/>.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        public PdfDocument()
            : this(Native.fpdf_edit.FPDF_CreateNewDocument()) { }

        /// <summary>
        /// Loads a <see cref="PdfDocument"/> from the file system.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        /// <param name="fileName">Filepath of the PDF file to load.</param>
        public PdfDocument(string fileName, string? password = null)
            : this(Native.fpdfview.FPDF_LoadDocument(fileName, password)) { }

        /// <summary>
        /// Loads a <see cref="PdfDocument"/> from memory.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        /// <param name="data">Byte array containing the bytes of the PDF document to load.</param>
        /// <param name="index">The index of the first byte to be copied from <paramref name="data"/>.</param>
        /// <param name="count">The number of bytes to copy from <paramref name="data"/> or a negative value to copy all bytes.</param>
        public unsafe PdfDocument(Span<byte> data, string? password = null)
            : this(Native.fpdfview.FPDF_LoadMemDocument(new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(data))), data.Length, password)) { }

        /// <summary>
        /// Loads a <see cref="PdfDocument"/> from '<paramref name="count"/>' bytes read from a <paramref name="stream"/>.
        /// <see cref="Close"/> must be called in order to free unmanaged resources.
        /// </summary>
        /// <param name="count">
        /// The number of bytes to read from the <paramref name="stream"/>.
        /// If the value is equal to or smaller than 0, the stream is read to the end.
        /// </param>
        public PdfDocument(Stream stream, int count = 0, string? password = null)
            : this(Native.fpdfview.FPDF_LoadCustomDocument(Native.FPDF_FILEACCESS.FromStream(stream, count), password)) { }

        /// <summary>
        /// Saves the <see cref="PdfDocument"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="version">
        /// The new PDF file version of the saved file.
        /// 14 for 1.4, 15 for 1.5, etc. Values smaller than 10 are ignored.
        /// </param>
        public bool Save(Stream stream, SaveFlags flags = SaveFlags.None, int version = 0)
        {
            var fileWrite = Native.FPDF_FILEWRITE_.FromStream(stream);
            lock (NativeObject)
            {
                if (version >= 10)
                    return Native.fpdf_save.FPDF_SaveWithVersion(NativeObject, fileWrite, (uint)flags, version);
                else
                    return Native.fpdf_save.FPDF_SaveAsCopy(NativeObject, fileWrite, (uint)flags);
            }
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

        public bool FindBookmark(string title, [MaybeNullWhen(false)] out PdfBookmark bookmark)
        {
            bookmark = default;
            Native.FpdfBookmarkT handle;
            lock (NativeObject) { handle = Native.fpdf_doc.FPDFBookmarkFind(NativeObject, title); }
            if (handle == null)
                return false;
            bookmark = new(this, handle);
            return true;
        }

        public string GetMetaText(MetadataTags tag) { lock (NativeObject) { return Native.fpdf_doc.FPDF_GetMetaText(NativeObject, tag.ToString()); } }

        public void CopyViewerPreferencesFrom(PdfDocument srcDoc) { lock (NativeObject) { Native.fpdf_ppo.FPDF_CopyViewerPreferences(NativeObject, srcDoc.NativeObject); } }

        protected override void Dispose(bool disposing)
        {
            ((IDisposable)Pages).Dispose();
            lock (NativeObject) { Native.fpdfview.FPDF_CloseDocument(NativeObject); }
        }
    }
}
