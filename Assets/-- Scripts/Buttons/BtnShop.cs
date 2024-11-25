using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BtnShop : BtnScreen
{
    [SerializeField] protected bool _isOneTimePurchase;
    [SerializeField] protected UpgradeType _upgradeType;

    [Header("--- Texts")] 
    [SerializeField] private TMP_Text _textPrice;
    [SerializeField] private TMP_Text _textLevel;
    [SerializeField] private TMP_Text _textBtn;
    [SerializeField] private Color[] _colorCanBuy;
    
    [Header("--- Boughtable")] 
    [SerializeField] protected BoughtableAnim _boughtable;

    protected int _currentLevel = 0;
    protected int _pointsToUpgrade = 100;
    protected float _bonus;
    protected bool _isPurchased;

    public override void Start()
    {
        base.Start();
        LoadCurrentLevel();
        CheckOneTimePurchase();
        UpdatePointsToUpgrade();
        UpdateScreenText();
        PointsManager.Instance.OnPointsUpdated += UpdateScreenText;
    }

    public override void OnMouseDown()
    {
        AudioManager.Instance.PlaySound("Pop");
        if (_isOneTimePurchase && _isPurchased) return;

        CheckEnoughMoneyBuy();
    }

    private void CheckEnoughMoneyBuy()
    {
        if (PointsManager.Instance.CanBuy(_pointsToUpgrade) && !_isPurchased)
        {
            BuyUpgrade();
            AudioManager.Instance.PlaySound("Buy");
        }
        else
        {
            CantBuyAnim();
        }
    }

    public virtual void BuyUpgrade()
    {
        PointsManager.Instance.UpdatePoints(-_pointsToUpgrade);
            
        _currentLevel++;
        
        CheckOneTimePurchase();
        SaveCurrentLevel();
        base.OnMouseDown();
         
        UpdateScreenText();
        
        if (_boughtable != null)
        {
            _boughtable.BuyAnim();
            ClickHandler.Instance.CreateFXRepairGood(_boughtable.transform.position);
        }
    }
    
    public virtual void CheckOneTimePurchase()
    { 
        if (_currentLevel == 0) return;

        UpdatePointsToUpgrade();
    } 

    public virtual void UpdatePointsToUpgrade()
    {
        foreach (var repair in UpgradeManager.Instance.RepairLvl)
        {
            if(repair.MyUpgradeType == _upgradeType)
            {
                if (_currentLevel < repair.UpgradePrices.Count)
                {
                    _pointsToUpgrade = repair.UpgradePrices[_currentLevel].PriceLevel;
                    _bonus = repair.UpgradePrices[_currentLevel].Bonus;
                }
                else
                {
                    _currentLevel--;
                    _isPurchased = true;
                }

                UpgradeManager.Instance.CurrentRepairPower[(int)_upgradeType] = _bonus;
                
                break;
            }
        }
    }
    protected void UpdateScreenText()
    {
        if (_isPurchased)
        {
            _textPrice.text = $"<color=yellow>---------";
            _textLevel.text = $"lvl. <color=yellow>Max";
            _textBtn.text = $"<color=yellow>Owned";
        }
        else
        {
            _textPrice.text = PointsManager.Instance.CanBuy(_pointsToUpgrade) 
                ? $"<color=#{ColorUtility.ToHtmlStringRGBA(_colorCanBuy[0])}>{_pointsToUpgrade} PS" 
                : $"<color=#{ColorUtility.ToHtmlStringRGBA(_colorCanBuy[1])}>{_pointsToUpgrade} PS";

            _textLevel.text = !_isOneTimePurchase ? $"lvl. {_currentLevel+1}" : $"";
        }
    }
    
    protected void CantBuyAnim()
    {
        _textPrice.transform.DOKill();
        transform.DOLocalRotate(new Vector3(0, 0, 10), 0.1f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            transform.DOLocalRotate(_initRota, .25f);
        });
    }

    private void OnDisable()
    {
        PointsManager.Instance.OnPointsUpdated -= UpdateScreenText;
    }
    
    public virtual void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt($"Click_{_upgradeType}_CurrentLevel", _currentLevel);
        PlayerPrefs.SetInt($"Click_{_upgradeType}_IsPurchased", _isPurchased ? 1 : 0);
        PlayerPrefs.Save();
    }

    public virtual void LoadCurrentLevel()
    {
        _currentLevel = PlayerPrefs.GetInt($"Click_{_upgradeType}_CurrentLevel", 0);
        _isPurchased = PlayerPrefs.GetInt($"Click_{_upgradeType}_IsPurchased", 0) == 1;
    }
}