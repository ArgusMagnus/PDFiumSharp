using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFiumSharp;
using System.IO;

using System.Drawing;
using System.Drawing.Printing;

namespace TestPrint
{
	class Program
	{

		static void Main(string[] args)
		{
			using (var doc = new PdfDocument("TestDoc.pdf", "password"))
			{
				PrintDocument printDoc = new PrintDocument();
				//printDoc.PrinterSettings.PrinterName = "Some Printer Name"; // Use a named printer rather than default
				printDoc.PrintController = new StandardPrintController(); // Use StandardPrintController to prevent "page I of N" box being shown in WIndows				
				int i = 0;
				printDoc.PrintPage += new PrintPageEventHandler((sender, ev) => {
					if (i < doc.Pages.Count) {
						PdfPage page = doc.Pages[i];
						ev.Graphics.PageUnit = GraphicsUnit.Pixel;
						Rectangle r = Rectangle.Round(ev.Graphics.VisibleClipBounds);
						page.Render(ev.Graphics.GetHdc(), (r.Left, r.Top, r.Width, r.Height), PageOrientations.Normal, RenderingFlags.Printing);
					}
					i++;
					if (i >= doc.Pages.Count)
						ev.HasMorePages = false;
					else
						ev.HasMorePages = true;
				});
				printDoc.Print();
			}
			Console.ReadKey();
		}
	}
}
