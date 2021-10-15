using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace PDFiumSharp
{
    public abstract class NativeWrapper<T> where T : class
    {
        // Hack until I figure out how to make CppSharp make certain classes inherit from custom interfaces
        protected static Func<T?, IntPtr> GetInstance { get; } = GetGetInstance();

        static Func<T?, IntPtr> GetGetInstance()
        {
            var par = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T?, IntPtr>>(Expression.Property(par, "__Instance"), par).Compile();
        }

        T? _nativeObject;

        [MemberNotNullWhen(false, nameof(_nativeObject))]
        public bool IsDisposed => _nativeObject == null || GetInstance(_nativeObject) == IntPtr.Zero;
        public virtual T NativeObject => !IsDisposed ? _nativeObject : throw new ObjectDisposedException(nameof(NativeObject));

        private protected NativeWrapper(T nativeObj)
        {
            if (nativeObj == null)
                throw new PDFiumException();
            if (GetInstance(nativeObj) == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(nativeObj));

            _nativeObject = nativeObj;
        }

        protected virtual bool SetNativeObjectToNull([MaybeNullWhen(false)] out T oldValue)
        {
            oldValue = Interlocked.Exchange(ref _nativeObject!, default);
            return oldValue != default;
        }
    }

    public abstract class DisposableNativeWrapper<T> : NativeWrapper<T>, IDisposable
         where T : class
    {
        private protected DisposableNativeWrapper(T nativeObj)
            : base(nativeObj) { }

        protected abstract void Dispose(bool disposing);

        ~DisposableNativeWrapper()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            if (!IsDisposed)
                Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            if (!IsDisposed)
            {
                Dispose(disposing: true);
                base.SetNativeObjectToNull(out var _);
            }
            GC.SuppressFinalize(this);
        }

        protected override bool SetNativeObjectToNull(out T oldValue)
            => throw new InvalidOperationException($"Not supported, call {nameof(Dispose)} instead.");
    }
}
