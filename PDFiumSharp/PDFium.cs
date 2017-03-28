#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Runtime.InteropServices;
using PDFiumSharp.Types;
using System.IO;
using System.Linq;
using System.Globalization;

namespace PDFiumSharp
{
    public static partial class PDFium
    {
		public static bool IsAvailable { get; } = Initialize();

		static bool Initialize()
		{
			try { PDFium.FPDF_InitLibrary(); }
			catch { return false; }
			return true;
		}

		public static FPDF_DOCUMENT FPDF_LoadDocument(byte[] data, string password = null)
		{
			return FPDF_LoadMemDocument(ref data[0], data.Length, password);
		}

		public static FPDF_DOCUMENT FPDF_LoadDocument(Stream stream, string password = null) => FPDF_LoadDocument(stream, (int)(stream.Length - stream.Position), password);

		public static FPDF_DOCUMENT FPDF_LoadDocument(Stream stream, int length, string password = null)
		{
			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length));
			var start = stream.Position;
			byte[] data = null;
			FPDF_FILEREAD fileRead = new FPDF_FILEREAD(length, (ignore, position, buffer, size) =>
			{
				stream.Position = start + position;
				if (data == null || data.Length < size)
					data = new byte[size];
				if (stream.Read(data, 0, size) != size)
					return false;
				Marshal.Copy(data, 0, buffer, size);
				return true;
			});
			return FPDF_LoadCustomDocument(fileRead, password);
		}

		public static bool FPDF_ImportPages(FPDF_DOCUMENT dest_doc, FPDF_DOCUMENT src_doc, int index, params int[] pages)
		{
			string pageRange = null;
			if (pages != null && pages.Length > 0)
				pageRange = string.Join(",", pages.Select(p => (p + 1).ToString(CultureInfo.InvariantCulture)));
			return FPDF_ImportPages(dest_doc, src_doc, pageRange, index);
		}

		public static bool FPDF_SaveAsCopy(FPDF_DOCUMENT document, Stream stream, SaveFlags flags, int version = 0)
		{
			byte[] buffer = null;
			FPDF_FILEWRITE fileWrite = new FPDF_FILEWRITE((ignore, data, size) =>
			{
				if (buffer == null || buffer.Length < size)
					buffer = new byte[size];
				Marshal.Copy(data, buffer, 0, size);
				stream.Write(buffer, 0, size);
				return true;
			});

			if (version >= 10)
				return FPDF_SaveWithVersion(document, fileWrite, flags, version);
			else
				return FPDF_SaveAsCopy(document, fileWrite, flags);
		}
	}
}
