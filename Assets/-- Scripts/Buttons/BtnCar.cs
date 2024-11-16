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
        UpgradeManager.Instance.CurrentCarLevel++;
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
        PlayerPrefs.Save();
    }

    public override void LoadCurrentLevel()
    {
        _currentLevel = PlayerPrefs.GetInt($"Car_CurrentLevel", 0);
        UpgradeManager.Instance.CurrentCarLevel = _currentLevel;
    }
}