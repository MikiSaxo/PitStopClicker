using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GasCan : ClickObjects
{
    public static GasCan Instance;
    
    [SerializeField] private UpgradeType _mySecondType;

    [Header("--- Move ")]
    [SerializeField] private float _goDuration = 1f;
    [SerializeField] private float _returnDuration = 1f;

    [Header("--- Fill ")]
    [SerializeField] private GameObject _fillGas;

    public bool IsSet { get; set; }
    public ClickGasCan ClickGasCan { get; set; }

    private Vector3 _initPos;
    private Quaternion _initRota;
    private CarMovement _currentCar;
    private Transform _gasPoint;
    private Collider _collider;
    private float _currentRotaY;
    private float _fillPower => UpgradeManager.Instance.CurrentRepairPower[(int)_myType];
    private float _fillAuto => UpgradeManager.Instance.CurrentRepairPower[(int)_mySecondType];
    
    private readonly float _rotateDuration = 0.1f;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsSet = false;
        
        _initPos = transform.position;
        _initRota = transform.rotation;
        _collider = GetComponent<Collider>();

        CarSpawner.Instance.OnCarAtClickPoint += CheckUpgradeAutoMove;
    }
    
    public void CheckUpgradeAutoMove()
    {
        if (UpgradeManager.Instance.CurrentMecanoPower[(int)_mySecondType] == 1
            && ClickCarJack.Instance.IsSet
            && CarSpawner.Instance.CurrentCar.IsAtClickPoint
            && ClickGasCan != null
            && !ClickGasCan.IsRepaired)
        {
            MoveToGasPoint();
        }
    }

    public void MoveToGasPoint()
    {
        if (ClickGasCan == null)
            return;

        PlaySoundClick();
        transform.DOKill();
        
        _gasPoint = ClickGasCan.transform;
        _collider.enabled = false;

        IsSet = true;
        transform.DOMove(_gasPoint.position, _goDuration).SetEase(Ease.InOutQuad).OnComplete(FillAnim);
        transform.DORotate(new Vector3(0, 115, 0), _goDuration).SetEase(Ease.InOutQuad);
    }

    public void ReturnFromGasPoint()
    {
        AudioManager.Instance.StopSound("LoadingFillGas");
        AudioManager.Instance.PlaySound("Ding");

        transform.DOKill();
        
        // Go Back
        transform.DOMove(_initPos, _returnDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            // Refuel Gas
            _fillGas.transform.DOScale(new Vector3(1, 1, 1), .5f).SetEase(Ease.InOutQuad);
            _collider.enabled = true;
            IsSet = false;
        });

        transform.DORotateQuaternion(_initRota, _returnDuration).SetEase(Ease.InOutQuad);
    }

    public override void SelectMeAnim()
    {
        transform.DOKill();

        _currentRotaY = transform.rotation.eulerAngles.y;
        float randomAngle = Random.Range(_currentRotaY - 20f, _currentRotaY + 20f);

        // Rotate left
        transform.DORotate(new Vector3(0, -randomAngle, 0), _rotateDuration).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // Rotate right
                transform.DORotate(new Vector3(0, randomAngle, 0), _rotateDuration).SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        // Return to initial rotation
                        transform.DORotateQuaternion(_initRota, _rotateDuration).SetEase(Ease.InOutQuad);
                    });
            });
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if (CarSpawner.Instance.CurrentCar == null 
            || !CarSpawner.Instance.CurrentCar.IsAtClickPoint
            || ClickGasCan == null
            || ClickGasCan.IsRepaired)
        {
            CantSelectAnim();
            return;
        }

        if (ClickCarJack.Instance.IsSet == false)
        {
            ClickCarJack.Instance.SelectMeAnim();
            return;
        }

        if (!IsSet)
            MoveToGasPoint();
    }

    private void FillAnim()
    {
        AudioManager.Instance.PlaySound("LoadingFillGas");
        var fillDuration = ClickGasCan.FillDurationCar / _fillPower;
        _fillGas.transform.DOScale(new Vector3(1, 0.01f, 1), fillDuration).SetEase(Ease.InOutQuad).OnComplete(ReturnFromGasPoint);
        ClickGasCan.LaunchGasAnim(fillDuration);
    }

    private void OnDisable()
    {
        CarSpawner.Instance.OnCarAtClickPoint -= CheckUpgradeAutoMove;
    }
}