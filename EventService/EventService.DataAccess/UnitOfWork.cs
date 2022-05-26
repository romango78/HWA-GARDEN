using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Repositories;
using HWA.GARDEN.Utilities.Validation;
using System.Data.Common;

namespace HWA.GARDEN.EventService.Data
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(IConnectionFactory connectionFactory,Func<DbTransaction, IEventRepository> eventRepoFactory, 
            Func<DbTransaction, IEventGroupRepository> eventGroupRepoFactory)
            :base(connectionFactory)
        {
            Requires.NotNull(connectionFactory, nameof(connectionFactory));
            Requires.NotNull(eventRepoFactory, nameof(eventRepoFactory));
            Requires.NotNull(eventGroupRepoFactory, nameof(eventGroupRepoFactory));

            EventRepository = eventRepoFactory(Transaction);
            EventGroupRepository = eventGroupRepoFactory(Transaction);
        }

        public IEventRepository EventRepository { get; }

        public IEventGroupRepository EventGroupRepository { get; }

    }
}
