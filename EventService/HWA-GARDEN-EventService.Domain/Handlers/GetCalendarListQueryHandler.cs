using HWA.GARDEN.Contracts;
using HWA.GARDEN.Contracts.Messages;
using HWA.GARDEN.Contracts.Results;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.Utilities.Validation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Domain.Handlers
{
    public sealed class GetCalendarListQueryHandler
        : IStreamRequestHandler<GetCalendarListQuery, Calendar>
    {
        private readonly ILogger<GetCalendarListQueryHandler> _logger;
        private readonly IRequestClient<GetCalendarList> _requestClient;

        public GetCalendarListQueryHandler(IRequestClient<GetCalendarList> requestClient, ILogger<GetCalendarListQueryHandler> logger)
        {
            Requires.NotNull(requestClient, nameof(requestClient));
            Requires.NotNull(logger, nameof(logger));

            _requestClient = requestClient;
            _logger = logger;
        }

        public async IAsyncEnumerable<Calendar> Handle(GetCalendarListQuery request, 
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Response<CalendarList>? response = 
                await _requestClient.GetResponse<CalendarList>(new { Year = request.Year }, cancellationToken);
            _logger.LogInformation($"The \"Get-Calendar-List\" request was processed and data[count:{response.Message.Calendars.Count()}] was received...");

            await foreach(Calendar? item in response.Message.Calendars.ToAsyncEnumerable())
            {
                yield return item;
            }
        }
    }
}
