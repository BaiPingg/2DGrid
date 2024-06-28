using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class BuildContext
{
    public BuildingViewBase currentSelect;
    public BuildItem currentBuildItem;
}

public class BuildService : ServiceBase, IStateMachine
{
    [SerializeField, Required] private GridService _gridService;

    public List<BuildItem> buildItems = new List<BuildItem>();
    public List<BuildStateBase> buildStates = new List<BuildStateBase>();
    [ReadOnly] private BuildStateBase _previousState;
    [ReadOnly] private BuildStateBase _currentState;

    [ReadOnly] public BuildContext Context;

    public override void Init()
    {
        base.Init();
        Context = new BuildContext();
        for (int i = 0; i < buildItems.Count; i++)
        {
            buildItems[i] = buildItems[i].Copy();
        }

        for (int i = 0; i < buildStates.Count; i++)
        {
            buildStates[i] = buildStates[i].Copy();
        }


        _currentState = buildStates[0];
        _currentState.Enter(this);
    }


    public void SwitchState(Type state)
    {
        var newState = buildStates.Find((stateee => stateee.GetType() == state));
        if (newState != null)
        {
            if (_currentState != null)
            {
                _currentState.Leave();
                _previousState = _currentState;
            }

            _currentState = newState;
            _currentState.Enter(this);
        }
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.Tick(Time.deltaTime);
        }

        //if (InputSystem.)
        {
        }
    }
}