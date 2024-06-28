using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class BuildingViewBase : MonoBehaviour, ISelectableBuild, IBuildingView
{
    private BuildItem _buildItem;
    public BuildItem BuildItem => _buildItem;
    [SerializeField, Required] private Material _previewMaterial;
    [SerializeField, Required] private Material _previewErrorMaterial;
    [SerializeField, Required] private Material _normalMaterial;

    public void Init(BuildItem buildItem)
    {
        this._buildItem = buildItem;
    }

    public void Select()
    {
        Debug.Log($"{gameObject.name} selected");
    }

    public void UnSelect()
    {
        Debug.Log($"{gameObject.name} unselected");
    }


    public void SetPreview()
    {
        SetMaterial(_previewMaterial);
    }

    public void SetNormalView()
    {
        SetMaterial(_normalMaterial);
    }

    public void SetErrorView()
    {
        SetMaterial(_previewErrorMaterial);
    }

    public void SetPosition(Vector3 worldPosition, bool preview)
    {
        transform.position = worldPosition;
        if (!preview)
        {
            var grid = SL.Get<GridService>();
            var cell = grid.Grid.GetCellPosition(worldPosition);
            for (int i = _buildItem.placeX.x; i < _buildItem.placeX.y; i++)
            {
                for (int j = _buildItem.placeY.x; j < _buildItem.placeY.y; j++)
                {
                    var target = cell + new Vector2Int(i, j);
                    grid.Grid.GetValue(target.x, target.y).Placed = true;
                }
            }

            transform.DOScaleX(.9f, .2f);
            transform.DOScaleY(1.5f, .2f).onComplete += (() =>
            {
                transform.DOScaleX(1f, .2f);
                transform.DOScaleY(1f, .2f);
            });
        }
    }

    private void SetMaterial(Material mat)
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = mat;
        }
    }
}