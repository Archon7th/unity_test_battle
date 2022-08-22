using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.GameBehaviors
{
    public class WeaponMele : WeaponBase
    {
        [Header("Contact Setup")]
        [SerializeField] private LayerMask m_ContactLayerMask;
        [SerializeField] private float m_ContactRadius = .5f;
        [SerializeField] private bool m_OneDamageReciever = true;

        protected override void Attack()
        {
            if (!CanDamage())
                return;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_ContactRadius, m_ContactLayerMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                IDamageReciever reciever = colliders[i].GetComponentInParent<IDamageReciever>();
                if (reciever != null)
                {
                    if (reciever.AcceptDamageFrom(owner))
                    {
                        owner.DealDamageFromInto(this, reciever);
                        if (m_OneDamageReciever)
                            break;
                    }
                }
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.Contains(gameObject) || Selection.Contains(transform.root.gameObject))
            {
                Handles.color = Color.yellow;
                Handles.DrawWireDisc(transform.position, Vector3.back, m_ContactRadius);
            }
        }
#endif
    }
}