using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    public List<UpgradeInfo> Repairlvl = new List<UpgradeInfo>();

    private void Awake()
    {
        Instance = this;
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
    public List<UpgradePrice> UpgradePrices;
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
