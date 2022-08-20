using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button m_playButton;
        [SerializeField] private Button m_exitButton;

        private void OnEnable()
        {
            m_playButton.onClick.AddListener(OnPlayClick);
            m_exitButton.onClick.AddListener(OnExitClick);
        }


        private void OnDisable()
        {
            m_playButton.onClick.RemoveListener(OnPlayClick);
            m_exitButton.onClick.RemoveListener(OnExitClick);
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