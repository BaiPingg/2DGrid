using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "BuildSystem/BuildPlacementState")]
public class BuildPlacementState : BuildStateBase
{
    public override void Enter(IStateMachine machine)
    {
        base.Enter(machine);
        var _buildItem = buildService.currentBuildItem;

        var _preView = Instantiate(_buildItem.prefab);
        var grid = SL.Get<GridService>();
        if (SL.Get<InputService>().TryGetMouseWorldPos(out Vector3 pos))
        {
            _preView.gameObject.SetActive(true);
            var cell = grid.Grid.GetCellPosition(pos);
            var sss = grid.Grid.GetWorldPosition(cell.x, cell.y);
            _preView.gameObject.transform.position = sss;
            for (int i = _buildItem.placeX.x; i < _buildItem.placeX.y; i++)
            {
                for (int j = _buildItem.placeY.x; j < _buildItem.placeY.y; j++)
                {
                    var target = cell + new Vector2Int(i, j);
                    grid.Grid.GetValue(target.x, target.y).Placed = true;
                }
            }

            _preView.transform.DOScaleX(.9f, .2f);
            _preView.transform.DOScaleY(1.5f, .2f).onComplete += (() =>
            {
                _preView.transform.DOScaleX(1f, .2f);
                _preView.transform.DOScaleY(1f, .2f);
            });
            buildService.SwitchState(typeof(BuildSelectState));
        }
    }
}