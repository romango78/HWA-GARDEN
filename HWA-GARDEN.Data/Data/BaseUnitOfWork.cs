using HWA.GARDEN.Utilities.Validation;
using System.Data.Common;

namespace HWA.GARDEN.Common.Data
{
    public class BaseUnitOfWork : IDisposable
    {
        private bool _disposed = false;

        protected DbConnection Connection;
        protected DbTransaction Transaction;

        public BaseUnitOfWork(IConnectionFactory connectionFactory)
        {
            Requires.NotNull(connectionFactory, nameof(connectionFactory));

            Transaction = connectionFactory.GetTransaction();
            Connection = Transaction.Connection;
            Requires.NotNull(Transaction, "IConnectionFactory.GetTransaction");
        }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
        }

        ///<summary>
        /// Releases all resources used by an instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual <see cref="Dispose(bool)" /> method, passing in true, 
        /// and then suppresses 
        /// finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources before an instance of the <see cref="UnitOfWork" /> 
        /// class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// NOTE: Leave out the finalizer altogether if this class doesn't 
        /// own unmanaged resources itself, but leave the other methods
        /// exactly as they are.
        /// This method releases unmanaged resources by calling the virtual 
        /// <see cref="Dispose(bool)" /> method, passing in false.
        /// </remarks>
        ~BaseUnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the 
        /// <see cref="UnitOfWork" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;        
        /// false to release only unmanaged resources.</param>
#pragma warning disable CA1063 // Implement IDisposable Correctly
        private void Dispose(bool disposing)
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                        Transaction = null;
                    }
                    if (Connection != null)
                    {
                        Connection.Dispose();
                        Connection = null;
                    }
                }
                // free native resources if there are any.
            }
        }
    }
}
