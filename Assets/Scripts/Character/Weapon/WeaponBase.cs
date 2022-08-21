using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public class WeaponBase : MonoBehaviour, IDamageDealer, IPausable
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
        public bool IsReloading { get { return (reloading > -m_reloadTime); } }

        public bool CanAttack { get { return (!IsReloading); } }

        public void Triggered(IDamageDealer owner)
        {
            triggered = true;
            reloading = m_delayedTime;
            this.owner = owner;
        }

        protected virtual void Attack()
        {
        }

        public Vector2 GetDamagePositioin()
        {
            return transform.position;
        }

        public float GetDamage()
        {
            return owner.GetDamage();
        }

        public int GetSideIndex()
        {
            return owner.GetSideIndex();
        }

        public bool CanDamage()
        {
            return owner.CanDamage();
        }

        public bool DealDamageFromInto(IDamageDealer source, IDamageReciever target)
        {
            return owner.DealDamageFromInto(source, target);
        }

        public void OnPause(bool pause)
        {
            enabled = !pause;
        }
    }
}
