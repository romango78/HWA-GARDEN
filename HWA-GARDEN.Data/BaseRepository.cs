using HWA.GARDEN.Utilities.Validation;
using System.Data.Common;

namespace HWA.GARDEN.Data
{
    public abstract class BaseRepository
    {
        public BaseRepository(DbTransaction transaction)
        {
            Requires.NotNull(transaction, nameof(transaction));
            Transaction = transaction;
        }

        protected DbTransaction Transaction { get; }

        protected DbConnection Connection => Transaction?.Connection;
    }
}
