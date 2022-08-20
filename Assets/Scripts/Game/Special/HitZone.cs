using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Game
{
    [System.Serializable]
    public class IDamageRecieverObject : InterfaceObject<IDamageReciever> {}

    public class HitZone : MonoBehaviour, IDamageReciever, IPausable
    {
        [SerializeField] private bool m_IsTrigger = true;
        [SerializeField] private bool m_IsCollider = false;

        public IDamageRecieverObject DamageReciever;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsTrigger)
            {
                IDamageDealer damageDealer = collision.gameObject.GetComponent<IDamageDealer>();
                if (damageDealer != null)
                    damageDealer.DealDamageTo(DamageReciever.Get());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_IsCollider)
            {
                IDamageDealer damageDealer = collision.gameObject.GetComponent<IDamageDealer>();
                if (damageDealer != null)
                    damageDealer.DealDamageTo(DamageReciever.Get());
            }
        }

        public bool AcceptDamageFrom(IDamageDealer source)
        {
            return DamageReciever.Get().AcceptDamageFrom(source);
        }

        public void DirectDamage(float damage, Vector2 impulse)
        {
            DamageReciever.Get().DirectDamage(damage, impulse);
        }

        public void OnPause(bool pause)
        {
            enabled = !pause;
        }
    }
}
