using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ClickCarJack : ClickObjects
{
    [Header("--- Car Jack ")]
    [SerializeField] private Transform _jackPoint;
    [SerializeField] private float _goDuration = 1f;
    [SerializeField] private float _returnDuration = 1f;

    private Transform _startPoint;

    private void MoveToJackPoint()
    {
        transform.DOMove(_jackPoint.position, _goDuration).SetEase(Ease.InOutQuad);
        transform.DORotateQuaternion(_jackPoint.rotation, _goDuration).SetEase(Ease.InOutQuad);
    }

    public void ReturnFromJackPoint()
    {
        transform.DOMove(_startPoint.position, _returnDuration).SetEase(Ease.InOutQuad);
        transform.DORotateQuaternion(_startPoint.rotation, _returnDuration).SetEase(Ease.InOutQuad);
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        MoveToJackPoint();
    }
}