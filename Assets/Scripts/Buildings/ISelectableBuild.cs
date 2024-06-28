using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface ISelectableBuild
{
    void Select();
    void UnSelect();
}

public interface IBuildingView
{
    public void SetPreview();
    public void SetNormalView();
    public void SetErrorView();
    public List<Vector2Int> GetPlaceCell();
    

    void SetPosition(Vector3 worldPosition, float angle, bool preview);
}