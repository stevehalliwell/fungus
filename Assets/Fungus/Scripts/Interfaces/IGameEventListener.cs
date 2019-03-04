namespace Fungus
{
    public interface IGameEventListener<T>
    {
        void OnEventFired(T t);
    }
}