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
        _currentLevel = 0;
        UpdatePointsToUpgrade();
        UpdateScreenText();
        PointsManager.Instance.OnPointsUpdated += UpdateScreenText;
    }

    public override void OnMouseDown()
    {
        if (_isOneTimePurchase && _isPurchased) return;

        CheckEnoughMoneyBuy();
    }

    private void CheckEnoughMoneyBuy()
    {
        if (PointsManager.Instance.CanBuy(_pointsToUpgrade))
        {
            BuyUpgrade();
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
        UpdatePointsToUpgrade();
    } 

    public virtual void UpdatePointsToUpgrade()
    {
        foreach (var repair in UpgradeManager.Instance.Repairlvl)
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
                    _pointsToUpgrade = Mathf.RoundToInt(_pointsToUpgrade * 1.5f / 10) * 10;
                    _bonus *= 1.5f;
                }

                UpgradeManager.Instance.CurrentRepairPower[(int)_upgradeType] = _bonus;
                
                break;
            }
        }
    }
    private void UpdateScreenText()
    {
        if (_isPurchased)
        {
            _textPrice.text = $"<color=yellow>---------";
            _textLevel.text = $"<color=yellow>Already Bought";
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
    
    private void CantBuyAnim()
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
}