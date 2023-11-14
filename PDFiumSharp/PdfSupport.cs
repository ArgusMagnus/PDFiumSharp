using PDFiumSharp.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PDFiumSharp
{
    /*
     https://github.com/ArgusMagnus/PDFiumSharp/issues/25
     */
    public class PdfSupport
    {
        /// <summary>
        /// Split pdf
        /// </summary>
        /// <param name="dest">The destination document which add the pages.</param>
        /// <param name="pageNumber">from 1</param>
        /// <returns></returns>
        public static MemoryStream GetPDFPage(PdfDocument dest, int pageNumber)
        {
            var newDocument = PDFium.FPDF_CreateNewDocument();
            if (PDFium.FPDF_ImportPages(newDocument, dest.Handle, pageNumber.ToString(), 0))
            {
                var ms = new MemoryStream();
                {
                    PDFium.FPDF_SaveAsCopy(newDocument, ms, SaveFlags.Incremental);
                    PDFium.FPDF_CloseDocument(newDocument);
                }
                return ms;
            }
            return null;
        }
        /// <summary>
        /// Split pdf
        /// </summary>
        /// <param name="dest">The destination document which add the pages.</param>
        /// <param name="pagerange">
        ///eg. "1,1,1,1"  4 1page
        /// "1-3"      from 1 page to 3 
        /// "1,3"      1page、3page 
        /// </param>
        /// <returns></returns>
        public static MemoryStream GetPDFPage(PdfDocument dest, string pagerange)
        {
            var newDocument = PDFium.FPDF_CreateNewDocument();
            if (PDFium.FPDF_ImportPages(newDocument, dest.Handle, pagerange, 0))
            {
                var ms = new MemoryStream();
                {
                    PDFium.FPDF_SaveAsCopy(newDocument, ms, SaveFlags.Incremental);
                    PDFium.FPDF_CloseDocument(newDocument);
                }
                return ms;
            }

            return null;
        }
        /// <summary>
        /// Merge pdf
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        public static MemoryStream MergePDF(params PdfDocument[] documents)
        {
            if (documents.Length == 0)
            {
                throw new ArgumentException("arg: documents not be 0 count");
            }
            var newDocument = PDFium.FPDF_CreateNewDocument();
            var index = 0;
            var res = false;
            for (int i = 0; i < documents.Length; i++)
            {
                var dest_doc = documents[i];
                if (dest_doc is PdfDocument dest)
                {
                    var pageCount = PDFium.FPDF_GetPageCount(dest.Handle);
                    var page2 = $"{1}-{pageCount}";
                    res = PDFium.FPDF_ImportPages(newDocument, dest.Handle, page2, index);
                    index += pageCount;
                }
            }
            if (!res)
            {
                return null;
            }
            var ms = new MemoryStream();
            {
                PDFium.FPDF_SaveAsCopy(newDocument, ms, SaveFlags.Incremental);
                PDFium.FPDF_CloseDocument(newDocument);
            }
            return ms;
        }

        /// <summary>
        /// Import some pages to a PDF document.
        /// </summary>
        /// <param name="dest_doc">The destination document which add the pages.</param>
        /// <param name="src_doc">A document to be imported.</param>
        /// <param name="index">The page index wanted to insert from.</param>
        /// <returns></returns>
        public static MemoryStream ImportPage(PdfDocument dest, PdfDocument src, int index)
        {
            var pageCount = PDFium.FPDF_GetPageCount(dest.Handle);
            if (index < 0)
                index = -1;
            if (index >= pageCount)
                index = pageCount;
            var sheet = index + 1;
            var newDocument = PDFium.FPDF_CreateNewDocument();
            var preSheet = "";
            var afterSheet = "";
            bool res = false;
            var iindex = 0;
            if (sheet > 1)
            {
                preSheet = $"{1}-{sheet - 1}";
                res = PDFium.FPDF_ImportPages(newDocument, dest.Handle, preSheet, iindex);
                iindex += sheet - 1;
            }
            var currCount = PDFium.FPDF_GetPageCount(src.Handle);
            var currSheet = $"{1}-{currCount}";
            res = PDFium.FPDF_ImportPages(newDocument, src.Handle, currSheet, iindex);
            iindex += currCount;
            if (sheet < pageCount)
            {
                afterSheet = $"{sheet}-{pageCount}";
                res = PDFium.FPDF_ImportPages(newDocument, dest.Handle, afterSheet, iindex);
            }

            if (res)
            {
                var ms = new MemoryStream();
                {
                    PDFium.FPDF_SaveAsCopy(newDocument, ms, SaveFlags.Incremental);
                    PDFium.FPDF_CloseDocument(newDocument);
                }
                return ms;
            }
            return null;
        }

    }
}
