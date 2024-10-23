using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float _spawnDuration = 2f;
    [SerializeField] private float _exitDuration = 1f;
    [SerializeField] private int _clickLimit = 5;

    private Transform[] _movementPoints;
    private int _currentClicks = 0;
    private bool _isAtClickPoint = false;

    public UnityAction OnCarDestroyed;

    public void Init(Transform[] movPoints)
    {
        _movementPoints = movPoints;
        transform.position = _movementPoints[0].position;
        MoveToClickPoint();

        _clickLimit = 5;
    }

    void MoveToClickPoint()
    {
        transform.DOMove(_movementPoints[1].position, _spawnDuration).OnComplete(() => { _isAtClickPoint = true; });
    }

    public void OnCarClicked()
    {
        if (_isAtClickPoint && _currentClicks < _clickLimit)
        {
            _currentClicks++;
            ProvideFeedback();

            if (_currentClicks >= _clickLimit)
            {
                Debug.Log("Car repaired after " + _clickLimit + " clicks!");
                MoveToExitPoint();
            }
        }
    }
    
    void ProvideFeedback()
    {
        transform.DOPunchScale(Vector3.one * 0.2f, 0.05f, 10, 1);
    }

    void MoveToExitPoint()
    {
        transform.DOMove(_movementPoints[2].position, _exitDuration).SetEase(Ease.InQuart).OnComplete(() =>
        {
            Debug.Log("Car has exited!");
            DestroyCar();
        });
    }

    private void DestroyCar()
    {
        OnCarDestroyed?.Invoke();
        StartCoroutine(WaitToDeath());
    }

    private IEnumerator WaitToDeath()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}