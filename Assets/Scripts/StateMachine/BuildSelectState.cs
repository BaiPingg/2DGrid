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
    }

    public override void Leave()
    {
        base.Leave();
        SL.Get<InputService>().OpenInventory.performed -= OpenInventory;
    }

    private async void OpenInventory(InputAction.CallbackContext obj)
    {
        var handle = Addressables.InstantiateAsync("Assets/GameAssets/Prefabs/UIPanels/InventoryPanel.prefab");
        await handle.Task;
        var panel = handle.Result.GetComponent<UIPanel>();
        SL.Get<UIService>().OpenPanel(panel);
    }
}