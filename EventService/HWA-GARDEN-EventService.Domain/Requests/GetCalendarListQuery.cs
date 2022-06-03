using HWA.GARDEN.Contracts;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Requests
{
    public sealed class GetCalendarListQuery : IStreamRequest<Calendar>
    {
        public int Year { get; set; }
    }
}
