using Assets.Scripts.GameBehaviors;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMenu
{
    public class ScoreBar : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter m_mainPlayer;

        [Header("Scores")]
        [SerializeField] private Text m_score;
        [SerializeField] private string m_scorePrefix = "Scores: ";

        private int score = 0;

        private void OnEnable()
        {
            m_mainPlayer.OnScoredKillEvent.AddListener(OnScoredKill);
        }

        private void OnDisable()
        {
            m_mainPlayer.OnScoredKillEvent.RemoveListener(OnScoredKill);
        }

        private void OnScoredKill()
        {
            score++;
            m_score.text = m_scorePrefix + score;
        }

    }
}