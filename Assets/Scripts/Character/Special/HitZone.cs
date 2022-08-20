using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.GameBehaviors
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
            IDamageReciever reciever = DamageReciever.Get();
            if (m_IsTrigger && reciever != null)
            {
                IDamageDealer dealer = collision.gameObject.GetComponent<IDamageDealer>();
                if (dealer != null && dealer.CanDamage() && reciever.AcceptDamageFrom(dealer))
                    dealer.DealDamageFromInto(dealer, reciever);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IDamageReciever reciever = DamageReciever.Get();
            if (m_IsCollider && reciever != null)
            {
                IDamageDealer dealer = collision.gameObject.GetComponent<IDamageDealer>();
                if (dealer != null && dealer.CanDamage() && reciever.AcceptDamageFrom(dealer))
                    dealer.DealDamageFromInto(dealer, reciever);
            }
        }

        public Vector2 GetDamagePositioin()
        {
            return DamageReciever.Get().GetDamagePositioin();
        }

        public int GetSideIndex()
        {
            return DamageReciever.Get().GetSideIndex();
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
