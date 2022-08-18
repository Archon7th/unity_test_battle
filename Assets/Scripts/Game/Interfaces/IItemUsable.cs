using UnityEngine;

namespace Assets.Scripts.Game
{
    public interface IItemUsable
    {
        bool Use(IItemUser user);
    }
}
