﻿using Dapper;
using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Entities;
using HWA.GARDEN.Data.Utilities;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(DbTransaction transaction)
            : base(transaction)
        { }

        public async IAsyncEnumerable<EventEntity> GetAsync(int startDt, int endDt, int calendarId,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // TODO: Replace hardcoded SQL on LINQ expression
            var sql = $"SELECT * FROM [dbo].[Event] WHERE [StartDt] <= {endDt} AND [EndDt] >= {startDt} AND (CalendarID IS NULL OR CalendarID = {calendarId})";

            var command = new CommandDefinition(sql, transaction: Transaction, cancellationToken: cancellationToken);
            using (var reader = await Connection.ExecuteReaderAsync(command).ConfigureAwait(false))
            {
                await foreach (var item in reader.GetReader<EventEntity>()
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }
    }
}
