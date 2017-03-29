# PDFiumSharp Library

The PDFiumSharp library is a C#/.NET wrapper around the <a href="https://pdfium.googlesource.com/pdfium/">PDFium</a> library. It enables .NET developers to create, open, manipulate, render and save PDF documents.



## Getting Started

The easiest way to get going is to reference the NuGet packages. There are four NuGet packages available:
&nbsp;<ul><li><a href="https://www.nuget.org/packages/PDFiumSharp/">PDFiumSharp</a> contains the core package. With this you can create, open, manipulate, render and save PDF documents. <a href="T_PDFiumSharp_PdfPage">PdfPage</a>s can be rendered to <a href="T_PDFiumSharp_PDFiumBitmap">PDFiumBitmap</a>s (which are simply memory buffers).</li><li><a href="https://www.nuget.org/packages/PDFiumSharp.GdiPlus/">PDFiumSharp.GdiPlus</a> extends the <a href="T_PDFiumSharp_PdfPage">PdfPage</a> class with extension methods to render to <a href="http://msdn2.microsoft.com/en-us/library/4e7y164x" target="_blank">Bitmap</a>s.</li><li><a href="https://www.nuget.org/packages/PDFiumSharp.Wpf/">PDFiumSharp.Wpf</a> extends the <a href="T_PDFiumSharp_PdfPage">PdfPage</a> class with extension methods to render to <a href="http://msdn2.microsoft.com/en-us/library/aa347331" target="_blank">WriteableBitmap</a>s.</li><li><a href="https://www.nuget.org/packages/PDFiumSharp.Xwt/">PDFiumSharp.Xwt</a> extends the <a href="T_PDFiumSharp_PdfPage">PdfPage</a> class with extension methods to render to BitmapImages.</li></ul>&nbsp;
To create or load a PDF document, use one of the <a href="T_PDFiumSharp_PdfDocument">PdfDocument</a>'s constructors.
