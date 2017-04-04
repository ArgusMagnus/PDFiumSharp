using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PDFiumSharp.BMP
{
	class BitmapInfoHeader
	{
		public const uint Size = 40;

		readonly byte[] _buffer = new byte[Size];

		public BitmapInfoHeader(int width, int height, ushort bpp, int pixelPerMeterH, int pixelPerMeterV)
		{
			HeaderSize = Size;
			BitmapWidth = width;
			BitmapHeight = height;
			ColorPlaneCount = 1;
			BitsPerPixel = bpp;
			CompressionMethod = 0; // BI_RGB
			PixelPerMeterHorizontal = pixelPerMeterH;
			PixelPerMeterVertical = pixelPerMeterV;
			ColorsInPalette = 0;
			ImportantColorsCount = 0;
		}

		public void Write(Stream stream)
		{
			stream.Write(_buffer, 0, _buffer.Length);
		}

		public uint HeaderSize
		{
			get => BitConverter.ToUInt32(_buffer, 0);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 0, bytes.Length);
			}
		}

		public int BitmapWidth
		{
			get => BitConverter.ToInt32(_buffer, 4);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 4, bytes.Length);
			}
		}

		public int BitmapHeight
		{
			get => BitConverter.ToInt32(_buffer, 8);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 8, bytes.Length);
			}
		}

		public ushort ColorPlaneCount
		{
			get => BitConverter.ToUInt16(_buffer, 12);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 12, bytes.Length);
			}
		}

		public ushort BitsPerPixel
		{
			get => BitConverter.ToUInt16(_buffer, 14);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 14, bytes.Length);
			}
		}

		public uint CompressionMethod
		{
			get => BitConverter.ToUInt32(_buffer, 16);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 16, bytes.Length);
			}
		}

		public uint RawImageDataSize
		{
			get => BitConverter.ToUInt32(_buffer, 20);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 20, bytes.Length);
			}
		}

		public int PixelPerMeterHorizontal
		{
			get => BitConverter.ToInt32(_buffer, 24);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 24, bytes.Length);
			}
		}

		public int PixelPerMeterVertical
		{
			get => BitConverter.ToInt32(_buffer, 28);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 28, bytes.Length);
			}
		}

		public uint ColorsInPalette
		{
			get => BitConverter.ToUInt32(_buffer, 32);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 32, bytes.Length);
			}
		}

		public uint ImportantColorsCount
		{
			get => BitConverter.ToUInt32(_buffer, 36);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 36, bytes.Length);
			}
		}
	}
}
