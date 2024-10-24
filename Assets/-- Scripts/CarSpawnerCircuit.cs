using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CarSpawnerCircuit : MonoBehaviour
{
    public static CarSpawnerCircuit Instance;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _moveDuration = 5f;
    
    private void Awake()
    {
        Instance = this;
    }

    // private void Start()
    // {
    //     CarSpawner.Instance.OnCarDestroyed += SpawnCar;
    // }

    public void GoSpawnCar(GameObject car)
    {
        // GameObject spawnedCar = Instantiate(CarSpawner.Instance.SaveModel, _spawnPoint.position, _spawnPoint.rotation);
        car.transform.position = _spawnPoint.position;
        car.transform.rotation = _spawnPoint.rotation;
        MoveCarToEndPoint(car);
    }

    private void MoveCarToEndPoint(GameObject car)
    {
        car.transform.DOMove(_endPoint.position, _moveDuration)
            .SetEase(Ease.InSine)
            .OnComplete(() => OnCarReachedEndPoint(car));
    }

    private void OnCarReachedEndPoint(GameObject car)
    {
        Destroy(car);
    }
}