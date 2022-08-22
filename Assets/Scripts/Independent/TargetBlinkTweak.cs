using Assets.Scripts.GameBehaviors;
using UnityEngine;

public class TargetBlinkTweak : MonoBehaviour, IPausable
{

    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private AnimationCurve m_birthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve m_blinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] [Min(float.Epsilon)] private float m_birthTime = 1f;
    [SerializeField] [Min(float.Epsilon)] private float m_delayTime = 1f;
    [SerializeField] [Min(float.Epsilon)] private float m_blinkTime = 1f;

    private float lifeTime;

    #region UNITY

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime <= m_birthTime)
        {
            Color col = m_renderer.color;
            col.a = m_birthCurve.Evaluate(lifeTime / m_birthTime);
            m_renderer.color = col;
        }
        else if (lifeTime > m_delayTime)
        {
            Color col = m_renderer.color;
            col.a = m_blinkCurve.Evaluate(((lifeTime - m_delayTime) % m_blinkTime) / m_blinkTime);
            m_renderer.color = col;
        }
    }

    #endregion

    public void OnPause(bool pause)
    {
        enabled = !pause;
    }
}
