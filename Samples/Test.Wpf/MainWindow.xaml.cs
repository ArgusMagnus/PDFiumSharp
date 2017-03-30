using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PDFiumSharp;

namespace Test.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			using (var doc = new PdfDocument("TestDoc.pdf", "password"))
			{
				var page = doc.Pages[0];
				WriteableBitmap bitmap = new WriteableBitmap((int)page.Width, (int)page.Height, 72, 72, PixelFormats.Bgra32, null);
				page.Render(bitmap);
				_ctrlImage.Source = bitmap;
			}
		}
	}
}
