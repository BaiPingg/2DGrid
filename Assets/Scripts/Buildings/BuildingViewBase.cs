using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class BuildingViewBase : MonoBehaviour, ISelectableBuild, IBuildingView
{
    private BuildItem _buildItem;
    public BuildItem BuildItem => _buildItem;
    [ReadOnly] private float _angle;

    public float Angle
    {
        get => _angle;
        set
        {
            if (Math.Abs(_angle - value) < 0.01f)
            {
                _angle = value;
                transform.eulerAngles = new Vector3(0, _angle, 0);
            }
        }
    }


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

    public List<Vector2Int> GetPlaceCell()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        var cell = SL.Get<GridService>().Grid.GetCellPosition(transform.position);
        for (int i = _buildItem.placeX.x; i < _buildItem.placeX.y; i++)
        {
            for (int j = _buildItem.placeY.x; j < _buildItem.placeY.y; j++)
            {
                var target = cell + new Vector2Int(i, j);
                result.Add(target);
            }
        }

        return result;
    }

    public void SetPosition(Vector3 worldPosition, float angle, bool preview)
    {
        transform.position = worldPosition;
        if (!preview)
        {
            // var grid = SL.Get<GridService>();
            // var cell = grid.Grid.GetCellPosition(worldPosition);
            // for (int i = _buildItem.placeX.x; i < _buildItem.placeX.y; i++)
            // {
            //     for (int j = _buildItem.placeY.x; j < _buildItem.placeY.y; j++)
            //     {
            //         var target = cell + new Vector2Int(i, j);
            //         grid.Grid.GetValue(target.x, target.y).Placed = true;
            //     }
            // }
            var list = GetPlaceCell();
            foreach (var celld in list)
            {
                SL.Get<GridService>().Grid.GetValue(celld.x, celld.y).Placed = true;
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