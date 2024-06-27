using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    [CreateAssetMenu(menuName = "GAS/GamePlayTag")]
    public class GamePlayTag : ScriptableObject, ICopyable<GamePlayTag>
    {
        public string description;

        public GamePlayTag Copy()
        {
            return Instantiate(this);
        }
    }
}