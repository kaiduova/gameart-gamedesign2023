using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ScaleWithCamera : MonoBehaviour
{ 
    [SerializeField]
    private Camera currentCamera;
    private Vector3 _defaultScale;
    private float _defaultCameraSize;

    private void Start()
    {
        _defaultScale = transform.localScale;
        _defaultCameraSize = currentCamera.orthographicSize;
    }

    private void Update()
    {
        transform.localScale = _defaultScale * (currentCamera.orthographicSize / _defaultCameraSize);
    }
}