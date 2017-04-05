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

namespace PDFiumSharp
{
	/// <summary>
	/// Flags specifying document permissions.
	/// </summary>
	/// <seealso cref="PdfDocument.SecurityHandlerRevision"/>
	/// <seealso cref="PDFium.FPDF_GetSecurityHandlerRevision(Types.FPDF_DOCUMENT)"/>
	/// <seealso href="http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf">PDF Reference: Table 22</seealso>
	[Flags]
    public enum DocumentPermissions : uint
    {
		/// <summary>
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 2: Print the document.
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 3 or greater: Print the document
		/// (possibly not at the highest quality level, depending on whether <see cref="PrintHighQuality"/> is also set). 
		/// </summary>
		Print = 1 << 2,

		/// <summary>
		/// Modify the contents of the document by operations other than those controlled by <see cref="ModfiyAnnotations"/>,
		/// <see cref="FillInForms"/> and <see cref="AssembleDocument"/>.
		/// </summary>
		Modify = 1 << 3,

		/// <summary>
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 2: Copy or otherwise extract text and graphics from the document,
		/// including extracting text and graphics (in support of accessibility to users with disabilities or for other purposes).
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 3 or greater: Copy or otherwise extract text and graphics from
		/// the document by operations other than that controlled by <see cref="ExtractTextAndGraphics2"/>. 
		/// </summary>
		ExtractTextAndGraphics = 1 << 4,

		/// <summary>
		/// Add or modify text annotations, fill in interactive form fields, and, if <see cref="Modify"/> is also set,
		/// create or modify interactive form fields (including signature fields). 
		/// </summary>
		ModfiyAnnotations = 1 << 5,

		/// <summary>
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 3 or greater: Fill in existing interactive form fields
		/// (including signature fields), even if <see cref="ModfiyAnnotations"/> is not set.
		/// </summary>
		FillInForms = 1 << 8,

		/// <summary>
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 3 or greater: Extract text and graphics
		/// (in support of accessibility to users with disabilities or for other purposes).
		/// </summary>
		ExtractTextAndGraphics2 = 1 << 9,

		/// <summary>
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 3 or greater: Assemble the document
		/// (insert, rotate, or delete pages and create bookmarks or thumbnail images), even if <see cref="Modify"/> is not set.
		/// </summary>
		AssembleDocument = 1 << 10,

		/// <summary>
		/// For <see cref="PdfDocument.SecurityHandlerRevision"/> of 3 or greater: Print the document to a representation
		/// from which a faithful digital copy of the PDF content could be generated. When <see cref="PrintHighQuality"/> is not set
		/// (and <see cref="Print"/> is set), printing is limited to a low-level representation of the appearance, possibly of degraded quality.
		/// </summary>
		PrintHighQuality = 1 << 11
    }
}
