using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GasCan : ClickObjects
{
    public static GasCan Instance;

    [Header("--- Move ")]
    [SerializeField] private float _goDuration = 1f;

    [SerializeField] private float _returnDuration = 1f;

    [Header("--- Fill ")]
    [SerializeField] private GameObject _fillGas;

    [SerializeField] private float _fillDuration = 2f;

    public bool IsSet { get; set; }
    public ClickGasCan ClickGasCan { get; set; }

    private Vector3 _initPos;
    private Quaternion _initRota;
    private CarMovement _currentCar;
    private Transform _gasPoint;
    private Collider _collider;
    private float _currentRotaY;
    
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
    }

    public void MoveToGasPoint()
    {
        transform.DOKill();
        _gasPoint = ClickGasCan.transform;

        _collider.enabled = false;

        IsSet = true;
        transform.DOMove(_gasPoint.position, _goDuration).SetEase(Ease.InOutQuad).OnComplete(FillAnim);
        transform.DORotate(new Vector3(0, 115, 0), _goDuration).SetEase(Ease.InOutQuad);
    }

    public void ReturnFromGasPoint()
    {
        transform.DOKill();

        IsSet = false;
        _collider.enabled = true;

        transform.DOMove(_initPos, _returnDuration).SetEase(Ease.InOutQuad).OnComplete(()
            => _fillGas.transform.DOScale(new Vector3(1, 1, 1), _fillDuration * .25f).SetEase(Ease.InOutQuad));

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
        if (CarSpawner.Instance.CurrentCar != null && !CarSpawner.Instance.CurrentCar.IsAtClickPoint) return;

        if (ClickGasCan != null && ClickGasCan.IsRepaired) return;

        if (ClickCarJack.Instance.IsSet == false) return;

        if (!IsSet)
            MoveToGasPoint();
    }

    private void FillAnim()
    {
        _fillGas.transform.DOScale(new Vector3(1, 0.01f, 1), _fillDuration).SetEase(Ease.InOutQuad).OnComplete(ReturnFromGasPoint);
        ClickGasCan.LaunchGasAnim(_fillDuration);
    }
}