using System.Collections;
using System.Collections.Generic;
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
    
    void SetPosition(Vector3 worldPosition,bool preview);
}