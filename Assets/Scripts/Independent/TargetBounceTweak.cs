using Assets.Scripts.GameBehaviors;
using UnityEngine;

public class TargetBounceTweak : MonoBehaviour, IPausable
{

    [SerializeField] private Transform m_transform;
    [SerializeField] private AnimationCurve m_curve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField][Min(float.Epsilon)] private float m_period = 1f;

    private float lifeTime;

    #region UNITY
    private void Awake()
    {
        if (m_transform != null)
        {
            Vector3 pos = m_transform.localPosition;
            pos.y = m_curve.Evaluate(0);
            m_transform.localPosition = pos;
        }

    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (m_transform != null) {
            Vector3 pos = m_transform.localPosition;
            pos.y = m_curve.Evaluate((lifeTime % m_period) / m_period);
            m_transform.localPosition = pos;
        }
        
    }

    #endregion

    public void OnPause(bool pause)
    {
        enabled = !pause;
    }
}
