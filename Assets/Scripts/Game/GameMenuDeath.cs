using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class GameMenuDeath : GameMenuBase
    {
        [SerializeField] private Button m_restartButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_restartButton.onClick.AddListener(OnReplayClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_restartButton.onClick.RemoveListener(OnReplayClick);
        }

        private void OnReplayClick()
        {
            FadeOut();
            GameController.RestartGame();
        }

        private void Update()
        {
            if (m_canvasGroup.alpha >= 1 && Input.GetKeyUp(KeyCode.Escape))
            {
                OnExitClick();
            }
        }
    }
}