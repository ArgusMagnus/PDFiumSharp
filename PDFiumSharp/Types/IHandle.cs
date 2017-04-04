using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp.Types
{
    public interface IHandle<T>
    {
		bool IsNull { get; }

		T SetToNull();
    }
}
