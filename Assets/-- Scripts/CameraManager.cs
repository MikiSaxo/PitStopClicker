using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    
    [SerializeField] private Transform[] _camTransforms;

    private Camera _mainCam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    public void GoToCam(int index)
    {
        _mainCam.transform.DOMove(_camTransforms[index].position, 1f).SetEase(Ease.InOutQuad);
        _mainCam.transform.DORotate(_camTransforms[index].rotation.eulerAngles, 1f).SetEase(Ease.InOutQuad);
    }
}
