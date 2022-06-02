using Dapper;
using HWA.GARDEN.Data;
using HWA.GARDEN.EventService.Data.Entities;
using System.Data.Common;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public class EventGroupRepository : BaseRepository, IEventGroupRepository
    {
        public EventGroupRepository(DbTransaction transaction)
            : base(transaction)
        { }

        public Task<EventGroupEntity> GetAsync(int id, CancellationToken cancellationToken)
        {
            // TODO: Replace hardcoded SQL on LINQ expression
            var sql = $"SELECT * FROM [eso].[EventGroup] WHERE [Id] = {id}";

            var command = new CommandDefinition(sql, transaction: Transaction, cancellationToken: cancellationToken);
            return Connection.QueryFirstOrDefaultAsync<EventGroupEntity>(command);
        }

        public Task<IEnumerable<EventGroupEntity>> GetAsync(int startDt, int endDt, int calendarId,
            CancellationToken cancellationToken)
        {
            // TODO: Replace hardcoded SQL on LINQ expression
            var sql = $"SELECT EG.* FROM [eso].[Event] E INNER JOIN [eso].[EventGroup] EG ON E.[EventGroupID] = EG.[ID] WHERE E.[StartDt] <= {endDt} AND E.[EndDt] >= {startDt} AND (E.CalendarID IS NULL OR E.CalendarID = {calendarId})";

            var command = new CommandDefinition(sql, transaction: Transaction, cancellationToken: cancellationToken);
            return Connection.QueryAsync<EventGroupEntity>(command);                        
        }
    }
}
