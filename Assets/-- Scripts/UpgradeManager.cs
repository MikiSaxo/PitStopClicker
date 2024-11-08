using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    public List<UpgradeInfo> Repairlvl = new List<UpgradeInfo>();
    public List<MecanoInfo> MecanoLvl = new List<MecanoInfo>();
     
    [HideInInspector] public List<float> CurrentRepairPower = new List<float>();
    [HideInInspector] public List<float> CurrentMecanoPower = new List<float>();

    private void Awake()
    {
        Instance = this;
       
        foreach (var repair in Repairlvl)
        {
            CurrentRepairPower.Add(repair.UpgradePrices[0].Bonus);
        }
        
        foreach (var mecano in MecanoLvl)
        {
            CurrentMecanoPower.Add(mecano.UpgradePrices[0].Bonus);
        }
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
    public List<MecanoPrice> UpgradePrices = new List<MecanoPrice>();
}

[System.Serializable]
public class MecanoPrice
{
    public int PriceLevel;
    public float Bonus;
    public Mesh MecanoMesh;
    public Material MecanoMaterial;
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
