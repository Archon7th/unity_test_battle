using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class GameMenuDeath : GameMenuBase
    {
        [SerializeField] private Button restartButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            restartButton.onClick.AddListener(OnReplayClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            restartButton.onClick.RemoveListener(OnReplayClick);
        }

        private void OnReplayClick()
        {
            FadeOut();
            GameController.RestartGame();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnExitClick();
            }
        }
    }
}