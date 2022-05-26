using HWA.GARDEN.Contracts;
using MediatR;

namespace HWA.GARDEN.CalendarService.Domain.Requests
{
    public sealed class CalendarListQuery : IStreamRequest<Calendar>
    {
        public int Year { get; set; }
    }
}
