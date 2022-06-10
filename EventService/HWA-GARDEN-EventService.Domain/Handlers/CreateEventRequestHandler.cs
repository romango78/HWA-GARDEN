using AutoMapper;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.Utilities.Validation;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Handlers
{
    public sealed class CreateEventRequestHandler : IRequestHandler<CreateEventRequest, Event>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateEventRequestHandler(IMediator mediator, IMapper mapper)
        {
            Requires.NotNull(mediator, nameof(mediator));
            Requires.NotNull(mapper, nameof(mapper));

            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Event> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            Calendar calendar =
                await _mediator.Send(_mapper.Map<GetOrCreateCalendarRequest>(request), cancellationToken)
                .ConfigureAwait(false);



            throw new NotImplementedException();
        }
    }
}
