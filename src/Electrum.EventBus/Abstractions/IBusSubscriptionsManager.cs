namespace Electrum.EventBus.Abstractions
{
    public interface IBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        void AddSubscription<TCommand, THandler>();
    }
}