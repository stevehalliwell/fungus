using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Matrix4x4 variable type.
    /// </summary>
    [VariableInfo("Other", "Matrix4x4", isPreviewedOnly:true)]
    [AddComponentMenu("")]
	[System.Serializable]
	public class Matrix4x4Variable : VariableBase<Matrix4x4>
	{ }

	/// <summary>
	/// Container for a Matrix4x4 variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct Matrix4x4Data
	{
		[SerializeField]
		[VariableProperty("<Value>", typeof(Matrix4x4Variable))]
		public Matrix4x4Variable matrix4x4Ref;

		[SerializeField]
		public Matrix4x4 matrix4x4Val;

		public static implicit operator Matrix4x4(Matrix4x4Data Matrix4x4Data)
		{
			return Matrix4x4Data.Value;
		}

		public Matrix4x4Data(Matrix4x4 v)
		{
			matrix4x4Val = v;
			matrix4x4Ref = null;
		}

		public Matrix4x4 Value
		{
			get { return (matrix4x4Ref == null) ? matrix4x4Val : matrix4x4Ref.Value; }
			set { if (matrix4x4Ref == null) { matrix4x4Val = value; } else { matrix4x4Ref.Value = value; } }
		}

		public string GetDescription()
		{
			if (matrix4x4Ref == null)
			{
				return matrix4x4Val.ToString();
			}
			else
			{
				return matrix4x4Ref.Key;
			}
		}
	}
}