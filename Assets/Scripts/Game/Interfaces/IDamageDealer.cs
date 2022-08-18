using UnityEngine;

namespace Assets.Scripts.Game
{
    public interface IDamageDealer
    {
        bool DealDamage(IDamageReciever target);
    }

}
