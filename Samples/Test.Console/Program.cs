using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFiumSharp;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var data = System.IO.File.ReadAllBytes("TestDoc.pdf");
			using (var doc = new PdfDocument(data, "password"))
			{
				var tmp = PDFium.FPDF_GetMetaText(doc.Handle, MetadataTags.CreationDate);
				Console.WriteLine(tmp);
			}
			Console.ReadKey();
		}
	}
}
