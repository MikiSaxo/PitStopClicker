using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<UpgradeInfo> Repairlvl = new List<UpgradeInfo>();
    public List<MecanoInfo> MecanoLvl = new List<MecanoInfo>();

    [Space(50)]

    public List<float> CurrentRepairPower = new List<float>();
    public List<float> CurrentMecanoPower = new List<float>();
    public List<float> CurrentMecanoSpeed = new List<float>();

    private void Awake()
    {
        Instance = this;

        foreach (var repair in Repairlvl)
        {
            CurrentRepairPower.Add(repair.UpgradePrices[0].Bonus);
        }

        for (int i = 0; i < MecanoLvl.Count + 1; i++)
        {
            CurrentMecanoPower.Add(0);
            CurrentMecanoSpeed.Add(0);
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

public enum UpgradeType
{
    Tire,
    Engine,
    Wash,
    Gas,
    AutoGas,
    AutoCarJack
}