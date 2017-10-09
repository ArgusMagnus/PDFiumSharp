using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;
using PDFiumSharp;
using System.IO;

namespace Test.Xwt
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Application.Initialize();
			Window window = new Window();
			window.Closed += (sender, e) => Application.Exit();

			using (var doc = new PdfDocument("TestDoc.pdf", "password"))
			{
				var page = doc.Pages[0];
				using (var bitmap = new PDFiumBitmap((int)page.Width, (int)page.Height, false))
				{
					page.Render(bitmap, PageOrientations.Normal);
					bitmap.Save("Page0.bmp");
					window.Content = new ImageView(Image.FromStream(bitmap.AsBmpStream()));
				}
			}

			window.Show();
			Application.Run();
		}
	}
}
