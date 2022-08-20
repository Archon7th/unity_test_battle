using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
	public class PlayerCharacter : CharacterController2D
	{
		public static PlayerCharacter Instance { get; protected set; }
		public PlayerCharacter() : base()
		{
			Instance = this;
		}
	}
}