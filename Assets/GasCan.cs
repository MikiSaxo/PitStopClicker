using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GasCan : ClickObjects
{
    public static GasCan Instance;

    [Header("--- Gas Can ")]
    [SerializeField] private float _goDuration = 1f;
    [SerializeField] private float _returnDuration = 1f;
    [SerializeField] private float _heightCar = 1f;

    public bool IsSet { get; set; }

    private Vector3 _initPos;
    private Quaternion _initRota;
    private CarMovement _currentCar;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsSet = false;
        
        _initPos = transform.position;
        _initRota = transform.rotation;
    }

    public void MoveToGasPoint(Transform gasPoint)
    {
        transform.DOKill();

        IsSet = true;
        transform.DOMove(gasPoint.position, _goDuration).SetEase(Ease.InOutQuad);
        transform.DORotateQuaternion(gasPoint.rotation, _goDuration).SetEase(Ease.InOutQuad);
    }

    public void ReturnFromGasPoint()
    {
        transform.DOKill();

        IsSet = false;

        transform.DOMove(_initPos, _returnDuration).SetEase(Ease.InOutQuad);
        transform.DORotateQuaternion(_initRota, _returnDuration).SetEase(Ease.InOutQuad);
    }

    public void SelectMeAnim()
    {
        transform.DOKill();
        
        float randomAngle = Random.Range(-20f, 20f);
        transform.DOPunchScale(Vector3.one * 0.05f, 0.05f, 10, 1);
        transform.DORotate(new Vector3(0, randomAngle, 0), 0.1f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                transform.DOMove(_initPos, 0.1f);
                transform.DORotateQuaternion(_initRota, 0.1f);
            });
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if (CarSpawner.Instance.CurrentCar != null && !CarSpawner.Instance.CurrentCar.IsAtClickPoint) return;

        if (IsSet)
            ReturnFromGasPoint();
        // else
            // MoveToGasPoint();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            OnClicked(Vector3.zero);
    }
}