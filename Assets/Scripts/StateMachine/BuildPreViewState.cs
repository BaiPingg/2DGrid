using System;
using System.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "BuildSystem/PreViewState")]
public class BuildPreViewState : BuildStateBase
{
    private bool _canPlace;

    public override void Enter(IStateMachine machine)
    {
        base.Enter(machine);
        if (buildService.Context.currentSelect != null)
        {
            var list = buildService.Context.currentSelect.GetPlaceCell();
            foreach (var cell in list)
            {
                SL.Get<GridService>().Grid.GetValue(cell.x, cell.y).Placed = false;
            }
            // var _buildItem = buildService.Context.currentSelect.BuildItem;
            //  var cell = SL.Get<GridService>().Grid
            //      .GetCellPosition(buildService.Context.currentSelect.transform.position);
            //  for (int i = _buildItem.placeX.x; i < _buildItem.placeX.y; i++)
            //  {
            //      for (int j = _buildItem.placeY.x; j < _buildItem.placeY.y; j++)
            //      {
            //          var target = cell + new Vector2Int(i, j);
            //          SL.Get<GridService>().Grid.GetValue(target.x, target.y).Placed = false;
            //      }
            //  }
        }
        else if (buildService.Context.currentBuildItem != null)
        {
            buildService.Context.currentSelect = Instantiate(buildService.Context.currentBuildItem.prefab)
                .GetComponent<BuildingViewBase>();
            buildService.Context.currentSelect.Init(buildService.Context.currentBuildItem);
        }

        buildService.Context.currentSelect.SetPreview();
        SL.Get<InputService>().PlaceObj.performed += PlaceObj;
        // SL.Get<InputService>().RotateObj.performed += RotateObj;
    }

    private void RotateObj(InputAction.CallbackContext obj)
    {
        // if (_preView.gameObject != null)
        // {
        //     var angle = _preView.gameObject.transform.eulerAngles;
        //     _preView.gameObject.transform.eulerAngles = angle + new Vector3(0, 90, 0);
        // }
    }


    public override void Leave()
    {
        base.Leave();
        SL.Get<InputService>().PlaceObj.performed -= PlaceObj;
        //     SL.Get<InputService>().RotateObj.performed -= RotateObj;
        _canPlace = false;
    }

    private void PlaceObj(InputAction.CallbackContext obj)
    {
        buildService.SwitchState(typeof(BuildPlacementState));
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        var grid = SL.Get<GridService>();
        var _buildItem = buildService.Context.currentSelect.BuildItem;
        if (SL.Get<InputService>().TryGetMouseWorldPos(out Vector3 pos))
        {
            buildService.Context.currentSelect.gameObject.SetActive(true);
            var cell = grid.Grid.GetCellPosition(pos);
            var sss = grid.Grid.GetWorldPosition(cell.x, cell.y);
            _canPlace = true;
            buildService.Context.currentSelect.SetPosition(sss, 0, true);
            var list = buildService.Context.currentSelect.GetPlaceCell();
            foreach (var posss in list)
            {
                if (SL.Get<GridService>().Grid.GetValue(posss.x, posss.y).Placed)
                {
                    _canPlace = false;
                }
            }
            // for (int i = _buildItem.placeX.x; i < _buildItem.placeX.y; i++)
            // {
            //     for (int j = _buildItem.placeY.x; j < _buildItem.placeY.y; j++)
            //     {
            //         var target = cell + new Vector2Int(i, j);
            //         if (grid.Grid.GetValue(target.x, target.y).Placed)
            //         {
            //             _canPlace = false;
            //         }
            //     }
            // }


            if (_canPlace)
            {
                buildService.Context.currentSelect.SetPreview();
            }
            else
            {
                buildService.Context.currentSelect.SetErrorView();
            }
        }
    }
}