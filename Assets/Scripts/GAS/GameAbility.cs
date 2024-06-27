using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    [CreateAssetMenu(menuName = "GAS/GameAbility")]
    public class GameAbility : ScriptableObject, ICopyable<GameAbility>
    {
        public string abilityName = "GameAbility";
        public string abilityDescription;
        public GameEffect cooldown;
        public GameEffect cost;
        public List<GameEffect> effects = new List<GameEffect>();
        public GameAbilitySystemComponent source, target, owner;

        public GameAbility Copy()
        {
            return Instantiate(this);
        }
    }
}