using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardInput : MonoBehaviour, IPausable
{
	[SerializeField] private CharacterController2D m_controller;

	private bool jumpLocked = false;

	private void FixedUpdate ()
	{
		if (!m_controller.IsAlive() || !enabled)
			return;

		m_controller.WantMove(Input.GetAxisRaw("Horizontal") );

		if (Input.GetButtonDown("Jump"))
            m_controller.WantJump();

		if (Input.GetButtonDown("Fire1"))
			m_controller.WantAttack();


		if (Input.GetAxisRaw("Vertical") < 0)
			m_controller.WantRoll();
		else if (!jumpLocked)
		{
			if (Input.GetAxisRaw("Vertical") > 0)
			{
				jumpLocked = true;
				m_controller.WantJump();
			}
		}
		else
			jumpLocked = false;

		if (Input.GetKeyUp(KeyCode.Escape))
		{
			GameController.PauseGame();
		}
	}

	public void OnPause(bool pause)
	{
		enabled = !pause;
	}
}
