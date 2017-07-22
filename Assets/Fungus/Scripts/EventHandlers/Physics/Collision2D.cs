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
    public class Collision2D : EventHandler
    {
        [Tooltip("Only fire the event if one of the tags match. Empty means any will fire.")]
        [SerializeField]
        protected string[] tagFilter;

        [Tooltip("Which of the 2d physics messages to we trigger on.")]
        [SerializeField]
        [EnumFlag]
        protected PhysicsMessageType FireOn = PhysicsMessageType.Enter;

        public override string GetSummary()
        { 
            //TODO
            return "None";
        }

        private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
        {
            if( (FireOn & PhysicsMessageType.Enter) != 0)
            {
                ProcessCollider(collision.collider);
            }
        }

        private void OnCollisionStay2D(UnityEngine.Collision2D collision)
        {
            if ((FireOn & PhysicsMessageType.Stay) != 0)
            {
                ProcessCollider(collision.collider);
            }
        }

        private void OnCollisionExit2D(UnityEngine.Collision2D collision)
        {
            if ((FireOn & PhysicsMessageType.Exit) != 0)
            {
                ProcessCollider(collision.collider);
            }
        }

        private void ProcessCollider(UnityEngine.Collider2D col)
        {
            if(tagFilter.Length == 0)
            {
                ExecuteBlock();
            }
            else
            {
                if(System.Array.IndexOf(tagFilter, col.tag) != -1)
                {
                    ExecuteBlock();
                }
            }
        }
    }
}
