using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public interface IItemUsable
    {
        bool Use(IItemUser user);
    }
}
