using System.Collections.Generic;
using UnityEngine;

public class GameEvent<T> : ScriptableObject
{
    public T data;
    private List<GameEventListener<T>> _listeners = new();

    public void Rise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i++)
        {
            _listeners[i].OnEventRaised(data);
        }
    }

    public void RegisterListener(GameEventListener<T> listener)
    {
        _listeners.Add(listener);
    }

    public void UnRegisterListener(GameEventListener<T> listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}