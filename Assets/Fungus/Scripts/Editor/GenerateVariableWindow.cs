using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fungus.EditorUtils
{
    [Serializable]
    public class GenerateVariableWindow : EditorWindow
    {
        private VariableScriptGenerator generator = new VariableScriptGenerator();

        public void OnGUI()
        {
            DrawMenuPanel();
        }

        private void DrawMenuPanel()
        {
            //EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            //EditorGUILayout.LabelField("Group by:", Styles.toolbarLabel, GUILayout.MaxWidth(60));
            //m_GroupBy = (GroupByType)EditorGUILayout.EnumPopup(m_GroupBy, EditorStyles.toolbarPopup, GUILayout.MaxWidth(150));

            //GUILayout.FlexibleSpace();

            //m_ShowType = (ShowType)EditorGUILayout.EnumPopup(m_ShowType, EditorStyles.toolbarPopup, GUILayout.MaxWidth(100));

            //EditorGUILayout.LabelField("Filter by:", Styles.toolbarLabel, GUILayout.MaxWidth(50));
            //m_FilterType = (FilterType)EditorGUILayout.EnumPopup(m_FilterType, EditorStyles.toolbarPopup, GUILayout.MaxWidth(100));
            //m_FilterText = GUILayout.TextField(m_FilterText, "ToolbarSeachTextField", GUILayout.MaxWidth(100));
            //if (GUILayout.Button(GUIContent.none, string.IsNullOrEmpty(m_FilterText) ? "ToolbarSeachCancelButtonEmpty" : "ToolbarSeachCancelButton", GUILayout.ExpandWidth(false)))
            //    m_FilterText = "";
            //EditorGUILayout.EndHorizontal();

            generator.NamespaceOfClass = EditorGUILayout.TextField("NamespaceOfClass", generator.NamespaceOfClass);
            generator.ClassName = EditorGUILayout.TextField("ClassName", generator.ClassName);
            generator.Category = EditorGUILayout.TextField("Category", generator.Category);

            if (GUILayout.Button("Generate Now"))
            {
                try
                {
                    generator.Generate();
                    generator = new VariableScriptGenerator();
                    AssetDatabase.Refresh();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }

        [MenuItem("Tools/Fungus/Utilities/Generate Fungus Varaible")]
        public static GenerateVariableWindow ShowWindow()
        {
            var w = GetWindow(typeof(GenerateVariableWindow),true, "Generate Fungus Varaible",true);
            w.Show();
            return w as GenerateVariableWindow;
        }
    }

    /// <summary>
    /// Helper script that auto generates the required script for FungusVariables that wrap existing classes.
    /// 
    /// Intended to speed up the creation of fungus wrappers of unity builtin types and custom user types.
    /// </summary>
    public class VariableScriptGenerator
    {
        private string _namespaceOfClass = "UnityEngine";
        public string NamespaceOfClass { get { return _namespaceOfClass; } set { _namespaceOfClass = value; } }
        public string ClassName { get; set; }
        private string _category = "Other";
        public string Category { get { return _category; } set { _category = value; } }

        #region consts
        const string VaraibleScriptLocation = "./Assets/Fungus/Scripts/VariableTypes/";
        const string EditorScriptLocation = VaraibleScriptLocation + "Editor/";
        const string EditorScriptTemplate = @"using UnityEditor;
using UnityEngine;

namespace Fungus.EditorUtils
{{
    [CustomPropertyDrawer(typeof({0}Data))]
    public class {0}DataDrawer : VariableDataDrawer<{0}Variable>
    {{ }}
}}";

        const string ScriptTemplate = @"using {1};

namespace Fungus
{{
    /// <summary>
    /// {0} variable type.
    /// </summary>
    [VariableInfo(""{3}"", ""{0}"")]
    [AddComponentMenu("""")]
	[System.Serializable]
	public class {0}Variable : VariableBase<{0}>
	{{ }}

	/// <summary>
	/// Container for a {0} variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct {0}Data
	{{
		[SerializeField]
		[VariableProperty(""<Value>"", typeof({0}Variable))]
		public {0}Variable {2}Ref;

		[SerializeField]
		public {0} {2}Val;

		public static implicit operator {0}({0}Data {0}Data)
		{{
			return {0}Data.Value;
		}}

		public {0}Data({0} v)
		{{
			{2}Val = v;
			{2}Ref = null;
		}}

		public {0} Value
		{{
			get {{ return ({2}Ref == null) ? {2}Val : {2}Ref.Value; }}
			set {{ if ({2}Ref == null) {{ {2}Val = value; }} else {{ {2}Ref.Value = value; }} }}
		}}

		public string GetDescription()
		{{
			if ({2}Ref == null)
			{{
				return {2}Val.ToString();
			}}
			else
			{{
				return {2}Ref.Key;
			}}
		}}
	}}
}}";

#endregion
        public VariableScriptGenerator()
        {
        }

        public void Generate()
        {
            if (ClassName.Length == 0)
                throw new Exception("No Class name provided.");

            //must be a valid class
            Type res = AppDomain.CurrentDomain.GetAssemblies()
                                   .SelectMany(x => x.GetTypes())
                                   .First(x => x.Name == ClassName);

            if (res == null)
                throw new Exception("No Type of name "+ClassName+" exists.");

            //don't allow dups
            try
            {
                var lowerClassName = Char.ToLowerInvariant(ClassName[0]) + ClassName.Substring(1);
                var scriptContents = string.Format(ScriptTemplate, ClassName, NamespaceOfClass, lowerClassName, Category);
                System.IO.File.WriteAllText(VaraibleScriptLocation + ClassName + "Variable.cs", scriptContents);

                var editorScriptContents = string.Format(EditorScriptTemplate, ClassName, NamespaceOfClass, lowerClassName, Category);
                System.IO.File.WriteAllText(EditorScriptLocation + ClassName + "VariableDrawer.cs", editorScriptContents);
                
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
