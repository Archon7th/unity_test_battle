using UnityEngine;

namespace Assets.Scripts.Game
{
    public interface IDamageDealer
    {
        bool DealDamageTo(IDamageReciever target);
    }

}
