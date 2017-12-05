using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Fungus.EditorUtils;

namespace Fungus
{
    [CustomPropertyDrawer(typeof(Block))]
    public class BlockPropertyDrawer : PropertyDrawer
    {
        private Flowchart GetRelatedFlowchart(SerializedProperty property)
        {
            Command cmd = property.serializedObject.targetObject as Command;
            if(cmd != null)
            {
                return cmd.GetFlowchart();
            }
            return null;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (GetRelatedFlowchart(property) != null)
                return 0;
            else
                return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var flowchart = GetRelatedFlowchart(property);

            if (flowchart != null)
            {
                BlockEditor.BlockField(property,
                                      new GUIContent("Target Block", "Block to call"),
                                      new GUIContent("<None>"),
                                      flowchart);
            }
            else
            {
				EditorGUI.PropertyField(position, property);
            }
        }
    }
}