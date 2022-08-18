using UnityEngine;

namespace Assets.Scripts.Game
{
    public interface IDamageReciever
    {
        int GetSide();
        
        bool AcceptDamage(IDamageDealer source);

        void DirectDamage(float damage);
    }
}
