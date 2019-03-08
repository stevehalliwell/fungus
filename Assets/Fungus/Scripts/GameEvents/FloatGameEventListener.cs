namespace Fungus
{
    /// <summary>
    ///  Listen to Fungus.FloatGameEvent, routes param through a float UnityEvent configured in the inspector.
    /// </summary>
    public class FloatGameEventListener : GameEventListener<float, FloatGameEvent, FloatUnityEvent>
    {
    }
}