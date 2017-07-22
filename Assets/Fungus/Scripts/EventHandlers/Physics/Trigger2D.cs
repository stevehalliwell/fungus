using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    /// <summary>
    /// The block will execute when a 2d physics trigger matching some basic conditions is met. 
    /// </summary>
    [EventHandlerInfo("Physics2D",
                      "Trigger",
                      "The block will execute when a 2d physics trigger matching some basic conditions is met.")]
    [AddComponentMenu("")]
    public class Trigger2D : EventHandler
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((FireOn & PhysicsMessageType.Enter) != 0)
            {
                ProcessCollider(col);
            }
        }
        
        private void OnTriggerStay2D(Collider2D col)
        {
            if ((FireOn & PhysicsMessageType.Stay) != 0)
            {
                ProcessCollider(col);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if ((FireOn & PhysicsMessageType.Exit) != 0)
            {
                ProcessCollider(col);
            }
        }

        private void ProcessCollider(Collider2D col)
        {
            if (tagFilter.Length == 0)
            {
                ExecuteBlock();
            }
            else
            {
                if (System.Array.IndexOf(tagFilter, col.tag) != -1)
                {
                    ExecuteBlock();
                }
            }
        }
    }
}
