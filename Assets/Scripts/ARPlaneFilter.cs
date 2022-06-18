using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneFilter : MonoBehaviour
{
    [SerializeField] private Vector2 bigPlaneDimensions;
    [SerializeField] private ARPlaneManager planeManager;
    private List<ARPlane> planes = new List<ARPlane>();
    public event Action OnHorizontalPlaneFound, OnVerticalPlaneFound, OnBigPlaneFound;
    private void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }
        
    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null && args.added.Count > 0)
            planes.AddRange(args.added);
        foreach (ARPlane plane in planes.Where(x => x.extents.x * x.extents.y >= 0.1f))
        {
            if (plane.alignment.IsVertical())
            {
                // we found a vertical plane
                OnHorizontalPlaneFound?.Invoke();
                // OnHorizontalPlaneFound += () => { }; //example of subscribing to anonymous function
            }
            else if (plane.alignment.IsHorizontal())
            {
                // we found a horizontal plane
                OnVerticalPlaneFound?.Invoke();
            }
            if (plane.extents.x * plane.extents.y >= bigPlaneDimensions.x * bigPlaneDimensions.y)
            {
                // we found a huge plane
                OnBigPlaneFound?.Invoke();
            }
        }
    }
}
