using UnityEngine;

namespace Fungus
{
	/// <summary>
	/// Assign to a value in Unity's Time.
	/// </summary>
	[CommandInfo("Time",
		"Set",
		"Assign to a value in Unity's Time.")]
	[AddComponentMenu("")]
	public class TimeSet : Command
	{
		public enum TimeSetVariable
		{
			captureFramerate,
			//deltaTime,
			fixedDeltaTime,
			//fixedTime,
			//fixedUnscaledDeltaTime,
			//fixedUnscaledTime,
			//frameCount,
			//inFixedTimeStep,
			maximumDeltaTime,
			maximumParticleDeltaTime,
			//realtimeSinceStartup,
			//smoothDeltaTime,
			//time,
			timeScale,
			//timeSinceLevelLoad,
			//unscaledDeltaTime,
			//unscaledTime,
		}


		[Tooltip("Which part of Time do you want to step.")]
		[SerializeField]
		protected TimeSetVariable variable = TimeSetVariable.timeScale;

		[Tooltip("Varaible to store the value in.")]
		[SerializeField]
		[VariableProperty(typeof(IntegerVariable), 
			typeof(FloatVariable))]
		protected Variable takeValueFrom;

		public override void OnEnter()
		{
			var asIntVar = takeValueFrom as IntegerVariable;
			var asFloatVar = takeValueFrom as FloatVariable;

			switch (variable)
			{
				case TimeSetVariable.captureFramerate:
					if(asIntVar != null)
					{
						asIntVar.Value = Time.captureFramerate;
					}
					break;
				case TimeSetVariable.fixedDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.fixedDeltaTime;
					}
					break;
				case TimeSetVariable.maximumDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.maximumDeltaTime;
					}
					break;
				case TimeSetVariable.maximumParticleDeltaTime:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.maximumParticleDeltaTime;
					}
					break;
				case TimeSetVariable.timeScale:
					if(asFloatVar != null)
					{
						asFloatVar.Value = Time.timeScale;
					}
					break;
				default:
					break;
			}

			Continue();
		}

		public override string GetSummary()
		{
			if(takeValueFrom == null)
				return "No store variable set.";

			return variable.ToString();
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

	}
}