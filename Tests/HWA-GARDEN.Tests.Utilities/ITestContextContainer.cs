using MassTransit;

namespace HWA.GARDEN.Tests.Utilities
{
    public interface ITestContextContainer
    {
        void AddMockedConsumer<TMessage>(IConsumer<TMessage> consumerMock)
            where TMessage: class;

        IConsumer<TMessage>? GetMockedConsumer<TMessage>()
            where TMessage : class;
    }
}
