using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Native
{
    public partial class FPDF_FILEACCESS
	{
		public delegate bool FileReadBlockHandler(int position, IntPtr buffer, int size);

		public FPDF_FILEACCESS(int fileLength, FileReadBlockHandler readBlock)
			:this()
		{
			MFileLen = (uint)fileLength;
			unsafe { MGetBlock = (ignore, position, buffer, size) => readBlock(checked((int)position), new IntPtr(buffer), checked((int)size)) ? 1 : 0; }
		}

		public static FPDF_FILEACCESS FromStream(Stream stream, int count = 0)
		{
			if (count <= 0)
				count = (int)(stream.Length - stream.Position);
			var start = stream.Position;
			byte[]? data = null;

			var fileRead = new FPDF_FILEACCESS();
			fileRead.MFileLen = (uint)count;
			return new FPDF_FILEACCESS(count, (position, buffer, size) =>
			{
				stream.Position = start + position;
				if (data == null || data.Length < size)
					data = new byte[size];
				if (stream.Read(data, 0, size) != size)
					return false;
				Marshal.Copy(data, 0, buffer, size);
				return true;
			});
		}
	}
}
