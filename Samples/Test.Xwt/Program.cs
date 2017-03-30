using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;
using PDFiumSharp;

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
				BitmapImage bitmap = new ImageBuilder(page.Width, page.Height).ToBitmap();
				page.Render(bitmap);
				window.Content = new ImageView(bitmap);
			}

			window.Show();
			Application.Run();
		}
	}
}
