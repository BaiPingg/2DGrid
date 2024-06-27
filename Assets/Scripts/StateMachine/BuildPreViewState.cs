using System;
using System.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "BuildSystem/PreViewState")]
public class BuildPreViewState : BuildStateBase
{
    private BuildItem _buildItem;
    private GameObject _preView;
    [SerializeField, Required] private Material _previewMaterial;
    [SerializeField, Required] private Material _previewErrorMaterial;

    private bool _canPlace;

    public override void Enter(IStateMachine machine)
    {
        base.Enter(machine);

        _buildItem = buildService.currentBuildItem;

        _preView = Instantiate(_buildItem.prefab);
        SetMaterial(_preView, _previewMaterial);
        SL.Get<InputService>().PlaceObj.performed += PlaceObj;
        SL.Get<InputService>().RotateObj.performed += RotateObj;
    }

    private void RotateObj(InputAction.CallbackContext obj)
    {
        if (_preView.gameObject != null)
        {
            var angle = _preView.gameObject.transform.eulerAngles;
            _preView.gameObject.transform.eulerAngles = angle + new Vector3(0, 90, 0);
        }
    }

    public void SetMaterial(GameObject obj, Material mat)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = mat;
        }
    }

    public override void Leave()
    {
        base.Leave();
        Destroy(_preView);
        SL.Get<InputService>().PlaceObj.performed -= PlaceObj;
        SL.Get<InputService>().RotateObj.performed -= RotateObj;
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
        if (SL.Get<InputService>().TryGetMouseWorldPos(out Vector3 pos))
        {
            _preView.gameObject.SetActive(true);
            var cell = grid.Grid.GetCellPosition(pos);
            var sss = grid.Grid.GetWorldPosition(cell.x, cell.y);
            _canPlace = true;
            for (int i = _buildItem.placeX.x; i < _buildItem.placeX.y; i++)
            {
                for (int j = _buildItem.placeY.x; j < _buildItem.placeY.y; j++)
                {
                    var target = cell + new Vector2Int(i, j);
                    if (grid.Grid.GetValue(target.x, target.y).Placed)
                    {
                        _canPlace = false;
                    }
                }
            }

            _preView.gameObject.transform.position = sss;
            if (_canPlace)
            {
                SetMaterial(_preView, _previewMaterial);
            }
            else
            {
                SetMaterial(_preView, _previewErrorMaterial);
            }
        }
    }
}