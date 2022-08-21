using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.GameBehaviors
{
    public class ItemBase : MonoBehaviour, IItemUsable, IPausable
    {
        [SerializeField] private bool m_IsTrigger = true;
        [SerializeField] private bool m_IsCollider = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsTrigger)
            {
                CharacterBase user = collision.gameObject.GetComponent<CharacterBase>();
                if (user != null && user.AcceptUseItem(this))
                    Use(user);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_IsCollider)
            {
                CharacterBase user = collision.gameObject.GetComponent<CharacterBase>();
                if (user != null && user.AcceptUseItem(this))
                    Use(user);
            }
        }

        public virtual void Use(CharacterBase user)
        {
            throw new System.NotImplementedException();
        }


        public void OnPause(bool pause)
        {
            enabled = !pause;
        }

    }
}
