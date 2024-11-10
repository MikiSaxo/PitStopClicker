using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMecano : BtnShop
{
    protected float _power;

    public override void CheckOneTimePurchase()
    {
        if (_isOneTimePurchase)
        {
            _isPurchased = true;
            UpgradeManager.Instance.CurrentMecanoPower[(int)_upgradeType] = 1;
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
                if (_currentLevel < repair.MecanoPrices.Count)
                {
                    _pointsToUpgrade = repair.MecanoPrices[_currentLevel].PriceLevel;
                    _bonus = repair.MecanoPrices[_currentLevel].Speed;
                    _power = repair.MecanoPrices[_currentLevel].Power;
                }
                else
                {
                    _pointsToUpgrade = Mathf.RoundToInt(_pointsToUpgrade * 1.5f / 10) * 10;
                    _bonus *= 1.5f;
                    _power *= 1.5f;
                }
                UpgradeManager.Instance.CurrentMecanoSpeed[(int)_upgradeType] = _bonus;
                UpgradeManager.Instance.CurrentMecanoPower[(int)_upgradeType] = _power;
                
                break;
            }
        }
    }

    public override void BuyUpgrade()
    {
        base.BuyUpgrade();

        MecanoManager.Instance.SetActiveMecano(_upgradeType, true);
        
        if(_boughtable != null)
            MecanoManager.Instance.UpdateMecanoMesh((int)_upgradeType, _currentLevel);
    }
}
