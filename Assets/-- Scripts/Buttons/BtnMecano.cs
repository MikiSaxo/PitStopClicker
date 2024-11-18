using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMecano : BtnShop
{
    protected float _power;

    public override void CheckOneTimePurchase()
    {
        if (_currentLevel == 0) return;
        
        if (_isOneTimePurchase)
        {
            _isPurchased = true;
            UpgradeManager.Instance.CurrentMecanoPower[(int)_upgradeType] = 1;
            ClickCarJack.Instance.CheckUpgradeAutoMove();
            GasCan.Instance.CheckUpgradeAutoMove();

            StartCoroutine(WaitActiveMecano());
        }
        else
            UpdatePointsToUpgrade();
    }

    IEnumerator WaitActiveMecano()
    {
        yield return new WaitForSeconds(.2f);

        MecanoManager.Instance.SetActiveMecano(_upgradeType, true);
        if(_boughtable != null)
            MecanoManager.Instance.UpdateMecanoMesh((int)_upgradeType, _currentLevel);
    }
    
    public override void UpdatePointsToUpgrade()
    {
        foreach (var repair in UpgradeManager.Instance.MecanoLvl)
        {
            if(repair.MyUpgradeType == _upgradeType)
            {
                if (_currentLevel < repair.MecanoPrices.Count - 1)
                {
                    _pointsToUpgrade = repair.MecanoPrices[_currentLevel].PriceLevel;
                    _bonus = repair.MecanoPrices[_currentLevel].Speed;
                    _power = repair.MecanoPrices[_currentLevel].Power;
                }
                else
                {
                    _isPurchased = true;
                    _pointsToUpgrade = repair.MecanoPrices[^1].PriceLevel;
                    _bonus = repair.MecanoPrices[^1].Speed;
                    _power = repair.MecanoPrices[^1].Power;
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
    
    public override void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt($"Meca_{_upgradeType}_CurrentLevel", _currentLevel);
        PlayerPrefs.SetInt($"Meca_{_upgradeType}_IsPurchased", _isPurchased ? 1 : 0);
        PlayerPrefs.Save();
    }

    public override void LoadCurrentLevel()
    {
        _currentLevel = PlayerPrefs.GetInt($"Meca_{_upgradeType}_CurrentLevel", 0);
        _isPurchased = PlayerPrefs.GetInt($"Meca_{_upgradeType}_IsPurchased", 0) == 1;
    }
}
