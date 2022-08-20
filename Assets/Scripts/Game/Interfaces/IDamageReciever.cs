using UnityEngine;

namespace Assets.Scripts.Game
{
    public interface IDamageReciever
    {
        bool AcceptDamageFrom(IDamageDealer source);

        void DirectDamage(float damage, Vector2 impulse);
    }
}
