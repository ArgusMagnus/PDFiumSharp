using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PDFiumSharp
{
    public interface ISize<T> : IEquatable<ISize<T>>
    {
        T Width { get; }
        T Height { get; }
    }

    [DebuggerDisplay("(w,h): ({Width}, {Height})")]
    public readonly struct SizeInt32 : ISize<int>
    {
        public int Width { get; }
        public int Height { get; }

        public SizeInt32(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public SizeInt32(ISize<int> size)
            : this(size.Width, size.Height) { }

        bool IEquatable<ISize<int>>.Equals(ISize<int> other)
            => (Width, Height) == (other.Width, other.Height);
        public static bool operator ==(in SizeInt32 a, in SizeInt32 b)
            => (a.Width, a.Height) == (b.Width, b.Height);
        public static bool operator !=(in SizeInt32 a, in SizeInt32 b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is SizeInt32 ? (this == (SizeInt32)obj) : false;
        public override int GetHashCode()
            => (Width, Height).GetHashCode();
    }

    [DebuggerDisplay("(w,h): ({Width}, {Height})")]
    public readonly struct SizeSingle : ISize<float>
    {
        public float Width { get; }
        public float Height { get; }

        public SizeSingle(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public SizeSingle(ISize<float> size)
            : this(size.Width, size.Height) { }

        bool IEquatable<ISize<float>>.Equals(ISize<float> other)
            => (Width, Height) == (other.Width, other.Height);
        public static bool operator ==(in SizeSingle a, in SizeSingle b)
            => (a.Width, a.Height) == (b.Width, b.Height);
        public static bool operator !=(in SizeSingle a, in SizeSingle b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is SizeSingle ? (this == (SizeSingle)obj) : false;
        public override int GetHashCode()
            => (Width, Height).GetHashCode();
    }

    [DebuggerDisplay("(w,h): ({Width}, {Height})")]
    public readonly struct SizeDouble : ISize<double>
    {
        public double Width { get; }
        public double Height { get; }

        public SizeDouble(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public SizeDouble(ISize<double> size)
            : this(size.Width, size.Height) { }

        bool IEquatable<ISize<double>>.Equals(ISize<double> other)
            => (Width, Height) == (other.Width, other.Height);
        public static bool operator ==(in SizeDouble a, in SizeDouble b)
            => (a.Width, a.Height) == (b.Width, b.Height);
        public static bool operator !=(in SizeDouble a, in SizeDouble b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is SizeDouble ? (this == (SizeDouble)obj) : false;
        public override int GetHashCode()
            => (Width, Height).GetHashCode();
    }

    public interface ICoordinates<T> : IEquatable<ICoordinates<T>>
    {
        T X { get; }
        T Y { get; }
    }

    [DebuggerDisplay("(x,y): ({X}, {Y})")]
    public readonly struct CoordinatesSingle : ICoordinates<float>
    {
        public float X { get; }
        public float Y { get; }

        public CoordinatesSingle(float x, float y)
        {
            X = x;
            Y = y;
        }

        public CoordinatesSingle(ICoordinates<float> coords)
            : this(coords.X, coords.Y) { }

        bool IEquatable<ICoordinates<float>>.Equals(ICoordinates<float> other)
            => (X, Y) == (other.X, other.Y);
        public static bool operator ==(in CoordinatesSingle a, in CoordinatesSingle b)
            => (a.X, a.Y) == (b.X, b.Y);
        public static bool operator !=(in CoordinatesSingle a, in CoordinatesSingle b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is CoordinatesSingle ? (this == (CoordinatesSingle)obj) : false;
        public override int GetHashCode()
            => (X, Y).GetHashCode();
    }

    [DebuggerDisplay("(x,y): ({X}, {Y})")]
    public readonly struct CoordinatesDouble : ICoordinates<double>
    {
        public double X { get; }
        public double Y { get; }

        public CoordinatesDouble(double x, double y)
        {
            X = x;
            Y = y;
        }

        public CoordinatesDouble(ICoordinates<double> coords)
            : this(coords.X, coords.Y) { }

        bool IEquatable<ICoordinates<double>>.Equals(ICoordinates<double> other)
            => (X, Y) == (other.X, other.Y);
        public static bool operator ==(in CoordinatesDouble a, in CoordinatesDouble b)
            => (a.X, a.Y) == (b.X, b.Y);
        public static bool operator !=(in CoordinatesDouble a, in CoordinatesDouble b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is CoordinatesDouble ? (this == (CoordinatesDouble)obj) : false;
        public override int GetHashCode()
            => (X, Y).GetHashCode();
    }

    [DebuggerDisplay("(x,y): ({X}, {Y})")]
    public readonly struct CoordinatesInt32 : ICoordinates<int>
    {
        public int X { get; }
        public int Y { get; }

        public CoordinatesInt32(int x, int y)
        {
            X = x;
            Y = y;
        }

        public CoordinatesInt32(ICoordinates<int> coords)
            : this(coords.X, coords.Y) { }

        bool IEquatable<ICoordinates<int>>.Equals(ICoordinates<int> other)
            => (X, Y) == (other.X, other.Y);
        public static bool operator ==(in CoordinatesInt32 a, in CoordinatesInt32 b)
            => (a.X, a.Y) == (b.X, b.Y);
        public static bool operator !=(in CoordinatesInt32 a, in CoordinatesInt32 b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is CoordinatesInt32 ? (this == (CoordinatesInt32)obj) : false;
        public override int GetHashCode()
            => (X, Y).GetHashCode();
    }

    public interface IRectangle<T> : IEquatable<IRectangle<T>>
    {
        public T Left { get; }
        public T Top { get; }
        public T Right { get; }
        public T Bottom { get; }
        public ISize<T> Size { get; }
        public ICoordinates<T> Center { get; }
    }

    [DebuggerDisplay("(l,t,r,b): ({Left}, {Top}, {Right}, {Bottom})")]
    public readonly struct RectangleInt32 : IRectangle<int>
    {
        public int Left { get; }
        public int Top { get; }
        public int Right { get; }
        public int Bottom { get; }
        public SizeInt32 Size => new(Math.Abs(Right - Left), Math.Abs(Bottom - Top));
        public CoordinatesInt32 Center => new((Right + Left) / 2, (Bottom + Top) / 2);
        ISize<int> IRectangle<int>.Size => Size;
        ICoordinates<int> IRectangle<int>.Center => Center;

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

        public RectangleInt32(IRectangle<int> rect)
            : this(rect.Left, rect.Top, rect.Right, rect.Bottom) { }

        bool IEquatable<IRectangle<int>>.Equals(IRectangle<int> other)
            => (Left, Top, Right, Bottom) == (other.Left, other.Top, other.Right, other.Bottom);
        public static bool operator ==(in RectangleInt32 a, in RectangleInt32 b)
            => (a.Left, a.Top, a.Right, a.Bottom) == (b.Left, b.Top, b.Right, b.Bottom);
        public static bool operator !=(in RectangleInt32 a, in RectangleInt32 b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is RectangleInt32 ? (this == (RectangleInt32)obj) : false;
        public override int GetHashCode()
            => (Left, Top, Right, Bottom).GetHashCode();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("(l,t,r,b): ({Left}, {Top}, {Right}, {Bottom})")]
    public readonly struct RectangleSingle : IRectangle<float>
    {
        // Must be first field
        readonly Native.FS_RECTF_.__Internal _native;

        public float Left => _native.left;
        public float Top => _native.top;
        public float Right => _native.right;
        public float Bottom => _native.bottom;
        public SizeSingle Size => new(Math.Abs(Right - Left), Math.Abs(Bottom - Top));
        public CoordinatesSingle Center => new((Right + Left) / 2, (Bottom + Top) / 2);
        ISize<float> IRectangle<float>.Size => Size;
        ICoordinates<float> IRectangle<float>.Center => Center;

        public RectangleSingle(float left, float top, float right, float bottom, bool treatRightBottomAsWidthHeight = false)
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

        public RectangleSingle(IRectangle<float> rect)
            : this(rect.Left, rect.Top, rect.Right, rect.Bottom) { }

        bool IEquatable<IRectangle<float>>.Equals(IRectangle<float> other)
            => (Left, Top, Right, Bottom) == (other.Left, other.Top, other.Right, other.Bottom);
        public static bool operator ==(in RectangleSingle a, in RectangleSingle b)
            => (a.Left, a.Top, a.Right, a.Bottom) == (b.Left, b.Top, b.Right, b.Bottom);
        public static bool operator !=(in RectangleSingle a, in RectangleSingle b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is RectangleSingle ? (this == (RectangleSingle)obj) : false;
        public override int GetHashCode()
            => (Left, Top, Right, Bottom).GetHashCode();
    }


    [DebuggerDisplay("(l,t,r,b): ({Left}, {Top}, {Right}, {Bottom})")]
    public readonly struct RectangleDouble : IRectangle<double>
    {
        public double Left { get; }
        public double Top { get; }
        public double Right { get; }
        public double Bottom { get; }
        public SizeDouble Size => new(Math.Abs(Right - Left), Math.Abs(Bottom - Top));
        public CoordinatesDouble Center => new((Right + Left) / 2, (Bottom + Top) / 2);
        ISize<double> IRectangle<double>.Size => Size;
        ICoordinates<double> IRectangle<double>.Center => Center;

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

        public RectangleDouble(IRectangle<double> rect)
            : this(rect.Left, rect.Top, rect.Right, rect.Bottom) { }

        bool IEquatable<IRectangle<double>>.Equals(IRectangle<double> other)
            => (Left, Top, Right, Bottom) == (other.Left, other.Top, other.Right, other.Bottom);
        public static bool operator ==(in RectangleDouble a, in RectangleDouble b)
            => (a.Left, a.Top, a.Right, a.Bottom) == (b.Left, b.Top, b.Right, b.Bottom);
        public static bool operator !=(in RectangleDouble a, in RectangleDouble b)
            => !(a == b);
        public override bool Equals(object obj)
            => obj is RectangleDouble ? (this == (RectangleDouble)obj) : false;
        public override int GetHashCode()
            => (Left, Top, Right, Bottom).GetHashCode();
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

            public static void Deconstruct<T>(this IRectangle<T> rect, out T left, out T top, out T right, out T bottom)
            {
                left = rect.Left;
                top = rect.Top;
                right = rect.Right;
                bottom = rect.Bottom;
            }

            public static void Deconstruct<T>(this IRectangle<T> rect, out T left, out T top, out T right, out T bottom, out T width, out T height)
            {
                left = rect.Left;
                top = rect.Top;
                right = rect.Right;
                bottom = rect.Bottom;
                var size = rect.Size;
                width = size.Width;
                height = size.Height;
            }
        }
    }
}
