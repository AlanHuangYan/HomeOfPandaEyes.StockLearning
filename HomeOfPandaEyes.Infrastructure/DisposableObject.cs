using System;

namespace HomeOfPandaEyes.Infrastructure
{
    /// <summary>
    /// This is the base class for the basic behavior of disposable object
    /// </summary>
    public class DisposableObject : IDisposable
    {
        private bool _isDisposed;

        #region IDisposable Members

        /// <summary>
        /// This is the Dispose method implementation from IDisposable
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    DisposeManagedResource();
                }
                DisposeUnmanagedResource();
                _isDisposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisposeManagedResource()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisposeUnmanagedResource()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }
    }
}