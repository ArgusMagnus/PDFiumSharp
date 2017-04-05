#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
# endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
	/// <summary>
	/// PDF Action Types
	/// </summary>
	public enum ActionTypes : uint
	{
		/// <summary>
		/// Unsupported action type.
		/// </summary>
		Unsupported = 0,

		/// <summary>
		/// Go to a destination within current document.
		/// </summary>
		GoTo = 1,

		/// <summary>
		/// Go to a destination within another document.
		/// </summary>
		RemoteGoTo = 2,

		/// <summary>
		/// URI, including web pages and other Internet resources.
		/// </summary>
		Uri = 3,

		/// <summary>
		/// Launch an application or open a file.
		/// </summary>
		Launch = 4
	}
}
