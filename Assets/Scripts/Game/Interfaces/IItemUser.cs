using UnityEngine;

namespace Assets.Scripts.Game
{
    public interface IItemUser
    {
        bool AcceptUse(IItemUsable item);
    }
}
