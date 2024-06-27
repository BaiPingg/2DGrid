using System;
using GAS;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu]
public class BuildItem : ScriptableObject, ICopyable<BuildItem>
{
    public string itemName;
    public string guid  =Guid.NewGuid().ToString();

    public Vector2Int placeX;
    public Vector2Int placeY;
    [ShowAssetPreview] 
    public Sprite uiPreview;

    [ShowAssetPreview]
    public GameObject prefab;

    public BuildItem Copy()
    {
        return Instantiate(this);
    }
}