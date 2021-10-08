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
using System.Text.RegularExpressions;

namespace Test.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(MainWindow), new PropertyMetadata(1.0));
		public double Zoom { get => (double)GetValue(ZoomProperty); set => SetValue(ZoomProperty, value); }

		PdfDocument _doc;
		public MainWindow()
		{
			//var test = PDFium.IsAvailable;
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
			Unloaded += MainWindow_Unloaded;
		}

		private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
		{
			_doc?.Dispose();
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var x = Environment.Is64BitProcess;
			_doc = new PdfDocument(@"TestDoc.pdf", "password");
			_list.ItemsSource = _doc?.Pages;
		}

		public static ScrollViewer GetScrollViewer(DependencyObject o)
		{
			// Return the DependencyObject if it is a ScrollViewer
			if (o is ScrollViewer)
			{ return (ScrollViewer)o; }

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
			{
				var child = VisualTreeHelper.GetChild(o, i);

				var result = GetScrollViewer(child);
				if (result == null)
				{
					continue;
				}
				else
				{
					return result;
				}
			}
			return null;
		}

		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
			if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
			{
				if (e.Delta > 0)
					Zoom += 0.05;
				else
					Zoom -= 0.05;
				e.Handled = true;
			}
			base.OnPreviewMouseWheel(e);
		}

		private void PdfPageView_GoToRequested(int pageIndex, PdfDestination.View view)
		{
			if (_doc == null)
				return;

			var sv = GetScrollViewer(_list);
			var offset = sv.ExtentHeight * pageIndex / _doc.Pages.Count();
			sv.ScrollToVerticalOffset(offset);
		}
	}
}
