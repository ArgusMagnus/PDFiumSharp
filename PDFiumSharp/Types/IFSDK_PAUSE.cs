using System;
using System.Runtime.InteropServices;

namespace PDFiumSharp.Types
{
	[StructLayout(LayoutKind.Sequential)]
    public class IFSDK_PAUSE
    {
		readonly int _version;

		[MarshalAs(UnmanagedType.FunctionPtr)]
		readonly Func<IntPtr, bool> _needToPauseCore;

		readonly IntPtr _userData;

		readonly Func<bool> _needToPause;

		public IFSDK_PAUSE(Func<bool> needToPause)
		{
			_needToPause = needToPause ?? throw new ArgumentNullException(nameof(needToPause));
			_needToPauseCore = (ignore) => needToPause();
			_version = 1;
			_userData = IntPtr.Zero;
		}
    }
}
