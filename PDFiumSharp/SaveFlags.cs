using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
	public enum SaveFlags : int
	{
		None = 0,
		Incremental = 1,
		NotIncremental = 2,
		RemoveSecurity = 3
	}
}
