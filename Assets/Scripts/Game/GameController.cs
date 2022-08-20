using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.GameBehaviors;

namespace Assets.Scripts.GameMenu
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameMenuBase m_pauseMenu;
        [SerializeField] private GameMenuBase m_deathMenu;
        [SerializeField] private CharacterBase m_mainPlayer;

        private static GameController instance;
        
        private void Awake()
        {
            Physics2D.IgnoreLayerCollision(8,8);
        }

        private void OnEnable()
        {
            instance = this;
            if (m_mainPlayer != null)
                m_mainPlayer.OnDeathEvent.AddListener(ShowDeathMenu);
        }

        private void OnDisable()
        {
            if (m_mainPlayer != null)
                m_mainPlayer.OnDeathEvent.RemoveListener(ShowDeathMenu);
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

        public static void ShowDeathMenu()
        {
            if (instance)
            {
                instance.m_deathMenu.FadeIn();
            }
        }

        public static void PauseGame()
        {
            if (instance)
            {
                PauseAll(true);
                instance.m_pauseMenu.FadeIn();
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