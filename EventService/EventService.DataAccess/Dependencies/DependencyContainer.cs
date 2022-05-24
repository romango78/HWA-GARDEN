using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace HWA.GARDEN.EventService.Data.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            builder.AddScoped<Func<IDbTransaction, ICalendarRepository>>(sp => (t) => 
            { 
                return new CalendarRepository(t);
            });
            builder.AddScoped<Func<IDbTransaction, IEventRepository>>(sp => (t) =>
            {
                return new EventRepository(t);
            });
            builder.AddScoped<Func<IDbTransaction, IEventGroupRepository>>(sp => (t) =>
            {
                return new EventGroupRepository(t);
            });

            builder.AddScoped<IConnectionFactory, ConnectionFactory>();

            builder.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.AddScoped<Func<IUnitOfWork>>(sp => () => { return sp.GetRequiredService<IUnitOfWork>(); });
        }
    }
}
