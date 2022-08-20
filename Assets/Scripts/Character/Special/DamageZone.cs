using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.GameBehaviors
{
    [System.Serializable]
    public class IDamageDelaerObject : InterfaceObject<IDamageDealer> {}

    public class DamageZone : MonoBehaviour, IDamageDealer, IPausable
    {
        [SerializeField] private bool m_IsTrigger = true;
        [SerializeField] private bool m_IsCollider = false;

        public IDamageDelaerObject DamageDealer;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageDealer dealer = DamageDealer.Get();
            if (m_IsTrigger && dealer != null && dealer.CanDamage())
            {
                IDamageReciever reciever = collision.gameObject.GetComponent<IDamageReciever>();
                if (reciever != null && reciever.AcceptDamageFrom(dealer))
                    dealer.DealDamageFromInto(dealer, reciever);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IDamageDealer dealer = DamageDealer.Get();
            if (m_IsCollider && dealer != null && dealer.CanDamage())
            {
                IDamageReciever reciever = collision.gameObject.GetComponent<IDamageReciever>();
                if (reciever != null && reciever.AcceptDamageFrom(dealer))
                    dealer.DealDamageFromInto(dealer, reciever);
            }
        }

        public Vector2 GetDamagePositioin()
        {
            return DamageDealer.Get().GetDamagePositioin();
        }

        public float GetDamage()
        {
            return DamageDealer.Get().GetDamage();
        }

        public int GetSideIndex()
        {
            return DamageDealer.Get().GetSideIndex();
        }

        public bool CanDamage()
        {
            return DamageDealer.Get().CanDamage();
        }

        public bool DealDamageFromInto(IDamageDealer source, IDamageReciever target)
        {
            return DamageDealer.Get().DealDamageFromInto(source, target);
        }

        public void OnPause(bool pause)
        {
            enabled = !pause;
        }


    }
}
