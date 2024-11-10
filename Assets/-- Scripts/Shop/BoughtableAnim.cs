using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoughtableAnim : MonoBehaviour
{
    [SerializeField] private float _jumpPower = 0.75f;
    [SerializeField] private float _jumpDuration = 0.5f;
    [SerializeField] private float _rotateDuration = 0.5f;

    private Vector3 _initPos;
    private float _initRotY;

    private void Awake()
    {
        _initPos = transform.position;
        _initRotY = transform.rotation.eulerAngles.y;
    }

    private void OnMouseDown()
    {
        JumpAnim();
    }

    private void JumpAnim()
    {
        transform.DOJump(_initPos, _jumpPower, 1, _jumpDuration);
    }

    private void RotateAnim()
    {
        transform.DORotate(new Vector3(0, _initRotY + 360, 0), _rotateDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad);
    }

    public void BuyAnim()
    {
        JumpAnim();
        RotateAnim();
    }
}