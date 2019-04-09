using System.Collections;
using UnityEngine;

namespace Fungus
{
    namespace Examples
    {
        public class GameEventTester : MonoBehaviour
        {
            public EmptyGameEvent emptyGameEvent;
            public float delay = 1;
            public IntGameEvent intGameEvent;
            public int intToSend;

            public void Start()
            {
                StartCoroutine(RunEventAfterDelay());
            }

            private IEnumerator RunEventAfterDelay()
            {
                yield return new WaitForSeconds(delay);
                emptyGameEvent.Fire();
                intGameEvent.Fire(intToSend);
            }
        }
    }
}