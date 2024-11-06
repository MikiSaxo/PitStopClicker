using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCam : BtnScreen
{
    [SerializeField] private int _camIndex;

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        ChangeCamPos();
    }

    private void ChangeCamPos()
    {
        CameraManager.Instance.GoToCam(_camIndex);
    }
}
