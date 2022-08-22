using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameBehaviors
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_group;
        [SerializeField] private RectTransform m_transform;
        [SerializeField] private RectTransform m_foreground;
        [SerializeField] private RectTransform m_background;

        private EnemyHealthBarController controller;
        private CharacterBase owner;

        private void OnEnable()
        {
            if (owner)
            {
                owner.OnHealthEvent.AddListener(OnHealthChanged);
                owner.OnDeathEvent.AddListener(OnDeath);
            }
        }

        private void OnDisable()
        {
            if (owner)
            {
                owner.OnHealthEvent.RemoveListener(OnHealthChanged);
                owner.OnDeathEvent.RemoveListener(OnDeath);
            }
        }

        private void Update()
        {
            if (m_group.alpha > 0)
            {
                m_group.alpha -= Time.deltaTime;
                Vector3 viewport = controller.GetWorldToViewport(owner.transform.position);
                m_transform.anchoredPosition = controller.GetViewportToOffset(viewport);
            }
        }

        public void Initialize(EnemyHealthBarController controller, CharacterBase owner)
        {
            this.controller = controller;
            this.owner = owner;
            m_group.alpha = 0;
            enabled = true;
        }

        private void OnHealthChanged(float value)
        {
            SetHealth(owner.Health / owner.HealthMax);
            m_group.alpha = 1f;
        }

        private void SetHealth(float factor)
        {
            float width = m_background.sizeDelta.x;
            m_foreground.sizeDelta = new Vector2(factor * width, m_foreground.sizeDelta.y);
        }

        private void OnDeath()
        {
            controller.Unregister(owner, this);
            Destroy(gameObject, 1f);
        }
    }
}
