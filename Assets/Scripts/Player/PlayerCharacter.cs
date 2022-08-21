using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameBehaviors
{
	public class PlayerCharacter : CharacterController2D
	{
		public static PlayerCharacter Instance { get; protected set; }

		public UnityEvent OnScoredKillEvent = new UnityEvent();

		public PlayerCharacter() : base()
		{
			Instance = this;
		}

		protected override void KillsTarget(IDamageReciever target)
		{
			if (target.GetSideIndex() != GetSideIndex())
				OnScoredKillEvent.Invoke();
		}
	}
}