#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	public delegate bool FileReadBlockHandler(IntPtr ignore, int position, IntPtr buffer, int size);
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
