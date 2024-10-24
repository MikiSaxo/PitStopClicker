using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CarSpawner : MonoBehaviour
{
    public static CarSpawner Instance;
    
    [Tooltip("0: spawn, 1: click, 2: exit")]
    [SerializeField] private Transform[] _movementPoints;
    [SerializeField] private GameObject _carPrefab;
    
    public GameObject SaveModel { get; set; }

    
    public UnityAction OnCarDestroyed;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_movementPoints.Length < 3)
        {
            Debug.LogError("Please assign 3 movement points (Spawn, Click, Exit) in the inspector.");
            return;
        }

        SpawnCar();
    }

    private void SpawnCar()
    {
        GameObject newCar = Instantiate(_carPrefab, _movementPoints[0].position, Quaternion.identity);
        CarMovement carMovement = newCar.GetComponent<CarMovement>();

        if (carMovement != null)
        {
            OnCarDestroyed += HandleCarDestroyed;
        }
        
        carMovement.Init(_movementPoints);
    }

    private void HandleCarDestroyed()
    {
        print("car is destroyed");
        SpawnCar();
    }
}