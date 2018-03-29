/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    /// <summary>
    /// Collision variable type.
    /// </summary>
    [VariableInfo("Other", "Collision")]
    [AddComponentMenu("")]
	[System.Serializable]
	public class CollisionVariable : VariableBase<UnityEngine.Collision>
	{ }

	/// <summary>
	/// Container for a Collision variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct CollisionData
	{
		[SerializeField]
		[VariableProperty("<Value>", typeof(CollisionVariable))]
		public CollisionVariable collisionRef;

		[SerializeField]
		public UnityEngine.Collision collisionVal;

		public static implicit operator UnityEngine.Collision(CollisionData CollisionData)
		{
			return CollisionData.Value;
		}

		public CollisionData(UnityEngine.Collision v)
		{
			collisionVal = v;
			collisionRef = null;
		}

		public UnityEngine.Collision Value
		{
			get { return (collisionRef == null) ? collisionVal : collisionRef.Value; }
			set { if (collisionRef == null) { collisionVal = value; } else { collisionRef.Value = value; } }
		}

		public string GetDescription()
		{
			if (collisionRef == null)
			{
				return collisionVal.ToString();
			}
			else
			{
				return collisionRef.Key;
			}
		}
	}
}