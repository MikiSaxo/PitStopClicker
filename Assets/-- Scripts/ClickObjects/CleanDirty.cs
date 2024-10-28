using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CleanDirty : MonoBehaviour
{
    public static CleanDirty Instance;

    [Header("--- Move ")]
    [SerializeField] private float _returnDuration = 0.5f; 

    [Header("--- Clean ")]
    [SerializeField] private float _cleanDuration = 2f;
    
    public ClickCarClean ClickCarClean { get; set; }
    public bool IsSet { get; set; }

    private Camera _mainCamera;
    private Plane _plane;
    private Vector3 _initPos;
    private GameObject _dirtObject;
    private bool _hasClickedOnIt;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        IsSet = false;
        _initPos = transform.position;
        
        _mainCamera = Camera.main;
        Vector3 _offset = new Vector3(0, 2.5f, 0); // -> Faire en sorte de récup la hauteur du decal dirty
        _plane = new Plane(Vector3.up, _offset);
    }

    private void OnMouseDown()
    {
        _hasClickedOnIt = true;
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
    }

    private void OnTriggerStay(Collider other)
    {
      
    }

    private void CleanDirt(GameObject dirt)
    {
        print("On Dirt");
    }
}
