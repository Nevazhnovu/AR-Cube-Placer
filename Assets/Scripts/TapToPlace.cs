using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    [SerializeField] private Slider sizeSlider;
    
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
    ARAnchorManager m_ReferencePointManager;
    List<ARAnchor> m_ReferencePoint;
    ARPlaneManager m_PlaneManager;

    private ARAnchor objectAnchor;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_ReferencePointManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_ReferencePoint = new List<ARAnchor>();
        sizeSlider.onValueChanged.AddListener(OnSliderSizeChange);
    }

    private void OnDestroy()
    {
        sizeSlider.onValueChanged.RemoveListener(OnSliderSizeChange);
    }

    void OnSliderSizeChange(float value)
    {
        ChangeObjectSize(objectAnchor, sizeSlider.value);
    }
    
    //Remove all reference points created
    public void RemoveAllReferencePoints()
    {
        foreach (var referencePoint in m_ReferencePoint)
        {
            m_ReferencePointManager.RemoveAnchor(referencePoint);
        }
        m_ReferencePoint.Clear();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (!ClickBlocker.isBlocked && Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    void Update()
    {
        if(m_PlaneManager == null || !m_PlaneManager.isActiveAndEnabled) return;
        if (!TryGetTouchPosition(out Vector2 touchPosition)) return;
        
        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            TrackableId planeId = s_Hits[0].trackableId; //get the ID of the plane hit by the raycast
            var referencePoint = m_ReferencePointManager.AttachAnchor(m_PlaneManager.GetPlane(planeId), hitPose);
            objectAnchor = referencePoint;
            
            if (sizeSlider.isActiveAndEnabled)
            { 
                print(">>> referencePoint= " + referencePoint.gameObject);
                ChangeObjectSize(referencePoint, sizeSlider.value);
            }

            if (referencePoint != null)
            {
                RemoveAllReferencePoints();
                m_ReferencePoint.Add(referencePoint);
            }
        }
    }

    void ChangeObjectSize(ARAnchor referencePoint, float value)
    {
        if(referencePoint != null && referencePoint.isActiveAndEnabled)
            referencePoint.transform.localScale = new Vector3(
                x: value,
                y: value,
                z: value);
    }
}
