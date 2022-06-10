using MassTransit;

namespace HWA.GARDEN.Tests.Utilities
{
    public class TestContextContainer : ITestContextContainer
    {
        private readonly IDictionary<Type, object> _container
            = new Dictionary<Type, object>(10);

        public void AddMockedConsumer<TMessage>(IConsumer<TMessage> consumerMock) where TMessage : class
        {
            if (consumerMock == null)
            {
                throw new ArgumentNullException(nameof(consumerMock));
            }
            if (_container.ContainsKey(typeof(TMessage)))
            {
                throw new InvalidOperationException($"The consumer for \"{typeof(TMessage).FullName}\" type has been already added.");
            }
            _container.Add(typeof(TMessage), consumerMock);
        }

        public IConsumer<TMessage>? GetMockedConsumer<TMessage>() where TMessage : class
        {
            if (_container.ContainsKey(typeof(TMessage)))
            {
                return (IConsumer<TMessage>)_container[typeof(TMessage)];
            }
            return null;
        }
    }
}
