using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public interface IDamageDealer
    {
        Vector2 GetDamagePositioin();

        int GetSideIndex();

        float GetDamage();

        bool CanDamage();

        bool DealDamageFromInto(IDamageDealer source, IDamageReciever target);
    }

}
