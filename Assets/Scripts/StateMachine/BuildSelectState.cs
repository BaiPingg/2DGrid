using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "BuildSystem/BuildSelectState")]
public class BuildSelectState : BuildStateBase
{
    public override void Enter(IStateMachine machine)
    {
        base.Enter(machine);
        SL.Get<InputService>().OpenInventory.performed += OpenInventory;
        SL.Get<InputService>().MoveObj.performed += MoveObj;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            if (SL.Get<InputService>().TryMouseRaycast(out hit))
            {
                var select = hit.transform.GetComponent<BuildingViewBase>();
                if (select != null)
                {
                    buildService.Context.currentSelect = select;
                    select.Select();
                }
            }
            else
            {
                if (buildService.Context.currentSelect != null)
                {
                    buildService.Context.currentSelect.UnSelect();
                    buildService.Context.currentSelect = null;
                }
            }
        }
    }

    public override void Leave()
    {
        base.Leave();
        SL.Get<InputService>().OpenInventory.performed -= OpenInventory;
        SL.Get<InputService>().MoveObj.performed -= MoveObj;
    }

    private void MoveObj(InputAction.CallbackContext obj)
    {
        buildService.SwitchState(typeof(BuildPreViewState));
    }

    private async void OpenInventory(InputAction.CallbackContext obj)
    {
        var handle = Addressables.InstantiateAsync("Assets/GameAssets/Prefabs/UIPanels/InventoryPanel.prefab");
        await handle.Task;
        var panel = handle.Result.GetComponent<UIPanel>();
        SL.Get<UIService>().OpenPanel(panel);
    }
}