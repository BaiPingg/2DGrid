using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrp : MonoBehaviour
{
    private void Awake()
    {
        SL.Register<InputService>(transform.GetComponentInChildren<InputService>());
        SL.Register<UIService>(transform.GetComponentInChildren<UIService>());
        SL.Register<GridService>(transform.GetComponentInChildren<GridService>());
        SL.Register<BuildService>(transform.GetComponentInChildren<BuildService>());
      
    }
}
