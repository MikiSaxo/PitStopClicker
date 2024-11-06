using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCam : MonoBehaviour
{
    [SerializeField] private int _camIndex;

    private void OnMouseDown()
    {
        ChangeCamPos();
    }

    private void ChangeCamPos()
    {
        CameraManager.Instance.GoToCam(_camIndex);
    }
}
