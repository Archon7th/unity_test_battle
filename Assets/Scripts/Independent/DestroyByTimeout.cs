using Assets.Scripts.GameBehaviors;
using UnityEngine;

public class DestroyByTimeout : MonoBehaviour, IPausable
{

    [SerializeField] private float m_timeout = 1f;

    private float lifeTime;

    #region UNITY

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > m_timeout)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public void OnPause(bool pause)
    {
        enabled = !pause;
    }
}
