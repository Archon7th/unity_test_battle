using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public class EnemiesSpawner : MonoBehaviour, IPausable, IWaitPlayerDeath
    {
        [SerializeField] private EnemyCharacter[] m_enemyVariants;
        [SerializeField] private Transform[] m_spawnPoints;

        [SerializeField] private float m_spawnPeriod = 3;
        [SerializeField] private float m_healthInit = 1;
        [SerializeField] private float m_healthGrow = 1;
        [SerializeField] private float m_healthGrowTime = 10;

        private int spawnIndex;
        private float lifetime;



        void Start()
        {
            lifetime = 0;
            spawnIndex = (m_spawnPoints.Length > 0) ? Random.Range(0, m_enemyVariants.Length) : 0;
        }

        void Update()
        {
            float prevLifetime = lifetime;
            lifetime += Time.deltaTime;
            if (m_spawnPeriod > 0 && prevLifetime % m_spawnPeriod > lifetime % m_spawnPeriod)
            {
                if (m_enemyVariants.Length > 0 && m_spawnPoints.Length > 0)
                {
                    EnemyCharacter enemyPrefab = m_enemyVariants[Random.Range(0, m_enemyVariants.Length)];
                    if (enemyPrefab != null)
                    {
                        float health = m_healthInit + ((m_healthGrowTime > 0) ? m_healthGrow * Mathf.Floor(lifetime / m_healthGrowTime) : 0);
                        Transform spawnPoint = m_spawnPoints[spawnIndex % m_spawnPoints.Length];
                        EnemyCharacter enemy = Instantiate<EnemyCharacter>(enemyPrefab, spawnPoint.position, Quaternion.identity);
                        enemy.SehHealthOnSpawn(health);
                        spawnIndex++;
                    }
                }

            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.Contains(gameObject))
            {
                for (int i = 0; i < m_spawnPoints.Length; i++)
                {
                    Handles.color = Color.yellow;
                    Handles.DrawWireDisc(m_spawnPoints[i].position, Vector3.back, 0.5f);
                }
            }
        }
#endif

        public void OnPause(bool pause)
        {
            enabled = !pause;
        }

        public void OnPlayerDeath(bool death)
        {
            enabled = !death;
        }
    }
}