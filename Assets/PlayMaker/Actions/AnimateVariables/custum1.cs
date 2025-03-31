using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.AnimateVariables)]
	public class custum1 : FsmStateAction
	{

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			Finish();
		}


	}

}
