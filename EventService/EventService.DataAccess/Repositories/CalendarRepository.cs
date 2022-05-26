using Dapper;
using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Entities;
using System.Data.Common;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public class CalendarRepository : BaseRepository, ICalendarRepository
    {
        public CalendarRepository(DbTransaction transaction)
            :base(transaction)
        { }

        public Task<CalendarEntity> GetAsync(int year, CancellationToken cancellationToken)
        {
            // TODO: Replace hardcoded SQL on LINQ expression
            var sql = $"SELECT * FROM [dbo].[Calendar] WHERE [Year] = {year}";

            var command = new CommandDefinition(sql, transaction: Transaction, cancellationToken: cancellationToken);
            return Connection.QueryFirstOrDefaultAsync<CalendarEntity>(command);
        }
    }
}
