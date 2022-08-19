using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class GameMenuPause : GameMenuBase
    {
        [SerializeField] private Button resumeButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            resumeButton.onClick.AddListener(OnResumeClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            resumeButton.onClick.RemoveListener(OnResumeClick);
        }

        private void OnResumeClick()
        {
            FadeOut();
            GameController.ResumeGame();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnResumeClick();
            }
        }
    }
}