using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    /// <summary>
    /// The block will execute when a 3d physics trigger matching some basic conditions is met. 
    /// </summary>
    [EventHandlerInfo("Physics",
                      "Trigger",
                      "The block will execute when a 3d physics trigger matching some basic conditions is met.")]
    [AddComponentMenu("")]
    public class Trigger : BasePhysicsEventHandler
    {

        public override string GetSummary()
        {
            //TODO
            return "None";
        }

        private void OnTriggerEnter(Collider col)
        {
            ProcessCollider(PhysicsMessageType.Enter, col.tag);
        }

        private void OnTriggerStay(Collider col)
        {
            ProcessCollider(PhysicsMessageType.Stay, col.tag);
        }

        private void OnTriggerExit(Collider col)
        {
            ProcessCollider(PhysicsMessageType.Exit, col.tag);
        }
    }
}
