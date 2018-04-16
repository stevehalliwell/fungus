using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// A simple state machine for use with Fungus Blocks. Define a list of states that can have a block called
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

        //custom inspector handles drawing this manually, all others are done with default draw
        [HideInInspector] [SerializeField] protected List<State> states = new List<State>();
        protected int currentState = -1;
        [Tooltip("The flowchart that contains all the blocks the FSM will refer to. If the FSM is on the same GO as a flowchart it will default there.")]
        [SerializeField] public Flowchart blocksLiveOn;
        [Tooltip("Does this FSM Enter the top state in its Start?")]
        [SerializeField] protected bool startOnStart = true;
        [Tooltip("Does this FSM tick the current state's Update in its Update")]
        [SerializeField] protected bool tickInUpdate = true;

        public List<State> States { get { return states; } }
        
        private bool isTransitioningState = false;
        public bool IsTransitioningState { get { return isTransitioningState; } }
        private bool isWaitingOnUpdateToComplete = false;
        public bool IsWaitingOnUpdateToComplete { get { return isWaitingOnUpdateToComplete; } }

        public int CurrentStateIndex { get { return currentState; } }
        public string CurrentStateName
        {
            get
            {
                return (CurrentStateIndex < states.Count && CurrentStateIndex >= 0) ? states[CurrentStateIndex].Name : string.Empty;
            }
        }

        private void Start()
        {
            if (startOnStart)
                ChangeState(0);
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
            if (IsTransitioningState)
            {
                Debug.LogWarning("Attempted to change state while in the middle of a transition, this is not supported " +
                    "and the call is being ignored.");
                return;
            }

            //is it actually different and valid
            if (newIndex == currentState || (newIndex < 0 || newIndex >= states.Count))
                return;

            //if cur be able to exit it
            State curState = null;
            Block prevExit = null;
            if (currentState >= 0 && currentState < states.Count)
            {
                curState = states[currentState];
                if (curState != null)
                {
                    prevExit = curState.Exit;
                }
            }

            //if the cur state is still updating, stop it
            if(isWaitingOnUpdateToComplete && curState != null && curState.Update != null)
            {
                curState.Update.Stop();
                UpdateComplete();
            }

            //prep new cur
            currentState = newIndex;
            curState = states[currentState];
            curState.HasEntered = false;

            //kick off the exit
            if (prevExit != null)
            {
                isTransitioningState = true;
                prevExit.StartExecution(onComplete: TransitionComplete);
            }
            else
            {
                EnterCurState();
            }
        }

        public void Update()
        {
            if (tickInUpdate)
            {
                Tick();
            }
        }

        public void Tick()
        {
            if (isTransitioningState || (currentState < 0 && currentState >= states.Count))
                return;

            var curState = states[currentState];
            //allow things with no enter to update immediately
            if (!isTransitioningState && curState.Update != null && !isWaitingOnUpdateToComplete)
            {
                isWaitingOnUpdateToComplete = true;
                curState.Update.StartExecution(onComplete : UpdateComplete);
            }
        }

        private void TransitionComplete()
        {
            isTransitioningState = false;

            //we call this here so we know that enter is called even if tick isn't
            EnterCurState();
        }

        private void UpdateComplete()
        {
            isWaitingOnUpdateToComplete = false;
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

                while (names.IndexOf(curName) != -1)
                {
                    curName += suffix;
                }

                names.Add(curName);
                curstate.Name = curName;
            }
        }
    }
}