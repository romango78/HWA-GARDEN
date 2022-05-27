using HWA.GARDEN.Contracts;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Requests
{
    public sealed class CalendarListQuery : IRequest<Calendar>
    {
        public int Year { get; set; }
    }
}
