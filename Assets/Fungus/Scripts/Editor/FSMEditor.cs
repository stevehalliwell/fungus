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
        protected SerializedProperty nameProp, startOnStartProp, startingStateProp, tickInUpdateProp;
        protected ReorderableList statesList;

        private int selectedItem = -1;

        protected virtual void OnEnable()
        {
            statesProp = serializedObject.FindProperty("states");
            currentStateProp = serializedObject.FindProperty("currentState");
            nameProp = serializedObject.FindProperty("name");
            startOnStartProp = serializedObject.FindProperty("startOnStart");
            startingStateProp = serializedObject.FindProperty("startingState");
            tickInUpdateProp = serializedObject.FindProperty("tickInUpdate");

            statesList = new ReorderableList(serializedObject, statesProp);
            statesList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "States");
            };

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
                    rect.width /= 2;
                    rect.width -= 20;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Name"), GUIContent.none);
                    rect.x += rect.width;
                    rect.x += 5;
                    rect.width += 15;
                    var flow = (serializedObject.targetObject as MonoBehaviour).gameObject.GetComponent<Flowchart>();
                    DrawBlockElement(index, rect, element, "Update", flow);
                    
                    if (selectedItem == index)
                    {
                        rect = origRect;
                        rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing*2;
                        EditorGUI.indentLevel++;
                        rect = EditorGUI.IndentedRect(rect);
                        rect.width /= 2;
                        DrawBlockElement(index, rect, element, "Enter", flow);
                        rect.x += rect.width;
                        DrawBlockElement(index, rect, element, "Exit", flow);
                        EditorGUI.indentLevel--;
                    }
                }
            };

            statesList.elementHeightCallback = (int index) =>
            {
                return (selectedItem == index ? (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing*2) * 2 : EditorGUIUtility.singleLineHeight) + EditorGUIUtility.standardVerticalSpacing;
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(nameProp);
            EditorGUILayout.PropertyField(currentStateProp);
            EditorGUILayout.PropertyField(startOnStartProp);
            EditorGUILayout.PropertyField(startingStateProp);
            EditorGUILayout.PropertyField(tickInUpdateProp);
            statesList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawBlockElement(int index, Rect rect, SerializedProperty element, string propName, Flowchart chart)
        {
            const float labelWidth = 50;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight), propName);
            rect.x += labelWidth;
            rect.width -= labelWidth;
            var prop = element.FindPropertyRelative(propName);
            prop.objectReferenceValue = BlockEditor.BlockField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent(BlockEditor.NullName), chart, prop.objectReferenceValue as Block);
        }
    }
}