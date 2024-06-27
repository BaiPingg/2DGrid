using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEventBase : ScriptableObject
{
    private List<GameEventListenerBase> _listeners = new();

    public void Rise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListenerBase listener)
    {
        _listeners.Add(listener);
    }

    public void UnRegisterListener(GameEventListenerBase listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}