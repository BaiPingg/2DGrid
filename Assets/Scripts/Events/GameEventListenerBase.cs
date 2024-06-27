using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerBase : MonoBehaviour
{
    public GameEventBase gameEvent;
    public UnityEvent unityAction;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnRegisterListener(this);
    }

    public void OnEventRaised()
    {
        unityAction?.Invoke();
    }
}