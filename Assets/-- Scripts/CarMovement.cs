using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CarMovement : MonoBehaviour, IClickable
{
    [SerializeField] private int _clickLimit = 5;

    [Header("--- Timing")]
    [SerializeField] private float _spawnDuration = 2f;
    [SerializeField] private float _exitDuration = 1f;
    
    [Header("--- Models")]
    [SerializeField] private Transform _modelParent;
    [SerializeField] private GameObject[] _models;
    [SerializeField] private ClickEngine _clickEngine;

    private Transform[] _movementPoints;
    private int _currentClicks = 0;
    private bool _isAtClickPoint = false;
    private GameObject _saveModel;


    public UnityAction OnCarDestroyed;

    public void Init(Transform[] movPoints)
    {
        _movementPoints = movPoints;
        transform.position = _movementPoints[0].position;
        
        AddRandomModel();
        MoveToClickPoint();

        _clickLimit = 5;
    }

    private void AddRandomModel()
    {
        int rdn = Random.Range(0, _models.Length);
        _saveModel = Instantiate(_models[rdn], _modelParent);
        _clickEngine.Init(this, _clickLimit);
    }
    private void MoveToClickPoint()
    {
        transform.DOMove(_movementPoints[1].position, _spawnDuration).OnComplete(() => { _isAtClickPoint = true; });
    }

    public void OnClicked()
    {
        if (_isAtClickPoint && _currentClicks < _clickLimit)
        {
            _currentClicks++;
            ProvideFeedback();

            if (_currentClicks >= _clickLimit)
            {
                MoveToExitPoint();
            }
        }
    }

    private void ProvideFeedback()
    {
        transform.DOPunchScale(Vector3.one * 0.2f, 0.05f, 10, 1);
    }

    public void MoveToExitPoint()
    {
        transform.DOMove(_movementPoints[2].position, _exitDuration).SetEase(Ease.InQuart).OnComplete(DestroyCar);
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