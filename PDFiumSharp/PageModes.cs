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
	public enum PageModes : int
	{
		/// <summary>
		/// Unknown page mode.
		/// </summary>
		Unknown = -1,

		/// <summary>
		/// Document outline, and thumbnails hidden.
		/// </summary>
		UseNone = 0,

		/// <summary>
		/// Document outline visible.
		/// </summary>
		UseOutlines = 1,

		/// <summary>
		/// Thumbnail images visible.
		/// </summary>
		UseThumbs = 2,

		/// <summary>
		/// Full-screen mode, no menu bar, window controls, or other decorations visible.
		/// </summary>
		Fullscreen = 3,

		/// <summary>
		/// Optional content group panel visible.
		/// </summary>
		UseOC = 4,

		/// <summary>
		/// Attachments panel visible.
		/// </summary>
		UseAttachments = 5
	}
}
