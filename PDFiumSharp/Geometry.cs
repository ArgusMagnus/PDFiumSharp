using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp
{
	public interface ISize<T> : IEquatable<ISize<T>>
    {
		T Width { get; }
		T Height { get; }
    }

	public readonly struct SizeDouble : ISize<double>
	{
		public double Width { get; }
		public double Height { get; }

		public SizeDouble(double width, double height)
		{
			Width = width;
			Height = height;
		}

		bool IEquatable<ISize<double>>.Equals(ISize<double> other) => (Width, Height) == (other.Width, other.Height);
    }

	public interface ICoordinates<T> : IEquatable<ICoordinates<T>>
	{
		T X { get; }
		T Y { get; }
	}

	public readonly struct CoordinatesDouble : ICoordinates<double>
	{
		public double X { get; }
		public double Y { get; }

		public CoordinatesDouble(double x, double y)
		{
			X = x;
			Y = y;
		}

		bool IEquatable<ICoordinates<double>>.Equals(ICoordinates<double> other) => (X, Y) == (other.X, other.Y);
    }

	public readonly struct CoordinatesInt32 : ICoordinates<int>
	{
		public int X { get; }
		public int Y { get; }

		public CoordinatesInt32(int x, int y)
		{
			X = x;
			Y = y;
		}

        bool IEquatable<ICoordinates<int>>.Equals(ICoordinates<int> other) => (X, Y) == (other.X, other.Y);
	}

	public interface IRectangel<T> : IEquatable<IRectangel<T>>
	{
		public T Left { get; }
		public T Top { get; }
		public T Right { get; }
		public T Bottom { get; }
		public T Width { get; }
		public T Height { get; }
	}

	public readonly struct RectangleInt32 : IRectangel<int>
	{
		public int Left { get; }
		public int Top { get; }
		public int Right { get; }
		public int Bottom { get; }
		public int Width => Right - Left;
		public int Height => Bottom - Top;

		public RectangleInt32(int left, int top, int right, int bottom, bool treatRightBottomAsWidthHeight = false)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
			if (treatRightBottomAsWidthHeight)
			{
				Right += left;
				Bottom += top;
			}
		}

		bool IEquatable<IRectangel<int>>.Equals(IRectangel<int> other) => (Left, Top, Right, Bottom) == (other.Left, other.Top, other.Right, other.Bottom);
    }

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct RectangleFloat : IRectangel<float>
	{
		// Must be first field
		readonly Native.FS_RECTF_.__Internal _native;

		public float Left => _native.left;
		public float Top => _native.top;
		public float Right => _native.right;
		public float Bottom => _native.bottom;
		public float Width => Right - Left;
		public float Height => Bottom - Top;

		public RectangleFloat(float left, float top, float right, float bottom, bool treatRightBottomAsWidthHeight = false)
		{
			_native.left = left;
			_native.top = top;
			_native.right = right;
			_native.bottom = bottom;
			if (treatRightBottomAsWidthHeight)
			{
				_native.right += left;
				_native.bottom += top;
			}
		}

		bool IEquatable<IRectangel<float>>.Equals(IRectangel<float> other) => (Left, Top, Right, Bottom) == (other.Left, other.Top, other.Right, other.Bottom);
	}


	public readonly struct RectangleDouble : IRectangel<double>
	{
		public double Left { get; }
		public double Top { get; }
		public double Right { get; }
		public double Bottom { get; }
		public double Width => Right - Left;
		public double Height => Bottom - Top;

		public RectangleDouble(double left, double top, double right, double bottom, bool treatRightBottomAsWidthHeight = false)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
			if (treatRightBottomAsWidthHeight)
			{
				Right += left;
				Bottom += top;
			}
		}

		bool IEquatable<IRectangel<double>>.Equals(IRectangel<double> other) => (Left, Top, Right, Bottom) == (other.Left, other.Top, other.Right, other.Bottom);
	}

	namespace Extensions
	{
		public static class GeometryExtensions
		{
			public static void Deconstruct<T>(this ISize<T> size, out T width, out T height)
			{
				width = size.Width;
				height = size.Height;
			}
			public static void Deconstruct<T>(this ICoordinates<T> coord, out T x, out T y)
			{
				x = coord.X;
				y = coord.Y;
			}

			public static void Deconstruct<T>(this IRectangel<T> rect, out T left, out T top, out T right, out T bottom)
			{
				left = rect.Left;
				top = rect.Top;
				right = rect.Right;
				bottom = rect.Bottom;
			}

			public static void Deconstruct<T>(this IRectangel<T> rect, out T left, out T top, out T right, out T bottom, out T width, out T height)
			{
				left = rect.Left;
				top = rect.Top;
				right = rect.Right;
				bottom = rect.Bottom;
				width = rect.Width;
				height = rect.Height;
			}
		}
	}
}
