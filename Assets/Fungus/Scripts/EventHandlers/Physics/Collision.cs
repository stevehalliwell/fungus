using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    [System.Flags]
    public enum PhysicsMessageType
    {
        Enter = 1 << 0,
        Stay = 1 << 1,
        Exit = 1 << 2,
    }

    /// <summary>
    /// The block will execute when a 3d physics collision matching some basic conditions is met 
    /// </summary>
    [EventHandlerInfo("Physics",
                      "Collision",
                      "The block will execute when a 3d physics collision matching some basic conditions is met.")]
    [AddComponentMenu("")]
    public class Collision : EventHandler
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

        private void OnCollisionEnter(UnityEngine.Collision collision)
        {
            if( (FireOn & PhysicsMessageType.Enter) != 0)
            {
                ProcessCollider(collision.collider);
            }
        }

        private void OnCollisionStay(UnityEngine.Collision collision)
        {
            if ((FireOn & PhysicsMessageType.Stay) != 0)
            {
                ProcessCollider(collision.collider);
            }
        }

        private void OnCollisionExit(UnityEngine.Collision collision)
        {
            if ((FireOn & PhysicsMessageType.Exit) != 0)
            {
                ProcessCollider(collision.collider);
            }
        }

        private void ProcessCollider(Collider col)
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
