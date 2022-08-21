using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
    public interface IItemUsable
    {
        void Use(CharacterBase user);
    }
}
