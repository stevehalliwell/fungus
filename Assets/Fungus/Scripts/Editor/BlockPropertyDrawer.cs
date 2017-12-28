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
            else if (CommandEditor.currentlyDrawingCommand != null)
            {
                return CommandEditor.currentlyDrawingCommand.GetFlowchart();
            }
            return null;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //if (GetRelatedFlowchart(property) != null)
            //    return 0;
            //else
                return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var flowchart = GetRelatedFlowchart(property);

            if (flowchart != null)
            {
                //try to handle the label for arrays and lists
                if (!label.text.StartsWith("Element ") && !string.IsNullOrEmpty(label.text))
                {
                    const float labelWidth = 50;
                    EditorGUI.LabelField(new Rect(position.x, position.y, labelWidth, position.height), label);
                    position.x += labelWidth;
                    position.width -= labelWidth;
                }
                property.objectReferenceValue = BlockEditor.BlockField(position,
                                                                       new GUIContent("<None>"),
                                                                       flowchart,
                                                                       property.objectReferenceValue as Block);
            }
            else
            {
				EditorGUI.PropertyField(position, property);
            }
        }
    }
}