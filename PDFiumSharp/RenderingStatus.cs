using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
	public enum RenderingStatus : int
	{
		Reader = 0,

		ToBeContinued = 1,

		Done = 2,

		Failed = 3
	}
}
