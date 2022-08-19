using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnPlayClick);
            exitButton.onClick.AddListener(OnExitClick);
        }


        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlayClick);
            exitButton.onClick.RemoveListener(OnExitClick);
        }

        private void OnPlayClick()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        private void OnExitClick()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
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