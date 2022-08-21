using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public interface IDamageReciever
    {
        Vector2 GetDamagePositioin();

        int GetSideIndex();

        bool IsAlive();

        bool AcceptDamageFrom(IDamageDealer source);

        void DirectDamage(float damage, Vector2 impulse);

        void KilledByDealer(IDamageDealer source);
    }
}
