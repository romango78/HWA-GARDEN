using HWA.GARDEN.CalendarService.Data;
using HWA.GARDEN.CalendarService.Data.Entities;
using HWA.GARDEN.CalendarService.Domain.Adaptors;
using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.CalendarService.Domain.Handlers
{
    public sealed class CalendarListQueryHandler : IStreamRequestHandler<CalendarListQuery, Calendar>
    {
        private const int DefaultCalendarId = 0;

        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public CalendarListQueryHandler(Func<IUnitOfWork> unitOfWorkFactory)
        {
            Requires.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async IAsyncEnumerable<Calendar> Handle(CalendarListQuery request, 
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory())
            {
                await foreach (CalendarEntity item in 
                    uow.CalendarRepository.GetListAsync(request.Year)
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false))
                {
                    yield return new CalendarAdaptor(item);
                }
                yield return new Calendar
                {
                    Id = DefaultCalendarId,
                    Name = Strings.DefaultCalendarName,
                    Description = Strings.DefaultCalendarDescription,
                    Year = request.Year
                };
            }
        }
    }
}
