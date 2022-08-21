using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.GameBehaviors
{
    public class ItemHeart : ItemBase
    {
        [SerializeField] private float m_healingValue = 1f;

        public override void Use(CharacterBase user)
        {
            if (user.CanRecieveHeal())
            {
                user.Heal(m_healingValue);
                Destroy(gameObject);
            }
        }
    }
}
