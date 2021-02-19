using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIcon : MonoBehaviour
{
    public Camera targetCamera;
    public Transform target;

    private RectTransform _rect;
    
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 point = targetCamera.WorldToScreenPoint(target.position);
        _rect.anchoredPosition = point;
    }
}
