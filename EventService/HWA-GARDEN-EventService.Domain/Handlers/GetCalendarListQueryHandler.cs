using HWA.GARDEN.Contracts;
using HWA.GARDEN.Contracts.Messages;
using HWA.GARDEN.Contracts.Results;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.Utilities.Validation;
using MassTransit;
using MediatR;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Domain.Handlers
{
    public sealed class GetCalendarListQueryHandler
        : IStreamRequestHandler<GetCalendarListQuery, Calendar>
    {
        private readonly IRequestClient<GetCalendarList> _requestClient;

        public GetCalendarListQueryHandler(IRequestClient<GetCalendarList> requestClient)
        {
            Requires.NotNull(requestClient, nameof(requestClient));

            _requestClient = requestClient;
        }

        public async IAsyncEnumerable<Calendar> Handle(GetCalendarListQuery request,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Response<CalendarList>? response = 
                await _requestClient.GetResponse<CalendarList>(new { Year = request.Year }, cancellationToken)
                .ConfigureAwait(false);

            await foreach(Calendar? item in response.Message.Calendars.ToAsyncEnumerable())
            {
                yield return item;
            }
        }
    }
}
