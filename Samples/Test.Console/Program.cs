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
			using (var doc = new PdfDocument("TestDoc.pdf", "password"))
			{
				var tmp = PDFium.FPDF_GetMetaText(doc.Handle, MetadataTags.CreationDate);
				Console.WriteLine(tmp);
			}
			Console.ReadKey();
		}
	}
}
