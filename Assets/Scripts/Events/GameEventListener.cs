using System;
using UnityEngine;
using UnityEngine.Events;


public class GameEventListener<T> : MonoBehaviour
{
    public GameEvent<T> gameEvent;
    public UnityAction<T> unityAction;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnRegisterListener(this);
    }

    public void OnEventRaised(T data)
    {
        unityAction?.Invoke(data);
    }
}