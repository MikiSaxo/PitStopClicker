using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private int _clickLimit = 5;

    [Header("--- Timing")]
    [SerializeField] private float _spawnDuration = 2f;
    [SerializeField] private float _exitDuration = 1f;

    [Header("--- Models")]
    [SerializeField] private Transform _modelParent;
    [SerializeField] private GameObject[] _models;
    [SerializeField] private ClickObjects[] _clickObjects;
    
    [Header("--- FX")]
    [SerializeField] private GameObject[] _circuitFX;

    private Transform[] _movementPoints;
    private bool _isAtClickPoint = false;
    
    private Vector3 _initClickPos;
    private Quaternion _initClickRota;

    public void Init(Transform[] movPoints)
    {
        _movementPoints = movPoints;
        transform.position = _movementPoints[0].position;

        AddRandomModel();
        AddRandomRepair();
        MoveToClickPoint();

        _clickLimit = 5;
    }

    private void AddRandomModel()
    {
        int rdn = Random.Range(0, _models.Length);
        CarSpawner.Instance.SaveModel = Instantiate(_models[rdn], _modelParent);
    }

    private void AddRandomRepair()
    {
        foreach (var obj in _clickObjects)
        {
            obj.gameObject.SetActive(false);
        }

        int totalObjects = _clickObjects.Length;
        int randomActiveCount = Random.Range(1, totalObjects + 1);

        List<int> activeIndexes = new List<int>();

        while (activeIndexes.Count < randomActiveCount)
        {
            int randomIndex = Random.Range(0, totalObjects);
            if (!activeIndexes.Contains(randomIndex))
            {
                activeIndexes.Add(randomIndex);
                _clickObjects[randomIndex].gameObject.SetActive(true);
                _clickObjects[randomIndex].Init(this, _clickLimit);
            }
        }
    }
    
    private void MoveToClickPoint()
    {
        transform.DOMove(_movementPoints[1].position, _spawnDuration).OnComplete(() =>
        {
            _isAtClickPoint = true;
            _initClickRota = transform.rotation;
            _initClickPos = transform.position;
        });
    }

    public void OnClickFeedback()
    {
        transform.DOKill();
        float randomAngle = Random.Range(-20f, 20f); // Random rotation angle between -30 and 30 degrees
        transform.DOPunchScale(Vector3.one * 0.05f, 0.05f, 10, 1);
        transform.DORotate(new Vector3(0, 0, randomAngle), 0.1f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                transform.DOMove(_initClickPos, 0.1f);
                transform.DORotateQuaternion(_initClickRota, 0.1f);
            });
    }

    public void CheckAllRepairing()
    {
        foreach (var obj in _clickObjects)
        {
            if (!obj.IsActive) continue;

            if (!obj.IsRepaired) return;
        }

        MoveToExitPoint();
    }

    #region GoDeath

    private void MoveToExitPoint()
    {
        transform.DOMove(_movementPoints[2].position, _exitDuration).SetEase(Ease.InQuart).OnComplete(DestroyCar);
    }

    private void DestroyCar()
    {
        CarSpawner.Instance.OnCarDestroyed?.Invoke();
        GoToCircuit();
    }


    private void GoToCircuit()
    {
        foreach (var fx in _circuitFX)
        {
            fx.gameObject.SetActive(true);
        }

        foreach (var obj in _clickObjects)
        {
            obj.gameObject.SetActive(false);
        }
        
        CarSpawnerCircuit.Instance.GoSpawnCar(this.gameObject);
    }

    #endregion
}