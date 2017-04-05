using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFiumSharp;

namespace Test.Nuget
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var doc = new PdfDocument("TestDoc.pdf", "password"))
			{
				var page = doc.Pages[0];
				using (var bitmap = new PDFiumBitmap((int)page.Width, (int)page.Height, true))
				{
					page.Render(bitmap);
					bitmap.Save("Page1.bmp");
				}
			}
		}
	}
}
