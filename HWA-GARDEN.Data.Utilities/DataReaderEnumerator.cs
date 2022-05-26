using Dapper;
using HWA.GARDEN.Utilities.Validation;
using System.Data;
using System.Data.Common;

namespace HWA.GARDEN.Data.Utilities
{
    public class DataReaderEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly DbDataReader _dataReader;

        public DataReaderEnumerator(DbDataReader dataReader)
        {
            Requires.NotNull(dataReader, nameof(dataReader));

            _dataReader = dataReader;
            RowParser = dataReader.GetRowParser<T>();
        }

        public T Current => _dataReader.FieldCount != 0 ? RowParser(_dataReader) : default(T);

        private Func<IDataReader, T> RowParser { get; }

        public async ValueTask DisposeAsync()
        {
            await _dataReader.DisposeAsync();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await _dataReader.ReadAsync();
        }
    }
}
