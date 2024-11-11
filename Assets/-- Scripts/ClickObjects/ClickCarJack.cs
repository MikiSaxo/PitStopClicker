using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ClickCarJack : ClickObjects
{
    public static ClickCarJack Instance;

    [Header("--- Car Jack ")]
    [SerializeField] private Transform _jackPoint;

    [SerializeField] private float _goDuration = 1f;
    [SerializeField] private float _returnDuration = 1f;
    [SerializeField] private float _heightCar = 1f;

    
    public UnityAction OnCarJackSet;

    public bool IsSet { get; set; }

    private Vector3 _initPos;
    private Quaternion _initRota;
    private CarMovement _currentCar;
    private bool _isJumping;

    readonly float _jumpPower = 0.2f;
    readonly float _duration = 0.25f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsSet = false;
        
        _initPos = transform.position;
        _initRota = transform.rotation;
        
        CarSpawner.Instance.OnCarAtClickPoint += CheckUpgradeAutoMove;
    }

    public void CheckUpgradeAutoMove()
    {
        if(UpgradeManager.Instance.CurrentMecanoPower[(int)_myType] == 1 && !IsSet)
            MoveToJackPoint();
    }
    private void MoveToJackPoint()
    {
        transform.DOKill();

        IsSet = true;
        GasCan.Instance.CheckUpgradeAutoMove();
        
        transform.DOMove(_jackPoint.position, _goDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            SetHeightCurrentCar(true);
            OnCarJackSet?.Invoke();
        });
        transform.DORotateQuaternion(_jackPoint.rotation, _goDuration).SetEase(Ease.InOutQuad);
    }

    public void ReturnFromJackPoint()
    {
        transform.DOKill();

        IsSet = false;

        SetHeightCurrentCar(false);
        transform.DOMove(_initPos, _returnDuration).SetEase(Ease.InOutQuad);
        transform.DORotateQuaternion(_initRota, _returnDuration).SetEase(Ease.InOutQuad);
    }

    public override void SelectMeAnim()
    {
        if (_isJumping) return;

        transform.DOComplete();
        _isJumping = true;

        transform.DOJump(transform.position, _jumpPower, 1, _duration)
            .OnComplete(() => { _isJumping = false; });
    }

    private void SetHeightCurrentCar(bool isUp)
    {
        _currentCar = CarSpawner.Instance.CurrentCar;

        _currentCar.SetHeightCarJack(isUp, _heightCar);
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if (UpgradeManager.Instance.CurrentMecanoPower[(int)_myType] == 1)
            return;
        
        if (CarSpawner.Instance.CurrentCar == null) return;

        if (!CarSpawner.Instance.CurrentCar.IsAtClickPoint)
        {
            CantSelectAnim();
            return;
        }
        
        if (!IsSet)
            MoveToJackPoint();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !IsSet)
            OnClicked(Vector3.zero);
    }
}