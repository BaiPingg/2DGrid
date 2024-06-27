using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

namespace GAS
{
    public class GameAbilitySystemComponent : MonoBehaviour

    {
        [SerializeField] private Attribute[] _attributes;
        private Dictionary<string, Attribute> _realAttributes = new();
        private Dictionary<string, GameAbility> _possessAbilities = new();
        private Dictionary<string, GameAbility> _activeedAbilities = new();
        [SerializeField] private GameEventBase _eventBase;

        [Button("FireEvent")]
        public void FireEvent()
        {
            _eventBase.Rise();
        }

        [Button("InitAttributes")]
        public void InitAttributes()
        {
            if (_attributes != null && _attributes.Length != 0)
            {
                for (int i = 0; i < _attributes.Length; i++)
                {
                    var att = _attributes[i].Copy();
                    _realAttributes.Add(att.attributeName, att);
                    att.Init(this);
                }
            }
        }

        public GameAbility testAdd;

        [Button("testAdd")]
        public void TestAdd()
        {
            AddAbility(testAdd);
            Debug.Log(testAdd.abilityName + "added");
        }

        public void AddAbility(GameAbility ability)
        {
            if (!_possessAbilities.ContainsKey(ability.abilityName))
            {
                var aa = ability.Copy();
                _possessAbilities.Add(aa.abilityName, aa);
            }
        }

        public void RemoveAbility(GameAbility ability)
        {
            if (_possessAbilities.ContainsKey(ability.abilityName))
            {
                _possessAbilities.Remove(ability.abilityName);
            }
        }

        public bool TryActiveAbility(GameAbility ability, GameAbilitySystemComponent target)
        {
            if (_possessAbilities.ContainsKey(ability.abilityName))
            {
                //     +_activeedAbilities.Add(_possessAbilities[])
            }

            // _activeedAbilities.Add();
            return true;
        }
    }
}