using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Domain.Requests;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Handlers
{
    public sealed class GetOrCreateCalendarRequestHandler : IRequestHandler<GetOrCreateCalendarRequest, Calendar>
    {
        public Task<Calendar> Handle(GetOrCreateCalendarRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
