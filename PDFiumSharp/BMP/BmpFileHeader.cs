using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PDFiumSharp.BMP
{
    class BmpFileHeader
    {
		public const uint Size = 14;

		readonly byte[] _buffer = new byte[Size];

		public BmpFileHeader(uint fileSize, uint pixelArrayOffset)
		{
			HeaderField1 = (byte)'B';
			HeaderField2 = (byte)'M';
			FileSize = fileSize;
			PixelArrayOffset = pixelArrayOffset;
		}

		public void Write(Stream stream)
		{
			stream.Write(_buffer, 0, _buffer.Length);
		}

		public byte HeaderField1
		{
			get => _buffer[0];
			private set => _buffer[0] = value;
		}

		public byte HeaderField2
		{
			get => _buffer[1];
			private set => _buffer[1] = value;
		}

		public uint FileSize
		{
			get => BitConverter.ToUInt32(_buffer, 2);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 2, bytes.Length);
			}
		}

		public uint PixelArrayOffset
		{
			get => BitConverter.ToUInt32(_buffer, 10);
			private set
			{
				var bytes = BitConverter.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, _buffer, 10, bytes.Length);
			}
		}
    }
}
