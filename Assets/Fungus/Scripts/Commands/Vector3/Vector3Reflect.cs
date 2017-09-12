using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// Reflect one vector3 into another
    /// </summary>
    [CommandInfo("Vector3",
                 "Reflect",
                 "Reflect a Vector3 off a another")]
    [AddComponentMenu("")]
    public class Vector3Reflect : Command
    {
        [SerializeField]
        protected Vector3Data vec3InDir, vec3InNormal, vec3Out;

        public override void OnEnter()
        {
            vec3Out.Value = Vector3.Reflect(vec3InDir, vec3InNormal);

            Continue();
        }

        public override string GetSummary()
        {
            if (vec3Out.vector3Ref == null)
            {
                return "Error: no output set";
            }

            return "Stored in " + vec3Out.vector3Ref.Key;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}