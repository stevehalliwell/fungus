using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// 
	/// Note: has a custom inspector to show reorderable list of states with blocks correctly
    /// </summary>
    public class FSM : MonoBehaviour
    {
        [System.Serializable]
        public class State
        {
            public string Name;
            public Block Enter, Update, Exit;
            public bool HasEntered { get; set; }
        }
        
        public List<State> states;
        public int currentState = -1;
        public new string name;
        public bool startOnStart = true;
        public int startingState = 0;
		public bool IsTransitioningState {get {return isTransitioningState;}}
        public bool tickInUpdate = true;


        private bool isTransitioningState = false;

        private void Start()
        {
            if (startOnStart)
                ChangeState(startingState);
        }

        public void ChangeState(int newIndex)
        {
			if(IsTransitioningState)
				return;
			
            //is it actually different and valid
            if (newIndex == currentState || (newIndex < 0 || newIndex >= states.Count))
                return;

            //if cur state exit it
            State curState = null;
            if (currentState >= 0 && currentState < states.Count)
            {
                curState = states[currentState];
            }

            if(curState != null && curState.Exit != null)
            {
                isTransitioningState = true;
                curState.Exit.StartExecution(onComplete: TransitionComplete);
            }

            //prep new cur
            currentState = newIndex;
            curState = states[currentState];
            curState.HasEntered = false;
        }

        public void Update()
        {
            if(tickInUpdate)
            {
                Tick();
            }
        }

        public void Tick()
        {
            if (isTransitioningState || (currentState < 0 && currentState >= states.Count))
                return;

            var curState = states[currentState];

            if (!curState.HasEntered)
            {
                if (curState.Enter != null)
                {
                    isTransitioningState = true;
                    curState.Enter.StartExecution(onComplete: TransitionComplete);
                }
                curState.HasEntered = true;
            }

            //allow things with no enter to update immediately
            if (!isTransitioningState && curState.Update != null)
            {
                curState.Update.StartExecution();
            }
        }

        private void TransitionComplete()
        {
            isTransitioningState = false;
        }
    }
}