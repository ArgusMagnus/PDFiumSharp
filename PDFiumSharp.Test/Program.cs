using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFiumSharp;
using System.IO;

namespace PDFiumSharp.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			const string DocPath = "TestDoc.pdf";
			using (var doc = new PdfDocument(DocPath))
			{
				var page = doc.Pages[0];
				System.Drawing.Bitmap bitmap;
				page.Render(out bitmap);
				using (bitmap)
					bitmap.Save("Test.png");
				doc.Save("Test.pdf");
			}
			Console.ReadKey();
		}
	}
}
