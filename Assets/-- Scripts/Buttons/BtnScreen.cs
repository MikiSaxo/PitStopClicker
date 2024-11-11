using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnScreen : MonoBehaviour
{
    [SerializeField] private float _moveDistance = -0.035f;
    [SerializeField] private float _moveDuration = .25f;
    
    private Vector3 _initPos;
    private Vector3 _initScale;
    protected Vector3 _initRota;

    public virtual void Start()
    {
        _initPos = transform.localPosition;
        _initScale = transform.localScale;
        _initRota = transform.localEulerAngles;
    }

    public virtual void OnMouseDown()
    {
        AudioManager.Instance.PlaySound("Pop");

        AnimateCassette();
    }

    private void AnimateCassette()
    {
        BounceAnim();
        transform.DOLocalMoveZ(_moveDistance, _moveDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            transform.DOLocalMove(_initPos, 1f).SetEase(Ease.InOutQuad);
        });
    }
    
    private void BounceAnim()
    {
        transform.DOKill();
        Vector3 upScale = _initScale * 1.5f;

        transform.DOScale(upScale, 0.15f).SetEase(Ease.OutBounce).OnComplete(() => { transform.DOScale(_initScale, 0.05f); });
    }
}