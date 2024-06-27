using GAS;
using UnityEngine;

public class BuildStateBase : ScriptableObject, Istate, ICopyable<BuildStateBase>
{
    protected BuildService buildService;

    public virtual void Enter(IStateMachine machine)
    {
        buildService = machine as BuildService;
        Debug.Log($"{GetType().Name} enter");
    }

    public virtual void Tick(float delta)
    {
        Debug.Log($"{GetType().Name} Tick");
    }

    public virtual void FixedTick()
    {
    }

    public virtual void Leave()
    {
        Debug.Log($"{GetType().Name} Leave");
    }

    public BuildStateBase Copy()
    {
        return Instantiate(this);
    }
}