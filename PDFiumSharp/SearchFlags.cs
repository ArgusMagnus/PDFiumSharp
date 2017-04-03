using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
    public enum SearchFlags : uint
	{
		/// <summary>
		/// If not set, it will not match case by default.
		/// </summary>
		MatchCase = 0x00000001,

		/// <summary>
		/// If not set, it will not match the whole word by default.
		/// </summary>
		MatchWholeWord = 0x00000002
    }
}
