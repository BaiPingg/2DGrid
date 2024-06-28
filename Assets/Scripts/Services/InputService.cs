using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : ServiceBase
{
    private PlayerControls _PlayerControls;
    public PlayerControls PlayerControls => _PlayerControls;
    public InputAction OpenInventory;
    public InputAction PlaceObj;
    public InputAction RotateObj;
    public InputAction MoveObj;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private LayerMask buildingLayerMask;
    private Camera _camera;

    public override void Init()
    {
        base.Init();
        _camera = Camera.main;
        _PlayerControls = new PlayerControls();
        _PlayerControls.Player.Enable();
        OpenInventory = _PlayerControls.Player.OpenInventory;
        PlaceObj = _PlayerControls.Player.PlaceObj;
        RotateObj = _PlayerControls.Player.RotateObj;
        MoveObj = _PlayerControls.Player.MoveObj;
    }

    public bool TryGetMouseWorldPos(out Vector3 pos)
    {
        pos = Vector3.zero;
        var mousePos = Mouse.current.position.ReadValue();

        var ray = _camera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            pos = hit.point;
            return true;
        }

        return false;
    }

    public bool TryMouseRaycast(out RaycastHit hit)
    {
        var mousePos = Mouse.current.position.ReadValue();

        var ray = _camera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 100, buildingLayerMask))
        {
            return true;
        }

        return false;
    }
}