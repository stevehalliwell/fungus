using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// Project one vector3 into another
    /// </summary>
    [CommandInfo("Vector3",
                 "Project",
                 "Project a Vector3 onto another")]
    [AddComponentMenu("")]
    public class Vector3Project : Command
    {
        [SerializeField]
        protected Vector3Data vec3In, vec3OnNormal, vec3Out;

        public override void OnEnter()
        {
            vec3Out.Value = Vector3.Project(vec3In, vec3OnNormal);

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