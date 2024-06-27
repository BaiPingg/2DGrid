using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class CharacterTest : MonoBehaviour
{
    public Transform target;

    [Button("move to target")]
    void MoveRandom()
    {
        var pathPoint = SL.Get<GridService>().FindPath(transform.position, target.transform.position);
        if (pathPoint != null && pathPoint.Count != 0)
        {
            transform.DOPath(pathPoint.ToArray(), 3f, gizmoColor: Color.green);
        }
    }
}