using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    /// <summary>
    /// The block will execute when a 2d physics collision matching some basic conditions is met 
    /// </summary>
    [EventHandlerInfo("Physics2D",
                      "Collision",
                      "The block will execute when a 2d physics collision matching some basic conditions is met.")]
    [AddComponentMenu("")]
    public class Collision2D : BasePhysicsEventHandler
    {
       
        public override string GetSummary()
        { 
            //TODO
            return "None";
        }

        private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
        {
            ProcessCollider(PhysicsMessageType.Enter, collision.collider.tag);
        }

        private void OnCollisionStay2D(UnityEngine.Collision2D collision)
        {
            ProcessCollider(PhysicsMessageType.Stay, collision.collider.tag);
        }

        private void OnCollisionExit2D(UnityEngine.Collision2D collision)
        {
            ProcessCollider(PhysicsMessageType.Exit, collision.collider.tag);
        }
    }
}
