// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif//UNITY_EDITOR
using System;

namespace Fungus
{
    /// <summary>
    /// Standard comparison operators.
    /// </summary>
    public enum CompareOperator
    {
        /// <summary> == mathematical operator.</summary>
        Equals,
        /// <summary> != mathematical operator.</summary>
        NotEquals,
        /// <summary> < mathematical operator.</summary>
        LessThan,
        /// <summary> > mathematical operator.</summary>
        GreaterThan,
        /// <summary> <= mathematical operator.</summary>
        LessThanOrEquals,
        /// <summary> >= mathematical operator.</summary>
        GreaterThanOrEquals
    }

    /// <summary>
    /// Mathematical operations that can be performed on variables.
    /// </summary>
    public enum SetOperator
    {
        /// <summary> = operator. </summary>
        Assign,
        /// <summary> =! operator. </summary>
        Negate,
        /// <summary> += operator. </summary>
        Add,
        /// <summary> -= operator. </summary>
        Subtract,
        /// <summary> *= operator. </summary>
        Multiply,
        /// <summary> /= operator. </summary>
        Divide
    }

    /// <summary>
    /// Scope types for Variables.
    /// </summary>
    public enum VariableScope
    {
        /// <summary> Can only be accessed by commands in the same Flowchart. </summary>
        Private,
        /// <summary> Can be accessed from any command in any Flowchart. </summary>
        Public,
        /// <summary> Creates and/or references a global variable of that name, all variables of this name and scope share the same underlying fungus variable and exist for the duration of the instance of Unity.</summary>
        Global,
    }

    /// <summary>
    /// Attribute class for variables.
    /// </summary>
    public class VariableInfoAttribute : Attribute
    {
        //Note do not use "isPreviewedOnly:true", it causes the script to fail to load without errors shown
        public VariableInfoAttribute(string category, string variableType, int order = 0, bool isPreviewedOnly = false, bool hasCustomDraw = false)
        {
            this.Category = category;
            this.VariableType = variableType;
            this.Order = order;
            this.IsPreviewedOnly = isPreviewedOnly;
            this.HasCustomDraw = hasCustomDraw;
        }
        
        public string Category { get; set; }
        public string VariableType { get; set; }
        public int Order { get; set; }
        public bool IsPreviewedOnly { get; set; }
        public bool HasCustomDraw { get; set; }
    }

    /// <summary>
    /// Attribute class for variable properties.
    /// </summary>
    public class VariablePropertyAttribute : PropertyAttribute 
    {
        public VariablePropertyAttribute (params System.Type[] variableTypes) 
        {
            this.VariableTypes = variableTypes;
        }

        public VariablePropertyAttribute (string defaultText, params System.Type[] variableTypes) 
        {
            this.defaultText = defaultText;
            this.VariableTypes = variableTypes;
        }

        public String defaultText = "<None>";

        public Type[] VariableTypes { get; set; }
    }

    /// <summary>
    /// Abstract base class for variables.
    /// </summary>
    [RequireComponent(typeof(Flowchart))]
    public abstract class Variable : MonoBehaviour
    {
        [SerializeField] protected VariableScope scope;

        [SerializeField] protected string key = "";

        #region Public members

        /// <summary>
        /// Visibility scope for the variable.
        /// </summary>
        public virtual VariableScope Scope { get { return scope; } set { scope = value; } }

        /// <summary>
        /// String identifier for the variable.
        /// </summary>
        public virtual string Key { get { return key; } set { key = value; } }

        /// <summary>
        /// Callback to reset the variable if the Flowchart is reset.
        /// </summary>
        public abstract void OnReset();

#if UNITY_EDITOR
        /// <summary>
        /// Method to be overloaded in child classes to alter how a variable type draws itself
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="valueProp"></param>
        public virtual void InternalDrawProperty(Rect rect, SerializedProperty valueProp)
        {
            Debug.LogError("Internal Draw Property called when no specialisation is provided:" + this.ToString());
        }
#endif//UNITY_EDITOR

        #endregion
    }

    /// <summary>
    /// Generic concrete base class for variables.
    /// </summary>
    public abstract class VariableBase<T> : Variable
    {
        //caching mechanism for global static variables
        private VariableBase<T> _globalStaicRef;
        private VariableBase<T> globalStaicRef
        {
            get
            {
                if (_globalStaicRef != null)
                {
                    return _globalStaicRef;
                }
                else if(Application.isPlaying)
                {
                    return _globalStaicRef = FungusManager.Instance.GlobalVariables.GetOrAddVariable(Key, value, this.GetType());
                }
                else
                {
                    return null;
                }
            }
        }

        [SerializeField] protected T value;
        public virtual T Value
        {
            get
            {
                if (scope != VariableScope.Global || !Application.isPlaying)
                {
                    return this.value;
                }
                else
                { 
                    return globalStaicRef.value;
                }
            }
            set
            {
                if (scope != VariableScope.Global || !Application.isPlaying)
                {
                    this.value = value;
                }
                else
                {
                    globalStaicRef.Value = value;
                }
            }
        }

        protected T startValue;

        public override void OnReset()
        {
            Value = startValue;
        }
        
        public override string ToString()
        {
            if (Value != null)
                return Value.ToString();
            else
                return string.Empty;
        }
        
        protected virtual void Start()
        {
            // Remember the initial value so we can reset later on
            startValue = Value;
        }

        public virtual void Apply(SetOperator setOperator, T value) {
            Debug.LogError("Variable doesn't have any operators.");
        }
    }
}
