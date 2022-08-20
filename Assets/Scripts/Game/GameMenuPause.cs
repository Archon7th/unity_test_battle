using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMenu
{
    public class GameMenuPause : GameMenuBase
    {
        [SerializeField] private Button m_resumeButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_resumeButton.onClick.AddListener(OnResumeClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_resumeButton.onClick.RemoveListener(OnResumeClick);
        }

        private void OnResumeClick()
        {
            FadeOut();
            GameController.ResumeGame();
        }

        private void Update()
        {
            if (m_canvasGroup.alpha >= 1 && Input.GetKeyUp(KeyCode.Escape))
            {
                OnResumeClick();
            }
        }
    }
}