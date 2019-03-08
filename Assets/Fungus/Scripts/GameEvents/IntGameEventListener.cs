namespace Fungus
{
    /// <summary>
    ///  Listen to Fungus.IntGameEvent, routes param through a int UnityEvent configured in the inspector.
    /// </summary>
    public class IntGameEventListener : GameEventListener<int, IntGameEvent, IntUnityEvent>
    {
    }
}