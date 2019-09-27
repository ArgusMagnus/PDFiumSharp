using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PDFiumSharp.Types
{
    public abstract class NativeWrapper<T> : IDisposable
		where T : struct, IHandle<T>
    {
		T _handle;
		readonly long _unmanagedMemorySize;

		/// <summary>
		/// Handle which can be used with the native <see cref="PDFium"/> functions.
		/// </summary>
		public T Handle
		{
			get
			{
				if (_handle.IsNull)
					throw new ObjectDisposedException(GetType().FullName);
				return _handle;
			}
		}

		/// <summary>
		/// Gets a value indicating whether <see cref="IDisposable.Dispose"/> was already
		/// called on this instance.
		/// </summary>
		public bool IsDisposed => _handle.IsNull;

		protected NativeWrapper(T handle, long unmanagedMemorySize = 0)
		{
			if (handle.IsNull)
				throw new PDFiumException();
			_handle = handle;
			_unmanagedMemorySize = unmanagedMemorySize;
			if (_unmanagedMemorySize > 0)
				GC.AddMemoryPressure(_unmanagedMemorySize);
		}

		/// <summary>
		/// Implementors should clean up here. This method is guaranteed to only be called once.
		/// </summary>
		protected virtual void Dispose(T handle) { }

		void IDisposable.Dispose()
		{
			var oldHandle = _handle.SetToNull();
			if (!oldHandle.IsNull)
			{
				Dispose(oldHandle);
				if (_unmanagedMemorySize > 0)
					GC.RemoveMemoryPressure(_unmanagedMemorySize);
			}
		}
	}
}
