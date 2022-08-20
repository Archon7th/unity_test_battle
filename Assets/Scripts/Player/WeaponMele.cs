using System;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game;
using UnityEditor;

public class WeaponMele : WeaponBase
{
    [Header("Contact Setup")]
    [SerializeField] private LayerMask m_ContactLayerMask;
    [SerializeField] private float m_ContactRadius = .5f;

    protected override void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_ContactRadius, m_ContactLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            IDamageReciever reciever = colliders[i].GetComponent<IDamageReciever>();
            if (reciever != null)
            {
                if (reciever.AcceptDamageFrom(owner))
                {
                    owner.DealDamageTo(reciever);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Selection.Contains(gameObject) || Selection.Contains(transform.root.gameObject))
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, Vector3.back, m_ContactRadius);
        }
    }
}
