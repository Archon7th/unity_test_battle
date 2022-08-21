using Assets.Scripts.GameBehaviors;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMenu
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter m_mainPlayer;
        [SerializeField] private RectTransform m_foreground;
        [SerializeField] private RectTransform m_background;

        [Header("Effect")]
        [SerializeField] private Image m_block;
        [SerializeField] private float m_fallSpeed = 1f;
        [SerializeField] private float m_alphaSpeed = 1f;
        [SerializeField] private float m_rotationSpeed = 30f;

        [Header("Portrait")]
        [SerializeField] private Image m_portrait;
        [SerializeField] private AnimationCurve m_blinkCurve = AnimationCurve.EaseInOut(0,1,1,1);

        private float segment = 30f;

        private void OnEnable()
        {
            m_mainPlayer.OnHealthEvent.AddListener(OnHealthChanged);
        }

        private void OnDisable()
        {
            m_mainPlayer.OnHealthEvent.RemoveListener(OnHealthChanged);
        }

        private void Update()
        {
            if (m_block.enabled)
            {
                SetBlockRotation(m_block.rectTransform.eulerAngles.z - m_rotationSpeed * Time.deltaTime);
                SetBlockAlpha(m_block.color.a - m_alphaSpeed * Time.deltaTime);
                AddBlockFalling(m_fallSpeed * Time.deltaTime);
            }
            if (m_mainPlayer.IsAfterDamage || m_portrait.color.a < 0)
            {
                SetPortraitAlpha(m_mainPlayer.AfterDamage);
            }
        }

        private void OnHealthChanged(float value)
        {
            float factor = m_mainPlayer.Health / m_mainPlayer.HealthMax;
            float width = m_background.sizeDelta.x;
            m_foreground.sizeDelta = new Vector2(factor * width, m_foreground.sizeDelta.y);

            if (value < 0)
            {
                if (m_block.color.a > 0.5)
                    segment += Mathf.Abs(value) / m_mainPlayer.HealthMax;
                else
                    segment = Mathf.Abs(value) / m_mainPlayer.HealthMax;
                m_block.rectTransform.anchoredPosition = m_foreground.anchoredPosition + new Vector2(m_foreground.sizeDelta.x, 0);
                m_block.rectTransform.sizeDelta = new Vector2(segment * width, m_foreground.sizeDelta.y);
                SetBlockRotation(0);
                SetBlockAlpha(1);
            }
            else
            {
                SetBlockRotation(0);
                SetBlockAlpha(0);
            }
        }


        private void AddBlockFalling(float value)
        {
            m_block.rectTransform.anchoredPosition -= new Vector2(0, value);
        }

        private void SetBlockRotation(float value)
        {
            Vector3 rotation = m_block.rectTransform.eulerAngles;
            rotation.z = value;
            m_block.rectTransform.eulerAngles = rotation;
        }

        private void SetBlockAlpha(float value)
        {
            Color color = m_block.color;
            color.a = value;
            m_block.color = color;
            if (value > 0)
                m_block.enabled = true;
            else
                m_block.enabled = false;
        }

        private void SetPortraitAlpha(float value)
        {
            Color color = m_portrait.color;
            color.a = m_blinkCurve.Evaluate(value);
            m_portrait.color = color;
        }
    }
}