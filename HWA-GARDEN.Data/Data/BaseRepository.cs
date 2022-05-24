using HWA.GARDEN.Utilities.Validation;
using System.Data;

namespace HWA.GARDEN.Common.Data
{
    public abstract class BaseRepository
    {
        public BaseRepository(IDbTransaction transaction)
        {
            Requires.NotNull(transaction, nameof(transaction));
            Transaction = transaction;
        }

        protected IDbTransaction Transaction { get; }

        protected IDbConnection Connection => Transaction?.Connection;
    }
}
