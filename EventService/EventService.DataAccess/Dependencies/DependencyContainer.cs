using HWA.GARDEN.Data;
using HWA.GARDEN.EventService.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace HWA.GARDEN.EventService.Data.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            builder.AddScoped<Func<DbTransaction, IEventRepository>>(sp => (t) =>
            {
                return new EventRepository(t);
            });
            builder.AddScoped<Func<DbTransaction, IEventGroupRepository>>(sp => (t) =>
            {
                return new EventGroupRepository(t);
            });

            builder.AddScoped<IConnectionFactory, ConnectionFactory>();

            builder.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.AddScoped<Func<IUnitOfWork>>(sp => () => { return sp.GetRequiredService<IUnitOfWork>(); });
        }
    }
}
