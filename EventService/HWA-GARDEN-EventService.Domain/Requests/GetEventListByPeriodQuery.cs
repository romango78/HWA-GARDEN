using HWA.GARDEN.Contracts;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Requests
{
    public sealed class GetEventListByPeriodQuery : IStreamRequest<Event>
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
