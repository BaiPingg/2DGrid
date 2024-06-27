using System;

public interface IStateMachine
{
    void SwitchState(Type state);
}