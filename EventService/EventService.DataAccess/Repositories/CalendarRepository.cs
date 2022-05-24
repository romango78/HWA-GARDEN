using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Entities;
using System.Data;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public class CalendarRepository : BaseRepository, ICalendarRepository
    {
        public CalendarRepository(IDbTransaction transaction)
            :base(transaction)
        { }

        public async Task<CalendarEntity> GetCalendarAsync(int year, CancellationToken cancellationToken)
        {
            /*var result = (from c in await Connection.QueryAsync<CalendarEntity>().ConfigureAwait(false)
                          where c.Year == year
                          select c).FirstOrDefault();*/

            throw new NotImplementedException();
        }
    }
}
