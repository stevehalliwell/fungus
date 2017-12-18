using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

//http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
namespace Fungus.EditorUtils
{
    [CustomEditor(typeof(FSM))]
    public class FSMEditor : Editor
    {
        protected SerializedProperty statesProp;
        protected SerializedProperty currentStateProp;
        protected ReorderableList statesList;

        private int selectedItem = 0;

        protected virtual void OnEnable()
        {
            statesProp = serializedObject.FindProperty("states");
            currentStateProp = serializedObject.FindProperty("currentState");

            statesList = new ReorderableList(serializedObject, statesProp);
            statesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                selectedItem = isFocused ? index : selectedItem;

                var element = statesList.serializedProperty.GetArrayElementAtIndex(index);
                if (element != null)
                {
                    var origRect = rect;
                    rect.y += 2;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), index.ToString());
                    rect.x += 20;
                    rect.width -= 20;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight),"Name");
                    rect.x += 50;
                    rect.width -= 50;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Name"), GUIContent.none);
                    rect = origRect;
                    if (selectedItem == index)
                    {
                        EditorGUI.indentLevel++;
                        rect = EditorGUI.IndentedRect(rect);
                        rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing*2;
                        var flow = (serializedObject.targetObject as MonoBehaviour).gameObject.GetComponent<Flowchart>();
                        DrawBlockElement(index, ref rect, element, "Enter", flow);
                        DrawBlockElement(index, ref rect, element, "Update", flow);
                        DrawBlockElement(index, ref rect, element, "Exit", flow);
                        EditorGUI.indentLevel--;
                    }
                }
            };

            statesList.elementHeightCallback = (int index) =>
            {
                return (selectedItem == index ? EditorGUIUtility.singleLineHeight * 5 : EditorGUIUtility.singleLineHeight) + EditorGUIUtility.standardVerticalSpacing;
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(currentStateProp);
            statesList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawBlockElement(int index, ref Rect rect, SerializedProperty element, string propName, Flowchart chart)
        {
            const float labelWidth = 60;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight), propName);
            rect.x += labelWidth;
            rect.width -= labelWidth;
            var prop = element.FindPropertyRelative(propName);
            prop.objectReferenceValue = BlockEditor.BlockField(rect, new GUIContent(BlockEditor.NullName), chart, prop.objectReferenceValue as Block);
            rect.x -= labelWidth;
            rect.width += labelWidth;
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}