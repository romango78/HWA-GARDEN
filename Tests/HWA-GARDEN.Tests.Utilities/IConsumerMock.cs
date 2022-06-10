using MassTransit;

namespace HWA.GARDEN.Tests.Utilities
{
    public interface IConsumerMock<TMessage> : IConsumer<TMessage>
        where TMessage : class
    {
        IConsumer<TMessage> Object { get; }
    }
}
