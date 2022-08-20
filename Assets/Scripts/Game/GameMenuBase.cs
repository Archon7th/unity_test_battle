using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Assets.Scripts.GameMenu
{
    public class GameMenuBase : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup m_canvasGroup;
        [SerializeField] protected Button m_exitButton;

        private const float FADE_TIME = 0.5f;
        private float fadeStartTime = 0;

        protected virtual void OnEnable()
        {
            m_exitButton.onClick.AddListener(OnExitClick);
        }

        protected virtual void OnDisable()
        {
            m_exitButton.onClick.RemoveListener(OnExitClick);
        }

        public async void FadeIn()
        {
            m_canvasGroup.gameObject.SetActive(true);
            m_canvasGroup.alpha = 0;
            fadeStartTime = Time.time;
            while (m_canvasGroup.alpha < 1)
            {
                m_canvasGroup.alpha = (Time.time - fadeStartTime) / FADE_TIME;
                await Task.Delay(1);
            }
        }

        public void FadeOut()
        {
            m_canvasGroup.alpha = 0;
            m_canvasGroup.gameObject.SetActive(false);
        }

        protected void OnExitClick()
        {
            GameController.ExitGame();
        }
    }
}