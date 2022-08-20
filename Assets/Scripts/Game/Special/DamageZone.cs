using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Game
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
            if (m_IsTrigger)
            {
                IDamageReciever reciever = collision.gameObject.GetComponent<IDamageReciever>();
                if (reciever != null && reciever.AcceptDamageFrom(DamageDealer.Get()))
                    DamageDealer.Get().DealDamageTo(reciever);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_IsCollider)
            {
                IDamageReciever reciever = collision.gameObject.GetComponent<IDamageReciever>();
                if (reciever != null && reciever.AcceptDamageFrom(DamageDealer.Get()))
                    DamageDealer.Get().DealDamageTo(reciever);
            }
        }

        public bool DealDamageTo(IDamageReciever target)
        {
            return DamageDealer.Get().DealDamageTo(target);
        }

        public void OnPause(bool pause)
        {
            enabled = !pause;
        }

    }
}
