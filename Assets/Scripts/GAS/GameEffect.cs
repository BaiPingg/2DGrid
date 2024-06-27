using System;
using UnityEngine;

namespace GAS
{
    [Serializable]
    public enum GameplayEffectDurationType
    {
        Instant, //瞬间
        Infinite, //永久
        Duration, //持续时间
    }

    public class GameEffect : ScriptableObject, ICopyable<GameEffect>
    {
        public GameplayEffectDurationType durationType;

        public GameEffect Copy()
        {
            return Instantiate(this);
        }
    }
}