# PDFiumSharp Library

The PDFiumSharp library is a C#/.NET wrapper around the <a href="https://pdfium.googlesource.com/pdfium/">PDFium</a> library. It enables .NET developers to create, open, manipulate, render and save PDF documents.



## Getting Started

The easiest way to get going is to reference the NuGet packages. There are five NuGet packages available:
&nbsp;<ul>><li><a href="https://www.nuget.org/packages/PDFium.Windows/">PDFium.Windows</a> contains the native PDFium binaries for windows. Either install this package or provide the binaries yourself (put pdfium_x64.dll, pdfium_x86.dll in the application directory).</li><li><a href="https://www.nuget.org/packages/PDFiumSharp/">PDFiumSharp</a> contains the core package. With this you can create, open, manipulate, render and save PDF documents. <a href="../../wiki/T_PDFiumSharp_PdfPage">PdfPage</a>s can be rendered to <a href="../../wiki/T_PDFiumSharp_PDFiumBitmap">PDFiumBitmap</a>s (which are simply memory buffers). This package depends on the presence of the native PDFium binaries.</li><li><a href="https://www.nuget.org/packages/PDFiumSharp.GdiPlus/">PDFiumSharp.GdiPlus</a> extends the <a href="../../wiki/T_PDFiumSharp_PdfPage">PdfPage</a> class with extension methods to render to <a href="http://msdn2.microsoft.com/en-us/library/4e7y164x" target="_blank">Bitmap</a>s.</li><li><a href="https://www.nuget.org/packages/PDFiumSharp.Wpf/">PDFiumSharp.Wpf</a> extends the <a href="../../wiki/T_PDFiumSharp_PdfPage">PdfPage</a> class with extension methods to render to <a href="http://msdn2.microsoft.com/en-us/library/aa347331" target="_blank">WriteableBitmap</a>s.</li><li><a href="https://www.nuget.org/packages/PDFiumSharp.Xwt/">PDFiumSharp.Xwt</a> extends the <a href="../../wiki/T_PDFiumSharp_PdfPage">PdfPage</a> class with extension methods to render to BitmapImages.</li></ul>&nbsp;
To create or load a PDF document, use one of the <a href="../../wiki/T_PDFiumSharp_PdfDocument">PdfDocument</a>'s constructors.
