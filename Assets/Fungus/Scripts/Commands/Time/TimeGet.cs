using UnityEngine;

namespace Fungus
{
	/// <summary>
	/// Fetch and store and value from Unity's Time.
	/// </summary>
	[CommandInfo("Time",
		"Get",
		"Fetch and store and value from Unity's Time.")]
	[AddComponentMenu("")]
	public class TimeGet : Command
	{
		public enum TimeVariable
		{
			captureFramerate,
			deltaTime,
			fixedDeltaTime,
			fixedTime,
			fixedUnscaledDeltaTime,
			fixedUnscaledTime,
			frameCount,
			inFixedTimeStep,
			maximumDeltaTime,
			maximumParticleDeltaTime,
			realtimeSinceStartup,
			smoothDeltaTime,
			time,
			timeScale,
			timeSinceLevelLoad,
			unscaledDeltaTime,
			unscaledTime,
		}


		[Tooltip("Which ")]
		[SerializeField]
		protected TimeVariable variable = TimeVariable.deltaTime;

		[Tooltip("Varaible to store the value in.")]
		[SerializeField]
		[VariableProperty(typeof(BooleanVariable),
			typeof(IntegerVariable), 
			typeof(FloatVariable))]
		protected Variable storeValueIn;

		public override void OnEnter()
		{
			var asIntVar = storeValueIn as IntegerVariable;
			var asFloatVar = storeValueIn as FloatVariable;
			var asBoolVar = storeValueIn as BooleanVariable;

			switch (variable)
			{
				case TimeVariable.captureFramerate:
					if(asIntVar != null)
					{
						asIntVar.Value = Time.captureFramerate;
					}
					break;
				case TimeVariable.deltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.deltaTime;
					}
					break;
				case TimeVariable.fixedDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.fixedDeltaTime;
					}
					break;
				case TimeVariable.fixedUnscaledDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.fixedUnscaledDeltaTime;
					}
					break;
				case TimeVariable.fixedUnscaledTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.fixedUnscaledTime;
					}
					break;
				case TimeVariable.frameCount:
					if(asIntVar != null)
					{
						asIntVar.Value = Time.frameCount;
					}
					break;
				case TimeVariable.inFixedTimeStep:
					if(asBoolVar != null)
					{
						asBoolVar.Value = Time.inFixedTimeStep;
					}
					break;
				case TimeVariable.maximumDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.maximumDeltaTime;
					}
					break;
				case TimeVariable.maximumParticleDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.maximumParticleDeltaTime;
					}
					break;
				case TimeVariable.realtimeSinceStartup:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.realtimeSinceStartup;
					}
					break;
				case TimeVariable.smoothDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.smoothDeltaTime;
					}
					break;
				case TimeVariable.time:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.time;
					}
					break;
				case TimeVariable.timeScale:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.timeScale;
					}
					break;
				case TimeVariable.timeSinceLevelLoad:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.timeSinceLevelLoad;
					}
					break;
				case TimeVariable.unscaledDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.unscaledDeltaTime;
					}
					break;
				case TimeVariable.unscaledTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.unscaledTime;
					}
					break;
				default:
					break;
			}
			
			Continue();
		}

		public override string GetSummary()
		{
			if(storeValueIn == null)
				return "No store variable set.";
			
			return variable.ToString();
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

	}
}