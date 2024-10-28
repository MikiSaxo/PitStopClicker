using DG.Tweening;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Plane _plane;
    private Camera _mainCamera;
    private Vector3 _initialPosition;

    void Start()
    {
        _mainCamera = Camera.main;
        Vector3 _offset = new Vector3(0, 2.5f, 0);
        _plane = new Plane(Vector3.up, _offset);
        _initialPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
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
            transform.DOMove(_initialPosition, 0.25f);
        }
    }
}