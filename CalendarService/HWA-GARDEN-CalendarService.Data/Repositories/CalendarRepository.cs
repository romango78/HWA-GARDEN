using Dapper;
using HWA.GARDEN.Common.Data;
using HWA.GARDEN.CalendarService.Data.Entities;
using System.Data.Common;
using System.Runtime.CompilerServices;
using HWA.GARDEN.Data.Utilities;

namespace HWA.GARDEN.CalendarService.Data.Repositories
{
    public class CalendarRepository : BaseRepository, ICalendarRepository
    {
        public CalendarRepository(DbTransaction transaction)
            :base(transaction)
        { }

        public async IAsyncEnumerable<CalendarEntity> GetListAsync(int year, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // TODO: Replace hardcoded SQL on LINQ expression
            var sql = $"SELECT * FROM [cso].[Calendar] WHERE [Year] = {year}";

            var command = new CommandDefinition(sql, transaction: Transaction, cancellationToken: cancellationToken);
            using (var reader = await Connection.ExecuteReaderAsync(command).ConfigureAwait(false))
            {
                await foreach (var item in reader.GetReader<CalendarEntity>()
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }
    }
}
