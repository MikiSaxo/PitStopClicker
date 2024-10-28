using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CleanDirty : ClickObjects
{
    public static CleanDirty Instance;

    [Header("--- Move ")]
    [SerializeField] private float _returnDuration = 0.5f;

    [Header("--- Clean ")]
    [SerializeField] private float _cleanPower;
    [SerializeField] private GameObject _cleanFX;
    [SerializeField] private Vector3 _cleanRotation;

    public ClickCarClean ClickCarClean { get; set; }

    private Camera _mainCamera;
    private Plane _plane;
    private Vector3 _initPos;
    private Vector3 _initRota;
    private GameObject _dirtObject;
    private bool _hasClickedOnIt;

    private Vector3 _lastPosition;
    private bool _isCollidingWithCleanable;
    private bool _isRepaired;
    private bool _isJumping;
    private float _cleanProgress;
    private float _cleaningThreshold = 0.2f;

    readonly float _jumpPower = 0.2f;
    readonly float _duration = 0.25f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _initPos = transform.position;
        _initRota = transform.rotation.eulerAngles;

        _mainCamera = Camera.main;
        Vector3 _offset = new Vector3(0, 2.5f, 0); // -> Faire en sorte de récup la hauteur du decal dirty
        _plane = new Plane(Vector3.up, _offset);
    }

    public void NewCarComing(ClickCarClean clickCarClean)
    {
        ClickCarClean = clickCarClean;
        _isRepaired = false;
        _cleanProgress = 0;
        
        _hasClickedOnIt = false;
        GoInitPos();
    }

    private void OnMouseDown()
    {
        if (!ClickCarJack.Instance.IsSet)
        {
            ClickCarJack.Instance.SelectMeAnim();
            return;
        }
        
        _hasClickedOnIt = true;
    }

    public override void OnClicked(Vector3 hitPoint)
    {
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && _hasClickedOnIt)
        {
            Ray _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            float _hitDistance;

            if (_plane.Raycast(_ray, out _hitDistance))
            {
                Vector3 _hitPoint = _ray.GetPoint(_hitDistance);
                transform.position = _hitPoint;

                if (_isCollidingWithCleanable)
                {
                    DetectBackAndForth();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _hasClickedOnIt = false;
            GoInitPos();
        }
    }

    private void GoInitPos()
    {
        transform.DOKill();
        transform.DOMove(_initPos, _returnDuration).SetEase(Ease.InOutQuad);
        transform.DORotate(_initRota, _returnDuration).SetEase(Ease.InOutQuad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ClickCarClean>() != null)
        {
            _isCollidingWithCleanable = true;

            transform.DOKill();
            transform.DORotate(_cleanRotation, 0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<ClickCarClean>() != null)
        {
            _isCollidingWithCleanable = false;
            transform.DORotate(_initRota, 0.5f);
        }
    }

    private void DetectBackAndForth()
    {
        Vector3 direction = transform.position - _lastPosition;
        
        if (direction.magnitude <= _cleaningThreshold) return;

        if (Vector3.Dot(direction.normalized, (_lastPosition - _initPos).normalized) < 0)
        {
            _cleanProgress += _cleanPower;
            ClickCarClean.UpdateDecalProjector(_cleanPower);
            if (_cleanProgress >= 1)
            {
                if (!_isRepaired)
                {
                    _isRepaired = true;
                    
                    Debug.Log("Nettoyage terminé !");
                    ClickHandler.Instance.CreateFXRepairGood(transform.position);
                    ClickCarClean.CleanFinished();
                }
            }
            else
            {
                Instantiate(_cleanFX, transform.position, _cleanFX.transform.rotation);
            }
        }

        _lastPosition = transform.position;
    }

    public override void SelectMeAnim()
    {
        print("call SelectMeAnim");
        if (_isJumping) return;

        print("call SelectMeAnim 2");
        _isJumping = true;

        transform.DOJump(transform.position, _jumpPower, 1, _duration)
            .OnComplete(() =>
            {
                _isJumping = false;
                print("jump false");
            });

        StartCoroutine(ResetIsJumping());
    }

    IEnumerator ResetIsJumping()
    {
        yield return new WaitForSeconds(0.5f);
        _isJumping = false;
    }
}