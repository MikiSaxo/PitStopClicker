using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClickCarJack : ClickObjects
{
    public static ClickCarJack Instance;

    [Header("--- Car Jack ")]
    [SerializeField] private Transform _jackPoint;
    [SerializeField] private float _goDuration = 1f;
    [SerializeField] private float _returnDuration = 1f;
    [SerializeField] private float _heightCar = 1f;

    public bool IsSet { get; set; }

    private Vector3 _startPos;
    private Quaternion _startRota;
    private CarMovement _currentCar;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsSet = false;
        _startPos = transform.position;
        _startRota = transform.rotation;
    }

    private void MoveToJackPoint()
    {
        transform.DOKill();

        IsSet = true;
        transform.DOMove(_jackPoint.position, _goDuration).SetEase(Ease.InOutQuad).OnComplete(() => SetHeightCurrentCar(true));
        transform.DORotateQuaternion(_jackPoint.rotation, _goDuration).SetEase(Ease.InOutQuad);
    }

    public void ReturnFromJackPoint()
    {
        transform.DOKill();

        IsSet = false;
        
        SetHeightCurrentCar(false);
        transform.DOMove(_startPos, _returnDuration).SetEase(Ease.InOutQuad);
        transform.DORotateQuaternion(_startRota, _returnDuration).SetEase(Ease.InOutQuad);
    }

    public void SelectMeAnim()
    {
        transform.DOKill();
    
        float shakeStrength = 0.2f;

        float randomX = _startPos.x + Random.Range(-shakeStrength, shakeStrength);
        float randomZ = _startPos.z + Random.Range(-shakeStrength, shakeStrength);
        Vector3 randomPosition = new Vector3(randomX, _startPos.y, randomZ);

        transform.DOPunchScale(Vector3.one * 0.05f, 0.05f, 10, 1);

        transform.DOMove(randomPosition, 0.1f).SetEase(Ease.Linear)
            .OnComplete(() => transform.DOMove(_startPos, 0.2f));
    }
    
    private void SetHeightCurrentCar(bool isUp)
    {
        _currentCar = CarSpawner.Instance.CurrentCar;

        if(isUp)
            _currentCar.transform.DOMoveY(_currentCar.transform.position.y + _heightCar, 0.25f);
        else
            _currentCar.transform.DOMoveY(_currentCar.transform.position.y - _heightCar, 0.1f);
            
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if(CarSpawner.Instance.CurrentCar != null && !CarSpawner.Instance.CurrentCar.IsAtClickPoint) return;
        
        if (IsSet)
            ReturnFromJackPoint();
        else
            MoveToJackPoint();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            OnClicked(Vector3.zero);
    }
}