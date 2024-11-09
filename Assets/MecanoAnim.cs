using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using DG.Tweening;

public class MecanoAnim : MonoBehaviour
{
    [field:SerializeField] public UpgradeType MyType { get; private set; }
    
    [SerializeField] private Transform _transformA;
    [SerializeField] private Transform _transformB;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private float _roundTripDuration = .05f;
    [SerializeField] private bool _isOneTimePurchased;

    private Vector3 _startPos;
    private Quaternion _startRot;
    private float _startRotY;

    private void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
        _startRotY = transform.rotation.eulerAngles.y;
    }

    private IEnumerator AnimateMovement(int roundTrips)
    {
        yield return DOTween.Sequence()
            .Append(transform.DOMove(_transformA.position, _moveDuration))
            .Join(transform.DORotate(_transformA.rotation.eulerAngles, _moveDuration))
            .WaitForCompletion();

        for (int i = 0; i < roundTrips; i++)
        {
            yield return DOTween.Sequence()
                .Append(transform.DOMove(_transformB.position, _roundTripDuration))
                .Join(transform.DORotate(_transformB.rotation.eulerAngles, _roundTripDuration))
                .Append(transform.DOMove(_transformA.position, _roundTripDuration))
                .Join(transform.DORotate(_transformA.rotation.eulerAngles, _roundTripDuration))
                .WaitForCompletion();
        }

        yield return DOTween.Sequence()
            .Append(transform.DOMove(_startPos, _moveDuration))
            .Join(transform.DORotate(_startRot.eulerAngles, _moveDuration))
            .WaitForCompletion();
    }

    private void RotateAnim()
    {
        transform.DORotate(new Vector3(0, _startRotY + 360, 0), _moveDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad);
    }

    public void LaunchAnim()
    {
        if(_isOneTimePurchased)
            RotateAnim();
        else
            StartCoroutine(AnimateMovement(6));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LaunchAnim();
        }
    }
}