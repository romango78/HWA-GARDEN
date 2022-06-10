using HWA.GARDEN.Contracts;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Requests
{
    public sealed class GetOrCreateCalendarRequest : IRequest<Calendar>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }
    }
}
