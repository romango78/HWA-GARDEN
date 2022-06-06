using AutoMapper;
using HWA.GARDEN.CalendarService.Data;
using HWA.GARDEN.CalendarService.Data.Entities;
using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.CalendarService.Domain.Handlers
{
    public sealed class CalendarListQueryHandler : IStreamRequestHandler<CalendarListQuery, Calendar>
    {
        private readonly IMapper _mapper;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public CalendarListQueryHandler(IMapper mapper, Func<IUnitOfWork> unitOfWorkFactory)
        {
            Requires.NotNull(mapper, nameof(mapper));
            Requires.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            _mapper = mapper;
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
                    yield return _mapper.Map<Calendar>(item);
                }
                yield return _mapper.Map<Calendar>(request);
            }
        }
    }
}
