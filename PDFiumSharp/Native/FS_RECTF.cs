using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PDFiumSharp.Native
{
    public partial class FS_RECTF_
    {
        internal unsafe FS_RECTF_(ref RectangleSingle dest)
            : this(Unsafe.AsPointer(ref dest)) { }
    }
}
