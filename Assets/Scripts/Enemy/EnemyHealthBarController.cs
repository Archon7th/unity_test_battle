using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public class EnemyHealthBarController : MonoBehaviour
    {
        public static EnemyHealthBarController Instance { get; protected set; }

        [SerializeField] private Camera m_targetCamera;
        [SerializeField] private EnemyHealthBar m_healthBarPrefab;
        [SerializeField] private RectTransform m_rectTransform;

        private readonly List<CharacterBase> enemies = new List<CharacterBase>();
        private readonly List<EnemyHealthBar> healthBars = new List<EnemyHealthBar>();

        public EnemyHealthBarController() : base()
        {
            Instance = this;
        }

        public void Register(CharacterBase enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
                EnemyHealthBar healthBar = Instantiate<EnemyHealthBar>(m_healthBarPrefab, transform);
                healthBar.Initialize(this, enemy);
                healthBars.Add(healthBar);
            }
        }

        public void Unregister(CharacterBase enemy, EnemyHealthBar healthBar)
        {
            enemies.Remove(enemy);
            healthBars.Remove(healthBar);
        }

        public Vector3 GetWorldToViewport(Vector3 position)
        {
            return m_targetCamera.WorldToViewportPoint(position);
        }

        public Vector2 GetViewportToOffset(Vector3 viewport)
        {
            return new Vector2(m_rectTransform.sizeDelta.x * viewport.x, m_rectTransform.sizeDelta.y * viewport.y);
        }
        
    }
}
