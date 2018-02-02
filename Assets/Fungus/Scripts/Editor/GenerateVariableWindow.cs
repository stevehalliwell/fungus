using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


namespace Fungus.EditorUtils
{
    /// <summary>
    /// Adds window that generates the require scripts to create a new FungusVariable that wraps an existing type. 
    /// 
    /// These can then be used in the fungus flowcharts. It also generates a *Property command to allow Gets and Sets
    /// on all the elements of that variable that Fungus Understands. The Quaternion and Matrix4x4 have been auto
    /// generated and then auto formatted in visual studio and set to preview only as examples of it's use. 
    /// 
    /// It can be used to help building variable wrappers for builtin Unity types or your own components or classes.
    /// 
    /// To add new types see the VariableScriptGenerator constructor.
    /// </summary>
    public class GenerateVariableWindow : EditorWindow
    {
        private VariableScriptGenerator generator = new VariableScriptGenerator();
        private string userInputClassName = "";
        private List<Type> typeList = new List<Type>();

        public void OnGUI()
        {
            DrawMenuPanel();
        }

        private void DrawMenuPanel()
        {
            EditorGUI.BeginChangeCheck();
            userInputClassName = EditorGUILayout.TextField("ClassName", userInputClassName);

            if (EditorGUI.EndChangeCheck())
            {
                generator.TargetType = null;

                try
                {
                    typeList = generator.types.Where(x => x.Name == userInputClassName).ToList();
                }
                catch (Exception)
                {
                }
            }

            try
            {
                int index = typeList.IndexOf(generator.TargetType);
                EditorGUI.BeginChangeCheck();
                index = GUILayout.SelectionGrid(index, typeList.Select(x => x.FullName).ToArray(), 1);

                if (index < 0 || index > typeList.Count)
                    index = 0;

                if (EditorGUI.EndChangeCheck() || generator.TargetType == null)
                    generator.TargetType = typeList[index];
            }
            catch (Exception)
            {
                generator.TargetType = null;
            }
            

            EditorGUILayout.Space();

            if (generator.TargetType == null)
            {
                EditorGUILayout.HelpBox("Must select a type first", MessageType.Info);
            }
            else
            {
                generator.generateVariableClass = EditorGUILayout.Toggle("Generate Variable", generator.generateVariableClass);

                if (generator.TargetType.IsAbstract)
                {
                    EditorGUILayout.HelpBox(generator.TargetType.FullName + " is abstract. No Variable will be generated", MessageType.Error);
                    generator.generateVariableClass = false;
                }

                if (generator.generateVariableClass)
                {
                    if (generator.ExistingGeneratedClass != null)
                    {
                        EditorGUILayout.HelpBox("Variable Appears to already exist. Overwriting or errors may occur.", MessageType.Warning);
                    }
                    if (generator.ExistingGeneratedDrawerClass != null)
                    {
                        EditorGUILayout.HelpBox("Variable Drawer Appears to already exist. Overwriting or errors may occur.", MessageType.Warning);
                    }

                    generator.Category = EditorGUILayout.TextField("Category", generator.Category);
                    generator.NamespaceUsingDeclare = EditorGUILayout.TextField("NamespaceUsingDeclare", generator.NamespaceUsingDeclare);
                }

                EditorGUILayout.Space();
                generator.generatePropertyCommand = EditorGUILayout.Toggle("Generate Property Command", generator.generatePropertyCommand);
                if (generator.generatePropertyCommand)
                {
                    if (generator.ExistingGeneratedPropCommandClass != null)
                    {
                        EditorGUILayout.HelpBox("Variable Appears to already exist. Overwriting or errors may occur.", MessageType.Warning);
                    }

                    generator.PreviewOnly = EditorGUILayout.Toggle("Variable List preview only", generator.PreviewOnly);
                }

                EditorGUILayout.Space();
                if (GUILayout.Button("Generate Now"))
                {
                    try
                    {
                        generator.Generate();
                        EditorUtility.DisplayProgressBar("Generating " + userInputClassName, "Importing Scripts", 0);
                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        //throw e;
                    }
                    generator = new VariableScriptGenerator();
                    EditorUtility.ClearProgressBar();
                }
            }
        }

        [MenuItem("Tools/Fungus/Utilities/Generate Fungus Varaible")]
        public static GenerateVariableWindow ShowWindow()
        {
            var w = GetWindow(typeof(GenerateVariableWindow), true, "Generate Fungus Varaible", true);
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
        public string NamespaceUsingDeclare { get; set; }
        
        public bool PreviewOnly { get; set; }
        private string _category = "Other";
        public string Category { get { return _category; } set { _category = value; } }

        public List<Type> types { get; private set; }

        public bool generateVariableClass = true, generatePropertyCommand = true;

        public string ClassName { get { return TargetType.Name; } }
        public string CamelCaseClassName { get { return Char.ToLowerInvariant(ClassName[0]) + ClassName.Substring(1);}}

        public string GenClassName { get { return ClassName + "Variable"; } }

        public string VariableFileName { get { return VaraibleScriptLocation + ClassName + "Variable.cs"; } }
        public string VariableEditorFileName { get { return EditorScriptLocation + ClassName + "VariableDrawer.cs"; } }
        public string PropertyFileName { get { return PropertyScriptLocation + ClassName + "Property.cs"; } }

        private Type _targetType;
        public Type TargetType
        {
            get
            {
                return _targetType;
            }
            set
            {
                _targetType = value;
                ExistingGeneratedClass = null;
                ExistingGeneratedDrawerClass = null;
                ExistingGeneratedPropCommandClass = null;

                if (_targetType != null)
                {
                    ExistingGeneratedClass = types.Find(x => x.Name == GenClassName);
                    ExistingGeneratedDrawerClass = types.Find(x => x.Name == (ClassName + "VariableDrawer"));
                    ExistingGeneratedPropCommandClass = types.Find(x => x.Name == (ClassName + "Property"));
                }
            }
        }

        public Type ExistingGeneratedClass { get; private set; }
        public Type ExistingGeneratedDrawerClass { get; private set; }
        public Type ExistingGeneratedPropCommandClass { get; private set; }

        StringBuilder enumBuilder, getBuilder, setBuilder;// = new StringBuilder("switch (property)\n{");

        //data and helper for a single native to single fungus type
        public class FungusVariableTypeHelper
        {

            private HashSet<Type> usedTypes = new HashSet<Type>();
            private List<TypeHandler> handlers = new List<TypeHandler>();

            public class TypeHandler
            {
                public TypeHandler(Type native, Type fungusType, string localName, string nativeAsString = null, string fungusTypeAsString = null)
                {
                    NativeType = native;
                    NativeTypeString = string.IsNullOrEmpty(nativeAsString) ? native.Name : nativeAsString;
                    FungusType = fungusType;
                    FungusTypeString = string.IsNullOrEmpty(fungusTypeAsString) ? fungusType.Name : fungusTypeAsString;
                    LocalVariableName = localName;
                }

                public Type NativeType { get; set; }
                public Type FungusType { get; set; }
                public string NativeTypeString { get; set; }
                public string FungusTypeString { get; set; }
                public string LocalVariableName { get; set; }
                public string GetLocalVariableNameWithDeclaration()
                {
                    return "var " + LocalVariableName + " = inOutVar as " + FungusTypeString + ';';
                }
                public string GetVarPropText()
                {
                    return "typeof(" + FungusTypeString + ")";
                }
            }

            public void AddHandler(TypeHandler t)
            {
                handlers.Add(t);
            }

            public bool IsTypeHandled(Type t)
            {
                return handlers.Find(x => x.NativeType == t) != null;
            }

            public string GetSpecificVariableVarientFromType(Type t)
            {
                usedTypes.Add(t);

                var loc = handlers.Find(x => x.NativeType == t);
                if (loc != null)
                {
                    return loc.LocalVariableName;
                }
                else
                {
                    return "ERROR - Unsupprted type requested";
                }
            }

            public string GetUsedTypeVars()
            {

                StringBuilder sb = new StringBuilder();

                foreach (Type t in usedTypes)
                {
                    var loc = handlers.Find(x => x.NativeType == t);
                    if (loc != null)
                    {
                        sb.Append("            ");
                        sb.AppendLine(loc.GetLocalVariableNameWithDeclaration());
                    }
                }

                return sb.ToString();
            }

            public string GetVariablePropertyTypeOfs()
            {
                StringBuilder sb = new StringBuilder();

                foreach (Type t in usedTypes)
                {
                    var loc = handlers.Find(x => x.NativeType == t);
                    if (loc != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.AppendLine(",");
                            sb.Append("                          ");
                        }
                        sb.Append(loc.GetVarPropText());
                    }
                }

                return sb.ToString();
            }
        }

        private FungusVariableTypeHelper helper = new FungusVariableTypeHelper();

        #region consts
        const string ScriptLocation = "./Assets/Fungus/Scripts/";
        const string PropertyScriptLocation = ScriptLocation + "Commands/Property/";
        const string VaraibleScriptLocation = ScriptLocation + "VariableTypes/";
        const string EditorScriptLocation = VaraibleScriptLocation + "Editor/";

        const string EditorScriptTemplate = @"/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEditor;
using UnityEngine;

namespace Fungus.EditorUtils
{{
    [CustomPropertyDrawer(typeof({0}Data))]
    public class {0}DataDrawer : VariableDataDrawer<{0}Variable>
    {{ }}
}}";



        //0 ClassName
        //1 NamespaceOfClass 
        //2 lowerClassName
        //3 Category
        //4 previewOnly
        //5 full name

        const string VariableScriptTemplate = @"/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;
{1}

namespace Fungus
{{
    /// <summary>
    /// {0} variable type.
    /// </summary>
    [VariableInfo(""{3}"", ""{0}""{4})]
    [AddComponentMenu("""")]
	[System.Serializable]
	public class {0}Variable : VariableBase<{5}>
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
		public {5} {2}Val;

		public static implicit operator {5}({0}Data {0}Data)
		{{
			return {0}Data.Value;
		}}

		public {0}Data({5} v)
		{{
			{2}Val = v;
			{2}Ref = null;
		}}

		public {5} Value
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

        //0 typeo
        //1 prop enum
        //2 lower class name
        //3 get generated
        //4 set generated
        //5 used vars
        //6 variableProperty Type of
        //7 null check summary
        const string PropertyScriptTemplate = @"/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


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
        [VariableProperty({6})]
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
        {{{7}
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
        const string DefaultCaseFailure = @"                        default:
                            Debug.Log(""Unsupported get or set attempted"");
                            break;
                    }";
        const string NullCheckSummary = @"
            if ({0}Data.Value == null)
            {{
                return ""Error: no {0} set"";
            }}";


        #endregion
        public VariableScriptGenerator()
        {
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(bool), typeof(BooleanVariable), "iob"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(float), typeof(FloatVariable), "iof"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(int), typeof(IntegerVariable), "ioi"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(string), typeof(StringVariable), "ios"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Animator), typeof(AnimatorVariable), "ioani"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(AudioSource), typeof(AudioSourceVariable), "ioaud"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Color), typeof(ColorVariable), "iocol"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(GameObject), typeof(GameObjectVariable), "iogo"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Material), typeof(MaterialVariable), "iomat"));
            //we skip object
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Rigidbody2D), typeof(Rigidbody2DVariable), "iorb2d"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Sprite), typeof(SpriteVariable), "iospr"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Texture), typeof(TextureVariable), "iotex"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Transform), typeof(TransformVariable), "iot"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Vector2), typeof(Vector2Variable), "iov2"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Vector3), typeof(Vector3Variable), "iov"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Quaternion), typeof(QuaternionVariable), "ioq"));
            helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Matrix4x4), typeof(Matrix4x4Variable), "iom4"));
            //helper.AddHandler(new FungusVariableTypeHelper.TypeHandler(typeof(Rigidbody), typeof(RigidbodyVariable), "iorb"));


            types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        public void Generate()
        {    
            if (TargetType == null)
                throw new Exception("No type given");
            

            EditorUtility.DisplayProgressBar("Generating " + ClassName, "Starting", 0);
            try
            {
                if (generateVariableClass)
                {
                    Func<string> lam = () => {
                        var usingDec = !string.IsNullOrEmpty(NamespaceUsingDeclare) ? ("using " + NamespaceUsingDeclare + ";") : string.Empty;
                        return string.Format(VariableScriptTemplate, ClassName, usingDec, CamelCaseClassName, Category, PreviewOnly ? ", IsPreviewedOnly = true" : "", TargetType.FullName);
                    };
                    FileSaveHelper("Variable", VaraibleScriptLocation, VariableFileName, lam);
                }
                
                if (generateVariableClass)
                {
                    Func<string> lam = () => { return string.Format(EditorScriptTemplate, ClassName); };
                    FileSaveHelper("VariableDrawer", EditorScriptLocation, VariableEditorFileName, lam);
                }
                
                if (generatePropertyCommand)
                {
                    enumBuilder = new StringBuilder("public enum Property \n        { \n".Replace("\n", System.Environment.NewLine));
                    getBuilder = new StringBuilder("switch (property)\n                    {\n".Replace("\n", System.Environment.NewLine));
                    setBuilder = new StringBuilder("switch (property)\n                    {\n".Replace("\n", System.Environment.NewLine));

                    EditorUtility.DisplayProgressBar("Generating " + ClassName, "Property", 0);
                    PropertyFieldLogic();
                    PropertyPropLogic();


                    EditorUtility.DisplayProgressBar("Generating " + ClassName, "Property Building", 0);

                    //finalise buidlers
                    setBuilder.AppendLine(DefaultCaseFailure);
                    var setcontents = setBuilder.ToString();

                    getBuilder.AppendLine(DefaultCaseFailure);
                    var getcontents = getBuilder.ToString();

                    enumBuilder.AppendLine("        }");
                    var enumgen = enumBuilder.ToString();

                    var typeVars = helper.GetUsedTypeVars();
                    var variablePropertyTypes = helper.GetVariablePropertyTypeOfs();

                    string nullCheck = "";

                    if (TargetType.IsClass)
                    {
                        nullCheck = string.Format(NullCheckSummary, CamelCaseClassName);
                    }


                    //write to file
                    Func<string> propContentOp = () => { return string.Format(PropertyScriptTemplate, ClassName, enumgen, CamelCaseClassName, getcontents, setcontents, typeVars, variablePropertyTypes, nullCheck); };
                    FileSaveHelper("Property", PropertyScriptLocation, PropertyFileName, propContentOp);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            EditorUtility.ClearProgressBar();
        }

        private void PropertyFieldLogic()
        {
            EditorUtility.DisplayProgressBar("Generating " + ClassName, "Property Scanning Fields", 0);
            var fields = TargetType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            for (int i = 0; i < fields.Length; i++)
            {
                if (helper.IsTypeHandled(fields[i].FieldType))
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

        private void PropertyPropLogic()
        {
            EditorUtility.DisplayProgressBar("Generating " + ClassName, "Property Scanning Props", 0);
            var props = TargetType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            for (int i = 0; i < props.Length; i++)
            {
                if (helper.IsTypeHandled(props[i].PropertyType) && props[i].GetIndexParameters().Length == 0 && !IsObsolete(props[i].GetCustomAttributes(false)))
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

        private void AddToSet(Type fieldType, string nameEnum, string nameVariable)
        {
            setBuilder.Append("                        case Property.");
            setBuilder.Append(nameEnum);
            setBuilder.AppendLine(":");
            setBuilder.Append("                            target.");
            setBuilder.Append(nameVariable);
            setBuilder.Append(" = ");
            setBuilder.Append(helper.GetSpecificVariableVarientFromType(fieldType));
            setBuilder.AppendLine(".Value;");
            setBuilder.AppendLine("                            break;");
        }

        private void AddToGet(Type fieldType, string nameEnum, string nameVariable)
        {
            getBuilder.Append("                        case Property.");
            getBuilder.Append(nameEnum);
            getBuilder.AppendLine(":");
            getBuilder.Append("                            " + helper.GetSpecificVariableVarientFromType(fieldType));
            getBuilder.Append(".Value = target.");
            getBuilder.Append(nameVariable);
            getBuilder.AppendLine(";");
            getBuilder.AppendLine("                            break;");
        }

        private void AddToEnum(string name)
        {
            enumBuilder.Append("            ");
            enumBuilder.Append(name);
            enumBuilder.AppendLine(", ");
        }

        private bool IsObsolete(object[] attrs)
        {
            if (attrs.Length > 0)
                return attrs.First(x => x.GetType() == typeof(ObsoleteAttribute)) != null;
            return false;
        }

        private void FileSaveHelper(string op, string loc, string filename, Func<string> opLambda)
        {
            EditorUtility.DisplayProgressBar("Generating " + ClassName, op, 0);
            var scriptContents = opLambda();
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(loc));
            System.IO.File.WriteAllText(filename, scriptContents);
            Debug.Log("Created " + filename);
        }
    }
}
