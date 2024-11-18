using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private List<UpgradeInfo> _repairLvl = new List<UpgradeInfo>();
    [SerializeField] private List<MecanoInfo> _mecanoLvl = new List<MecanoInfo>();
    [SerializeField] private List<CarInfo> _carsLvl = new List<CarInfo>();

    [HideInInspector] public List<float> CurrentRepairPower = new List<float>();
    [HideInInspector] public List<float> CurrentMecanoPower = new List<float>();
    [HideInInspector] public List<float> CurrentMecanoSpeed = new List<float>();

    public List<UpgradeInfo> RepairLvl => _repairLvl;
    public List<MecanoInfo> MecanoLvl => _mecanoLvl;
    
    public List<CarInfo> CarsLvl => _carsLvl;
    public int CurrentCarLevel { get; set; }

    private void Awake()
    {
        Instance = this;

        foreach (var repair in _repairLvl)
        {
            CurrentRepairPower.Add(repair.UpgradePrices[0].Bonus);
        }

        for (int i = 0; i < _mecanoLvl.Count + 2; i++)
        {
            CurrentMecanoPower.Add(0);
            CurrentMecanoSpeed.Add(0);
        }
    }
    
    public int GetCurrentMoneyWin()
    {
        if(CurrentCarLevel >= _carsLvl.Count)
            return _carsLvl[^1].MoneyWin;
        
        return _carsLvl[CurrentCarLevel].MoneyWin;
    }
    
    public CarInfo GetCurrentCarInfo()
    {
        if(CurrentCarLevel >= _carsLvl.Count)
            return _carsLvl[^1];
        
        return _carsLvl[CurrentCarLevel];
    }
}

[System.Serializable]
public class UpgradePrice
{
    public int PriceLevel;
    public float Bonus;
}

[System.Serializable]
public class UpgradeInfo
{
    public UpgradeType MyUpgradeType;
    public List<UpgradePrice> UpgradePrices = new List<UpgradePrice>();
}

[System.Serializable]
public class MecanoInfo
{
    public UpgradeType MyUpgradeType;
    public List<MecanoPrice> MecanoPrices = new List<MecanoPrice>();
}

[System.Serializable]
public class MecanoPrice
{
    public int PriceLevel;
    public float Speed;
    public float Power;
    public Mesh MecanoMesh;
    public Material MecanoMaterial;
}

[System.Serializable]
public class CarInfo
{
    public GameObject CarPrefab;
    [Header("--- Repair")]
    public Vector2 EngineNbClicks;
    public Vector2 TireNbClicks;
    public float WashCleanValue;
    public float GasDuration;
    [Header("--- Money")]
    public int MoneyWin;
    public int BuyPrice;
    
}

public enum UpgradeType
{
    Tire,
    Engine,
    Wash,
    Gas,
    AutoGas,
    AutoCarJack
}