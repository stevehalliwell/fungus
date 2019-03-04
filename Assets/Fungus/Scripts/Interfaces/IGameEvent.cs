namespace Fungus
{
    public interface IGameEvent<T>
    {
        void Fire(T t);
        void Subscribe(IGameEventListener<T> sub);
        void Unsubscribe(IGameEventListener<T> sub);
        void Subscribe(System.Action<T> action);
        void Unsubscribe(System.Action<T> action);
    }
}