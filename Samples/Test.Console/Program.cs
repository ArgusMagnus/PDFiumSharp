using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFiumSharp;
using System.IO;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var doc = new PdfDocument("TestDoc.pdf", "password"))
			{
				int i = 0;
				foreach (var page in doc.Pages)
                    using (page)
                    {
                        using (var bitmap = new PDFiumBitmap((int)page.Width, (int)page.Height, true))
                        using (var stream = new FileStream($"{i++}.bmp", FileMode.Create))
                        {
                            page.Render(bitmap);
                            bitmap.Save(stream);
                        }
                    }
                
			}
			Console.ReadKey();
		}
	}
}
