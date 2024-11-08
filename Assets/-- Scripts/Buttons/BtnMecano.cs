using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMecano : BtnShop
{
    public override void CheckOneTimePurchase()
    {
        if (_isOneTimePurchase)
        {
            _isPurchased = true;
            UpgradeManager.Instance.CurrentRepairPower[(int)_upgradeType] = 1;
            ClickCarJack.Instance.CheckUpgradeAutoMove();
            GasCan.Instance.CheckUpgradeAutoMove();
        }
        else
            UpdatePointsToUpgrade();
    }
    
    public override void UpdatePointsToUpgrade()
    {
        foreach (var repair in UpgradeManager.Instance.MecanoLvl)
        {
            if(repair.MyUpgradeType == _upgradeType)
            {
                _pointsToUpgrade = repair.UpgradePrices[_currentLevel].PriceLevel;
                _bonusPower = repair.UpgradePrices[_currentLevel].Bonus;
                break;
            }
        }
    }
}
