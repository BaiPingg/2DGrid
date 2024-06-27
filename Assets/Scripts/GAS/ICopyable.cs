using UnityEngine;

namespace GAS
{
    public interface ICopyable<T>
    {
        public T Copy();
    }
}