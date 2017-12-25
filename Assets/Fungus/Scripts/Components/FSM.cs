using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// A simple state machine for use with Fungus Blocks. Define a list of states that can have a blocked 
    /// when the state machine is ticked or updated and also when the state is entered or exited. All blocks
    /// can be multi frame and the state machine will wait for them to complete before moving on. 
    /// 
	/// Note: has a custom inspector to show reorderable list of states with blocks correctly
    /// </summary>
    public class FSM : MonoBehaviour
    {
        /// <summary>
        /// Data used by 1 state. All Block references are optional.
        /// </summary>
        [System.Serializable]
        public class State
        {
            public string Name;
            public Block Enter, Update, Exit;
            public bool HasEntered { get; set; }
        }
        
		[HideInInspector][SerializeField]protected List<State> states;
		[SerializeField]protected int currentState = -1;
		[SerializeField]protected new string name;
		[SerializeField]protected bool startOnStart = true;
		[SerializeField]protected int startingState = 0;
		[SerializeField]protected bool tickInUpdate = true;

		public List<State> States {get {return states;}}

        private bool isTransitioningState = false;
        public bool IsTransitioningState {get {return isTransitioningState;}}

        public int CurrentState {get {return currentState;}}
        public string CurrentStateName {get {return states[CurrentState].Name;}}

        private void Start()
        {
            if (startOnStart)
                ChangeState(startingState);
        }

        public int GetIndexFromStateName(string name)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].Name == name)
                    return i;
            }
            return -1;
        }

        public void ChangeState(string statename)
        {
            ChangeState(GetIndexFromStateName(statename));
        }

        /// <summary>
        /// Will change the currently active state to that of the given index. Will cause the current state to have it's Exit
        /// Block called if one has been set and once that is complete the Enter block will be called on the new state.
        /// 
        /// Calls made with invalid index or when the state machine is already transitioning between states will be ignored
        /// </summary>
        /// <param name="newIndex"></param>
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

            //chace previous
            Block prevExit = null;
            if(curState != null)
            {
                prevExit = curState.Exit;
            }

            //prep new cur
            currentState = newIndex;
            curState = states[currentState];
            curState.HasEntered = false;

            //kick off the exit
            if(prevExit != null)
            {
                isTransitioningState = true;
                prevExit.StartExecution(onComplete: TransitionComplete);
            }
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

            EnterCurState();
            
            var curState = states[currentState];
            //allow things with no enter to update immediately
            if (!isTransitioningState && curState.Update != null)
            {
                curState.Update.StartExecution();
            }
        }

        private void TransitionComplete()
        {
            isTransitioningState = false;

            EnterCurState();
        }

        private void EnterCurState()
        {
            var curState = states[currentState];
            if (!curState.HasEntered)
            {
                //set this first so we don't have to deal with infinite loop of self entering states
                curState.HasEntered = true;
                if (curState.Enter != null)
                {
                    isTransitioningState = true;
                    curState.Enter.StartExecution(onComplete: TransitionComplete);
                }
            }
        }

		private void OnValidate()
		{
			string suffix = " Copy";
			List<string> names = new List<string>();

			for (int i = 0; i < states.Count; i++)
			{
				var curstate = states[i];
				var curName = curstate.Name;

				while(names.IndexOf(curName) != -1)
				{
					curName += suffix;
				}

				names.Add(curName);
				curstate.Name = curName;
			}
		}
    }
}