using System;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game;

public class WeaponBase : MonoBehaviour, IPausable
{
    [SerializeField] protected float m_delayedTime = 0.1f;
    [SerializeField] protected float m_reloadTime = 0.5f;

    protected bool triggered;
    protected float reloading = 0;
    protected IDamageDealer owner;

    protected virtual void FixedUpdate()
    {
        reloading -= Time.fixedDeltaTime;
        if (triggered && reloading <= 0)
        {
            triggered = false;
            Attack();
        }
    }

    public bool CanAttack { get { return (reloading <= -m_reloadTime); } }

    public void Triggered(IDamageDealer owner)
    {
        triggered = true;
        reloading = m_delayedTime;
        this.owner = owner;
    }

    protected virtual void Attack()
    {
    }

    public void OnPause(bool pause)
    {
        enabled = !pause;
    }
}
