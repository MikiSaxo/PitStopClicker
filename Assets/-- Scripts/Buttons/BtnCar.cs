using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BtnCar : BtnShop
{
    private GameObject _car;
    
    private void Awake()
    {
        _car = _boughtable.gameObject;
    }

    public override void OnMouseDown()
    {
        if (PointsManager.Instance.CanBuy(_pointsToUpgrade) == false && _isPurchased == false)
        {
            CantBuyAnim();
            return;
        }

        UpgradeManager.Instance.CurrentCarLevel++;

        print($"car lvl : {UpgradeManager.Instance.CurrentCarLevel} - car count : {UpgradeManager.Instance.CarsLvl.Count}");
        if (UpgradeManager.Instance.CurrentCarLevel >= UpgradeManager.Instance.CarsLvl.Count-1)
        {
            _isPurchased = true;
            BuyUpgrade();
        }
        
        base.OnMouseDown();
    }

    public override void UpdatePointsToUpgrade()
    {
        _pointsToUpgrade = UpgradeManager.Instance.GetCurrentCarInfo().BuyPrice;
        
        GameObject newCar = Instantiate(UpgradeManager.Instance.GetCurrentCarInfo().CarPrefab, _car.transform.position, _car.transform.rotation);
        newCar.GetComponent<CarMovement>().InitCarGarage();
        newCar.transform.DORotate(new Vector3(0, -90, 0), 0);
        
        _boughtable = newCar.AddComponent<BoughtableAnim>();
        
        Destroy(_car);

        _car = newCar;
    }
    
    public override void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt($"Car_CurrentLevel", _currentLevel);
        PlayerPrefs.SetInt($"Car_IsPurchased", _isPurchased ? 1 : 0);
        PlayerPrefs.Save();
    }

    public override void LoadCurrentLevel()
    {
        _currentLevel = PlayerPrefs.GetInt($"Car_CurrentLevel", 0);
        _isPurchased = PlayerPrefs.GetInt($"Car_IsPurchased", 0) == 1;
        UpgradeManager.Instance.CurrentCarLevel = _currentLevel;
    }
}