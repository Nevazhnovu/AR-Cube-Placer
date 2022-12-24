using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionManager : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private ARPlaneManager planeManager;
    
    private bool isPlaneDetectionEnabled;

    private bool IsPlaneDetectionEnabled
    {
        get => isPlaneDetectionEnabled;
        set
        {
            planeManager.enabled = isPlaneDetectionEnabled = value;
            SetButtonState(value);
        }
    }

    private void SetButtonState(bool enabled)
    {
        print(">>> SetButtonState " + enabled);

        if (enabled)
        {
            var clr = new Color32(116, 238, 69, 255);
            image.color = clr;
            text.text = "Plane Detection: Enabled";
        }
        else
        {
            var clr = new Color32(238, 162, 69, 255);
            image.color = clr;
            text.text = "Plane Detection: Disabled";
        }
    }

    private void Start()
    {
        IsPlaneDetectionEnabled = true;
        button.onClick.AddListener(() => { IsPlaneDetectionEnabled = !IsPlaneDetectionEnabled; });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(() => { IsPlaneDetectionEnabled = !IsPlaneDetectionEnabled; });
    }
}
