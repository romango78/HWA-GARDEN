using MassTransit;

namespace HWA.GARDEN.Tests.Utilities
{
    public class ConsumerMock<TMessage> : IConsumerMock<TMessage>
        where TMessage : class
    {
        public ConsumerMock(ITestContextContainer testCtxContainer)
        {
            if(testCtxContainer == null)
            {
                throw new ArgumentNullException(nameof(testCtxContainer));
            }

            Object = testCtxContainer.GetMockedConsumer<TMessage>();
        }

        public IConsumer<TMessage>? Object { get; }

        public async Task Consume(ConsumeContext<TMessage> context)
        {
            await Task.Delay(1000);

            if (Object != null)
            {
                await Object.Consume(context);
            }
        }
    }
}
