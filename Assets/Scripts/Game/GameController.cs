using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameMenuBase pauseMenu;
        [SerializeField] private GameMenuBase deathMenu;

        private static GameController instance;

        private void Awake()
        {
        }

        private void OnEnable()
        {
            instance = this;
        }

        private static void PauseAll(bool pause)
        {
            MonoBehaviour[] monoScripts = (MonoBehaviour[])FindObjectsOfType(typeof(MonoBehaviour));
            foreach (var el in monoScripts)
            {
                if (el is IPausable)
                {
                    (el as IPausable).OnPause(pause);
                }
            }
        }

        public static void PauseGame()
        {
            if (instance)
            {
                instance.pauseMenu.FadeIn();
                PauseAll(true);
            }
        }

        public static void ResumeGame()
        {
            if (instance)
            {
                PauseAll(false);
            }
        }

        public static void RestartGame()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }


        public static void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}