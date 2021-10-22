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
	/// Interaction logic for PdfPageView.xaml
	/// </summary>
	public partial class PdfPageView : UserControl
	{
		static readonly DependencyProperty ModelProperty = DependencyProperty
			.Register(nameof(Model), typeof(PdfPage), typeof(PdfPageView), new PropertyMetadata(propertyChangedCallback: (sender, e) => ((PdfPageView)sender).OnModelChanged((PdfPage)e.OldValue, (PdfPage)e.NewValue)));
		public PdfPage Model { get => (PdfPage)GetValue(ModelProperty); set => SetValue(ModelProperty, value); }

		public event Action<int, PdfDestination.View> GoToRequested;

		public PdfPageView()
		{
			InitializeComponent();
		}

		void OnModelChanged(PdfPage oldPage, PdfPage newPage)
		{
			_image.Source = null;
			if (newPage == null)
				return;

			var factor = Math.Max(SystemParameters.PrimaryScreenWidth / newPage.Width, SystemParameters.PrimaryScreenHeight / newPage.Height);
			WriteableBitmap bitmap = new WriteableBitmap((int)(newPage.Width * factor), (int)(newPage.Height * factor), 96, 96, PixelFormats.Bgra32, null);
			newPage.Render(bitmap);
			_image.Source = bitmap;

			var brush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));
			_overlay.BeginInit();
			foreach (Rectangle rectangle in _overlay.Children)
				rectangle.MouseLeftButtonUp -= Rectangle_MouseLeftButtonUp;
			_overlay.Children.Clear();
			foreach (var link in newPage.Links)
			{
				var rect = link.AnnotationRectangle;
				if (float.IsNaN(rect.Left))
					continue;

                var size = rect.Size;
                var rectangle = new Rectangle() { Fill = brush, Width = size.Width * factor, Height = size.Height * factor, Cursor = Cursors.Hand, Tag = link };
				Canvas.SetLeft(rectangle, rect.Left * factor);
				Canvas.SetTop(rectangle, (newPage.Height - rect.Top) * factor);
				rectangle.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;
				_overlay.Children.Add(rectangle);
				//var dst = link.Destination;
				//if (dst.Handle.IsNull && link.Action.Type == ActionTypes.GoTo)
				//	dst = link.Action.Destination;
				//if (!dst.Handle.IsNull)
				//{
				//	var view = dst.GetView();
				//}
			}
			_overlay.EndInit();
		}

		private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var link = (PdfLink)((FrameworkElement)sender).Tag;
			var dst = link.Destination;
			if (dst.NativeObject == null && link.Action.Type == ActionTypes.GoTo)
				dst = link.Action.Destination;
			if (dst.NativeObject != null)
				GoToRequested?.Invoke(dst.PageIndex, dst.GetView());
		}
	}
}
