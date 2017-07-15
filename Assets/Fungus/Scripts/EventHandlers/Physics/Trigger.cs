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
    public class Trigger : EventHandler
    {
        [Tooltip("Only fire the event if one of the tags match. Empty means any will fire.")]
        [SerializeField]
        protected string[] tagFilter;

        [Tooltip("Which of the 3d physics messages to we trigger on.")]
        [SerializeField]
        [EnumFlag]
        protected PhysicsMessageType FireOn = PhysicsMessageType.Enter;

        public override string GetSummary()
        {
            //TODO
            return "None";
        }

        private void OnTriggerEnter(Collider col)
        {
            if ((FireOn & PhysicsMessageType.Enter) != 0)
            {
                ProcessCollider(col);
            }
        }

        private void OnTriggerStay(Collider col)
        {
            if ((FireOn & PhysicsMessageType.Stay) != 0)
            {
                ProcessCollider(col);
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if ((FireOn & PhysicsMessageType.Exit) != 0)
            {
                ProcessCollider(col);
            }
        }

        private void ProcessCollider(Collider col)
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
