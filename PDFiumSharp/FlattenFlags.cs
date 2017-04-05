#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
	public enum FlattenFlags : int
	{
		/// <summary>
		/// Flatten for normal display.
		/// </summary>
		NormalDisplay = 0,

		/// <summary>
		/// Flatten for print.
		/// </summary>
		Print = 1
	}

	public enum FlattenResults : int
	{
		/// <summary>
		/// Flatten operation failed.
		/// </summary>
		Fail = 0,

		/// <summary>
		/// Flatten operation succeed.
		/// </summary>
		Success = 1,

		/// <summary>
		/// Nothing to be flattened.
		/// </summary>
		NothingToDo = 2
	}
}
