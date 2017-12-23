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
        protected ReorderableList statesList;

        private int selectedItem = -1;

        protected virtual void OnEnable()
        {
            statesProp = serializedObject.FindProperty("states");

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
			DrawDefaultInspector();
            
			serializedObject.Update();
			statesList.DoLayoutList();

			if(GUILayout.Button(new GUIContent("Generate Blocks","Auto create blocks for all unassgined elements of states.")))
			{
				CreateAndAssignBlocks();
				EditorUtility.SetDirty(target);
			}

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

		private void CreateAndAssignBlocks()
		{
			FSM fsm = target as FSM;
			var states = fsm.States;
			var flow = fsm.GetComponent<Flowchart>();

			for (int i = 0; i < states.Count; i++)
			{
				var curState = states[i];
				if(curState != null)
				{
					if(curState.Enter == null)
						curState.Enter = FindOrCreateBlock(flow, curState.Name + " " + "Enter");
					if(curState.Update == null)
						curState.Update = FindOrCreateBlock(flow, curState.Name + " " + "Update");
					if(curState.Exit == null)
						curState.Exit = FindOrCreateBlock(flow, curState.Name + " " + "Exit");
				}
			}
		}

		//todo this should move to flowchart or at least flowchart edit util etc.
		static private Block FindOrCreateBlock(Flowchart flow, string blockName)
		{
			//does a block of matching name exist
			var block = flow.FindBlock(blockName);
			if(block == null)
			{
				block = flow.CreateBlock(Vector2.zero);
				block.BlockName = blockName;
			}

			return block;
		}
    }
}