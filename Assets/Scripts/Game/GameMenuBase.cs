using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Assets.Scripts.Game
{
    public class GameMenuBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button exitButton;

        private const float FADE_TIME = 0.5f;
        private float fadeStartTime = 0;

        protected virtual void OnEnable()
        {
            exitButton.onClick.AddListener(OnExitClick);
        }

        protected virtual void OnDisable()
        {
            exitButton.onClick.RemoveListener(OnExitClick);
        }

        public async void FadeIn()
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            fadeStartTime = Time.time;
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = (Time.time - fadeStartTime) / FADE_TIME;
                await Task.Delay(1);
            }
        }

        public void FadeOut()
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(false);
        }

        protected void OnExitClick()
        {
            GameController.ExitGame();
        }
    }
}