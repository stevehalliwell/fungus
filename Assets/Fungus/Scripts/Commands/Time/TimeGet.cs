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
		public enum TimeGetVariable
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


		[Tooltip("Which part of Time do you want to fetch")]
		[SerializeField]
		protected TimeGetVariable variable = TimeGetVariable.deltaTime;

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
				case TimeGetVariable.captureFramerate:
					if(asIntVar != null)
					{
						asIntVar.Value = Time.captureFramerate;
					}
					break;
				case TimeGetVariable.deltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.deltaTime;
					}
					break;
				case TimeGetVariable.fixedDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.fixedDeltaTime;
					}
					break;
				case TimeGetVariable.fixedUnscaledDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.fixedUnscaledDeltaTime;
					}
					break;
				case TimeGetVariable.fixedUnscaledTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.fixedUnscaledTime;
					}
					break;
				case TimeGetVariable.frameCount:
					if(asIntVar != null)
					{
						asIntVar.Value = Time.frameCount;
					}
					break;
				case TimeGetVariable.inFixedTimeStep:
					if(asBoolVar != null)
					{
						asBoolVar.Value = Time.inFixedTimeStep;
					}
					break;
				case TimeGetVariable.maximumDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.maximumDeltaTime;
					}
					break;
				case TimeGetVariable.maximumParticleDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.maximumParticleDeltaTime;
					}
					break;
				case TimeGetVariable.realtimeSinceStartup:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.realtimeSinceStartup;
					}
					break;
				case TimeGetVariable.smoothDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.smoothDeltaTime;
					}
					break;
				case TimeGetVariable.time:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.time;
					}
					break;
				case TimeGetVariable.timeScale:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.timeScale;
					}
					break;
				case TimeGetVariable.timeSinceLevelLoad:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.timeSinceLevelLoad;
					}
					break;
				case TimeGetVariable.unscaledDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.unscaledDeltaTime;
					}
					break;
				case TimeGetVariable.unscaledTime:
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