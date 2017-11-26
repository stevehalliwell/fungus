using System;
using System.Collections.Generic;
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

        private HashSet<Type> usedTypes = new HashSet<Type>();

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
//used vars

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
        [VariableProperty(typeof(FloatVariable),
                          typeof(IntegerVariable) )]
		
        protected Variable inOutVar;

        public override void OnEnter()
        {{
            {5}

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

            if (inOutVar == null)
            {{
                return ""Error: no variable set to push or pull data to or from"";
            }}

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

                                var actualName = fields[i].Name;
                                var upperCaseName = Char.ToUpperInvariant(actualName[0]) + actualName.Substring(1);
                                //add it to the enum
                                AddToEnum(upperCaseName);

                                //add it to the get
                                AddToGet(fields[i].FieldType, upperCaseName, actualName);

                                //add it to the set
                                AddToSet(fields[i].FieldType, upperCaseName, actualName);
                            }
                        }
                    }
                    {
                        var props = resTargetClass.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                        for (int i = 0; i < props.Length; i++)
                        {
                            if (IsHandledType(props[i].PropertyType) && ! IsObsolete(props[i].GetCustomAttributes(false)))
                            {
                                var actualName = props[i].Name;
                                var upperCaseName = Char.ToUpperInvariant(actualName[0]) + actualName.Substring(1);
                                //add it to the enum
                                AddToEnum(upperCaseName);

                                if (props[i].CanRead)
                                {
                                    //add it to the get
                                    AddToGet(props[i].PropertyType, upperCaseName, actualName);
                                }
                                if (props[i].CanWrite)
                                {
                                    //add it to the set
                                    AddToSet(props[i].PropertyType, upperCaseName, actualName);
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

                    var typeVars = GetUsedTypeVars();


                    var propScriptContents = string.Format(PropertyScriptTemplate, ClassName, enumgen, lowerClassName, getcontents, setcontents, typeVars);
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

        private string GetUsedTypeVars()
        {
            StringBuilder sb = new StringBuilder();

            /*
            
            var iof = inOutVar as FloatVariable;
            var iob = inOutVar as BooleanVariable;
            var ioi = inOutVar as IntegerVariable;
            var ios = inOutVar as StringVariable;
            var iov = inOutVar as Vector3Variable;
            var iot = inOutVar as TransformVariable;
            var iov2 = inOutVar as Vector2Variable;
            var iogo = inOutVar as GameObjectVariable;
            */

            foreach(Type fieldType in usedTypes)
            {
                if (fieldType == typeof(Single))
                {
                    sb.AppendLine("var iof = inOutVar as FloatVariable;");
                }
                else if (fieldType == typeof(int))
                {
                    sb.AppendLine("var ioi = inOutVar as IntegerVariable;");
                }
                else if (fieldType == typeof(Boolean))
                {
                    sb.AppendLine("var iob = inOutVar as BooleanVariable;");
                }
                else if (fieldType == typeof(string))
                {
                    sb.AppendLine("var ios = inOutVar as StringVariable;");
                }
                else if (fieldType == typeof(Transform))
                {
                    sb.AppendLine("var iot = inOutVar as TransformVariable;");
                }
                else if (fieldType == typeof(Vector3))
                {
                    sb.AppendLine("var iov = inOutVar as Vector3Variable;");
                }
                else if (fieldType == typeof(Vector2))
                {
                    sb.AppendLine("var iov2 = inOutVar as Vector2Variable;");
                }
                else if (fieldType == typeof(GameObject))
                {
                    sb.AppendLine("var iogo = inOutVar as GameObjectVariable;");
                }
            }

            return sb.ToString();
        }

        private string GetSpecificVariableVarientFromType(Type fieldType)
        {
            usedTypes.Add(fieldType);

            if (fieldType == typeof(Single))
            {
                return "iof.Value";
            }
            else if(fieldType == typeof(int))
            {
                return "ioi.Value";
            }
            else if (fieldType == typeof(Boolean))
            {
                return "iob.Value";
            }
            else if (fieldType == typeof(string))
            {
                return "ios.Value";
            }
            else if (fieldType == typeof(Transform))
            {
                return "iot.Value";
            }
            else if (fieldType == typeof(Vector3))
            {
                return "iov.Value";
            }
            else if (fieldType == typeof(Vector2))
            {
                return "iov2.Value";
            }
            else if (fieldType == typeof(GameObject))
            {
                return "iogo.Value";
            }
            return "ERROR - Unsupprted type requested";
        }

        private void AddToSet(Type fieldType, string nameEnum, string nameVariable)
        {
            setBuilder.Append("case Property.");
            setBuilder.Append(nameEnum);
            setBuilder.AppendLine(":");
            setBuilder.Append("target.");
            setBuilder.Append(nameVariable);
            setBuilder.Append(" = ");
            setBuilder.Append(GetSpecificVariableVarientFromType(fieldType));
            setBuilder.Append(";\nbreak;\n");
        }

        private void AddToGet(Type fieldType, string nameEnum, string nameVariable)
        {
            getBuilder.Append("case Property.");
            getBuilder.Append(nameEnum);
            getBuilder.AppendLine(":");
            getBuilder.Append(GetSpecificVariableVarientFromType(fieldType));
            getBuilder.Append(" = target.");
            getBuilder.Append(nameVariable);
            getBuilder.Append(";\nbreak;\n");
        }

        private void AddToEnum(string name)
        {
            enumBuilder.Append(name);
            enumBuilder.AppendLine(", ");
        }

        private bool IsHandledType(Type t)
        {
            return t == typeof(Single) || t == typeof(int) || t == typeof(string) || t == typeof(Boolean)
                || t == typeof(Vector3) || t == typeof(Vector2) || t == typeof(GameObject);
        }

        private bool IsObsolete(object [] attrs)
        {
            if(attrs.Length > 0)
                return attrs.First(x => x.GetType() == typeof(ObsoleteAttribute)) != null;
            return false;
        }
    }
}
