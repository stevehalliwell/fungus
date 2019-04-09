namespace Fungus
{
    /// <summary>
    /// Fire the given EmptyGameEvent.
    /// </summary>
    [CommandInfo("Event",
                 "Fire (Empty)",
                 "Fire the targeted EmptyGameEvent")]
    public class EmptyGameEventFireCommand : BaseGameEventFireCommand<Empty, EmptyGameEvent>
    {
        public override void OnEnterInner()
        {
            gameEvent.Fire();
        }
    }
}