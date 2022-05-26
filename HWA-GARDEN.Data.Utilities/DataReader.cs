
using HWA.GARDEN.Utilities.Validation;
using System.Data.Common;

namespace HWA.GARDEN.Data.Utilities
{
    public class DataReader<T> : IAsyncEnumerable<T>
    {
        private readonly DbDataReader _dataReader;

        public DataReader(DbDataReader dataReader)
        {
            Requires.NotNull(dataReader, nameof(dataReader));

            _dataReader = dataReader;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new DataReaderEnumerator<T>(_dataReader);
        }
    }
}
