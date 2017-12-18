using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// 
    /// </summary>
    public class FSM : MonoBehaviour
    {
        [System.Serializable]
        public class State
        {
            public string Name;
            public Block Enter, Update, Exit;
        }

        public List<State> states;
        public int currentState;
    }
}