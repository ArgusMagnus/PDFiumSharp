using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading;

namespace PDFiumSharp
{
    public abstract class NativeWrapper<T> where T : class
    {
        // Hack until I figure out how to make CppSharp make certain classes inherit from custom interfaces
        protected static Func<T, IntPtr> GetInstance { get; } = GetGetInstance();

        static Func<T, IntPtr> GetGetInstance()
        {
            var par = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, IntPtr>>(Expression.Property(par, "__Instance"), par).Compile();
        }

        T _nativeObject;

        public virtual T NativeObject => _nativeObject;

        protected NativeWrapper(T nativeObj)
        {
            if (nativeObj == null)
                throw new PDFiumException();
            if (GetInstance(nativeObj) == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(nativeObj));

            _nativeObject = nativeObj;
        }

        protected bool SetNativeObjectToNull(out T oldValue)
        {
            oldValue = Interlocked.Exchange(ref _nativeObject, default);
            return oldValue != default;
        }
    }

    public abstract class NativeDisposableWrapper<T> : NativeWrapper<T>, IDisposable
        where T : class, IDisposable
    {
        public override T NativeObject => GetInstance(base.NativeObject) != IntPtr.Zero ? base.NativeObject : throw new ObjectDisposedException(nameof(NativeObject));
        protected NativeDisposableWrapper(T nativeObj)
            : base(nativeObj) { }

        void IDisposable.Dispose() => DisposeCore();

        protected virtual void DisposeCore() => NativeObject.Dispose();

    }
}
