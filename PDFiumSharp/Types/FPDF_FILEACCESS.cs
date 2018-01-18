#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace PDFiumSharp.Types
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool FileReadBlockHandler(IntPtr ignore, int position, IntPtr buffer, int size);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool FileWriteBlockHandler(IntPtr ignore, IntPtr data, int size);

	[StructLayout(LayoutKind.Sequential)]
    public class FPDF_FILEREAD
    {
		readonly int _fileLength;

		[MarshalAs(UnmanagedType.FunctionPtr)]
		readonly FileReadBlockHandler _readBlock;

		readonly IntPtr _param;

		public FPDF_FILEREAD(int fileLength, FileReadBlockHandler readBlock)
		{
			_fileLength = fileLength;
			_readBlock = readBlock;
			_param = IntPtr.Zero;
		}

		public static FPDF_FILEREAD FromStream(Stream stream, int count = 0)
		{
			if (count <= 0)
				count = (int)(stream.Length - stream.Position);
			var start = stream.Position;
			byte[] data = null;
			FPDF_FILEREAD fileRead = new FPDF_FILEREAD(count, (ignore, position, buffer, size) =>
			{
				stream.Position = start + position;
				if (data == null || data.Length < size)
					data = new byte[size];
				if (stream.Read(data, 0, size) != size)
					return false;
				Marshal.Copy(data, 0, buffer, size);
				return true;
			});
			return fileRead;
		}
    }

	[StructLayout(LayoutKind.Sequential)]
	public class FPDF_FILEWRITE
	{
		readonly int _version;

		[MarshalAs(UnmanagedType.FunctionPtr)]
		readonly FileWriteBlockHandler _writeBlock;

		public FPDF_FILEWRITE(FileWriteBlockHandler writeBlock)
		{
			_version = 1;
			_writeBlock = writeBlock;
		}
	}
}
