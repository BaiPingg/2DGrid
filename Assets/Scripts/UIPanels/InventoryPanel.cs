using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : UIPanel
{
    [Required, SerializeField] private GameObject _template;
    [Required, SerializeField] private Transform _contentParent;

    public override void OnOpen(UIService service)
    {
        base.OnOpen(service);
        transform.eulerAngles = Vector3.zero;
        foreach (var buildItem in SL.Get<BuildService>().buildItems)
        {
            var obj = Instantiate(_template, _contentParent);
            obj.SetActive(true);
            obj.GetComponent<Image>().sprite = buildItem.uiPreview;
            obj.GetComponent<Button>().onClick.AddListener((() =>
            {
                SL.Get<BuildService>().Context.currentBuildItem = buildItem;
                SL.Get<BuildService>().SwitchState(typeof(BuildPreViewState));
                SL.Get<UIService>().ClosePanel();
            }));
        }
    }
}