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

    private void Start()
    {
        _initPos = transform.localPosition;
        _initScale = transform.localScale;
    }

    public virtual void OnMouseDown()
    {
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
        Vector3 upScale = _initScale * 1.1f;

        transform.DOScale(upScale, 0.05f).SetEase(Ease.InOutQuad).OnComplete(() => { transform.DOScale(_initScale, 0.05f).SetEase(Ease.InOutQuad); });
    }
}