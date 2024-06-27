using NaughtyAttributes;
using UnityEngine;

namespace GAS
{
    [CreateAssetMenu(menuName = "GAS/Attribute")]
    public class Attribute : ScriptableObject, ICopyable<Attribute>
    {
        public delegate void ChangeValue(float preValue, float currentValue);

        public ChangeValue onChangeValue;
        public string attributeName;
        public float defaultValue;
        [ReadOnly, SerializeField] private float _currentValue;
        public float CurrentValue => _currentValue;

        protected GameAbilitySystemComponent absComponent;

        public void FireEvent(float preValue, float currentValue)
        {
            onChangeValue?.Invoke(preValue, currentValue);
        }

        [Button("InitTest")]
        public virtual void Init(GameAbilitySystemComponent absComponent)
        {
            this.absComponent = absComponent;
            _currentValue = defaultValue;
            Debug.Log($"{attributeName} init");
        }

        [Button("Clear")]
        public void Clear()
        {
            _currentValue = 0;
        }

        public Attribute Copy()
        {
            return Instantiate(this);
        }
    }
}