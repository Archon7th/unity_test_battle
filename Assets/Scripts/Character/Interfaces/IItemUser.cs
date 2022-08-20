using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public interface IItemUser
    {
        bool AcceptUse(IItemUsable item);
    }
}
