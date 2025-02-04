﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class UIService : MonoBehaviour, IService
{
    public Canvas rootCanvas;
    public Stack<UIPanel> openedPanels = new Stack<UIPanel>();

    public UIPanel startPanel;

    public UIPanel Current
    {
        get => openedPanels.Peek();
    }


    public UIPanel OpenPanel(UIPanel panel)
    {
        if (startPanel != null)
        {
            Destroy(startPanel.gameObject);
        }

        SL.Get<InputService>().PlayerControls.Player.Disable();
        UIPanel prePanel;
        openedPanels.TryPeek(out prePanel);
        if (prePanel)
        {
            prePanel.OnCover();
        }

        openedPanels.Push(panel);
        panel.transform.SetParent(rootCanvas.transform);
        panel.transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        panel.transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        panel.transform.eulerAngles = Vector3.zero;
        panel.transform.localScale = Vector3.one;

        panel.OnOpen(this);
        return panel;
    }

    public void ClosePanel()
    {
        var panel = openedPanels.Pop();
        panel.OnClose();
        Destroy(panel.gameObject);
        UIPanel prePanel;
        openedPanels.TryPeek(out prePanel);
        if (prePanel)
        {
            prePanel.OnResume();
        }
        else
        {
            SL.Get<InputService>().PlayerControls.Player.Enable();
        }
    }

    public void CloseAllPanel()
    {
        while (openedPanels.TryPeek(out UIPanel pp))
        {
            ClosePanel();
        }
    }

    public void Init()
    {
    }

    public void Shutdown()
    {
    }
}