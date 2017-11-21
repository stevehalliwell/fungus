using System;
using System.Linq;
using System.Text;
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
                    //throw e;
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

        StringBuilder enumBuilder = new StringBuilder("public enum Property { "), getBuilder = new StringBuilder("switch (property)\n{"), setBuilder = new StringBuilder("switch (property)\n{");

        #region consts
        const string ScriptLocation = "./Assets/Fungus/Scripts/";
        const string VaraibleScriptLocation = ScriptLocation + "VariableTypes/";
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
        const string PropertyScriptTemplate = @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0 typeo
//1 prop enum
//2 lower class name
//3 get generated
//4 set generated

namespace Fungus
{{
    // <summary>
    /// Get or Set a property of a {0} component
    /// </summary>
    [CommandInfo(""{0}"",
                 ""Property"",
                 ""Get or Set a property of a {0} component"")]
    [AddComponentMenu("""")]
    public class {0}Property : BaseVariableProperty
    {{
		//generated property
        {1}
		
        [SerializeField]
        protected Property property;
		
        [SerializeField]
        protected {0}Data {2}Data;

        [SerializeField]
        [VariableProperty(typeof(FloatVariable))]
		
        protected Variable inOutVar;

        public override void OnEnter()
        {{
            var iof = inOutVar as FloatVariable;

            var target = {2}Data.Value;

            switch (getOrSet)
            {{
                case GetSet.Get:
                    {3}
                    break;
                case GetSet.Set:
                    {4}
                    break;
                default:
                    break;
            }}

            Continue();
        }}

        public override string GetSummary()
        {{
            if ({2}Data.Value == null)
            {{
                return ""Error: no {2} set"";
            }}

            var iof = inOutVar as FloatVariable;

            if (iof == null)
            {{
                return ""Error: no variable set to push or pull data to or from"";
            }}

            //We could do further checks here, eg, you have selected childcount but set a vec3variable

            return getOrSet.ToString() + "" "" + property.ToString();
        }}

        public override Color GetButtonColor()
        {{
            return new Color32(235, 191, 217, 255);
        }}

        public override bool HasReference(Variable variable)
        {{
            if ({2}Data.{2}Ref == variable || inOutVar == variable)
                return true;

            return false;
        }}

    }}
}}";
        const string DefaultCaseFailure = @" default:
                                Debug.Log(""Unsupported get or set attempted"");
                            break;
                    }";


#endregion
        public VariableScriptGenerator()
        {
        }

        public void Generate()
        {
            if (ClassName.Length == 0)
                throw new Exception("No Class name provided.");

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();

            //must be a valid class
            Type resTargetClass = types.Find(x => x.Name == ClassName);

            if (resTargetClass == null)
                throw new Exception("No Type of name "+ClassName+" exists.");

            //don't allow dups
            var genClassName = (ClassName + "Variable");
            Type resGeneratedClass = types.Find(x => x.Name == genClassName);
            Type resGeneratedDrawerClass = types.Find(x => x.Name == (ClassName + "VariableDrawer"));
            Type resGeneratedPropCommandClass = types.Find(x => x.Name == (ClassName + "Property"));


            try
            {
                var lowerClassName = Char.ToLowerInvariant(ClassName[0]) + ClassName.Substring(1);
                if (resGeneratedClass == null && !resTargetClass.IsAbstract)
                {
                    var scriptContents = string.Format(ScriptTemplate, ClassName, NamespaceOfClass, lowerClassName, Category);
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(VaraibleScriptLocation));
                    System.IO.File.WriteAllText(VaraibleScriptLocation + ClassName + "Variable.cs", scriptContents);
                }

                if (resGeneratedDrawerClass == null && !resTargetClass.IsAbstract)
                {
                    var editorScriptContents = string.Format(EditorScriptTemplate, ClassName, NamespaceOfClass, lowerClassName, Category);
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(EditorScriptLocation));
                    System.IO.File.WriteAllText(EditorScriptLocation + ClassName + "VariableDrawer.cs", editorScriptContents);
                }

                if(resGeneratedPropCommandClass == null)
                {
                    {
                        var fields = resTargetClass.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                        for (int i = 0; i < fields.Length; i++)
                        {
                            if (IsHandledType(fields[i].FieldType))
                            {
                                //add it to the enum
                                AddToEnum(fields[i].Name);

                                //add it to the get
                                AddToGet(fields[i].FieldType, fields[i].Name);

                                //add it to the set
                                AddToSet(fields[i].FieldType, fields[i].Name);
                            }
                        }
                    }
                    {
                        var props = resTargetClass.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                        for (int i = 0; i < props.Length; i++)
                        {
                            if (IsHandledType(props[i].PropertyType) && ! IsObsolete(props[i].GetCustomAttributes(false)))
                            {
                                //add it to the enum
                                AddToEnum(props[i].Name);

                                if (props[i].CanRead)
                                {
                                    //add it to the get
                                    AddToGet(props[i].PropertyType, props[i].Name);
                                }
                                if (props[i].CanWrite)
                                {
                                    //add it to the set
                                    AddToSet(props[i].PropertyType, props[i].Name);
                                }
                            }
                        }
                    }

                    //finalise buidlers
                    setBuilder.AppendLine(DefaultCaseFailure);
                    var setcontents = setBuilder.ToString();

                    getBuilder.AppendLine(DefaultCaseFailure);
                    var getcontents = getBuilder.ToString();

                    enumBuilder.AppendLine("}");
                    var enumgen = enumBuilder.ToString();


                    var propScriptContents = string.Format(PropertyScriptTemplate, ClassName, enumgen, lowerClassName, getcontents, setcontents);
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ScriptLocation));
                    System.IO.File.WriteAllText(ScriptLocation + ClassName + "Property.cs", propScriptContents);

                    //insert into template script
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void AddToSet(Type fieldType, string name)
        {
            setBuilder.Append("case Property.");
            setBuilder.Append(name);
            setBuilder.AppendLine(":");
            setBuilder.Append("iof.Value = target.");
            setBuilder.Append(name);
            setBuilder.Append(";\nbreak;\n");
        }

        private void AddToGet(Type fieldType, string name)
        {
            getBuilder.Append("case Property.");
            getBuilder.Append(name);
            getBuilder.AppendLine(":");
            getBuilder.Append("iof.Value = target.");
            getBuilder.Append(name);
            getBuilder.Append(";\nbreak;\n");
        }

        private void AddToEnum(string name)
        {
            enumBuilder.Append(name);
            enumBuilder.AppendLine(", ");
        }

        private bool IsHandledType(Type t)
        {
            return t == typeof(Single);
        }

        private bool IsObsolete(object [] attrs)
        {
            if(attrs.Length > 0)
                return attrs.First(x => x.GetType() == typeof(ObsoleteAttribute)) != null;
            return false;
        }
    }
}
