using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "BuildSystem/BuildPlacementState")]
public class BuildPlacementState : BuildStateBase
{
    public override void Enter(IStateMachine machine)
    {
        base.Enter(machine);

        var grid = SL.Get<GridService>();
        if (SL.Get<InputService>().TryGetMouseWorldPos(out Vector3 pos))
        {
            var cell = grid.Grid.GetCellPosition(pos);
            var sss = grid.Grid.GetWorldPosition(cell.x, cell.y);
            buildService.Context.currentSelect.SetPosition(sss, false);
            buildService.Context.currentSelect.SetNormalView();
            buildService.SwitchState(typeof(BuildSelectState));
        }
    }

    public override void Leave()
    {
        base.Leave();
        buildService.Context.currentSelect = null;
        buildService.Context.currentBuildItem = null;
    }
}