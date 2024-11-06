using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BtnShop : BtnScreen
{
    [SerializeField] private bool _isOneTimePurchase;
    [FormerlySerializedAs("_updateType")] [SerializeField] private UpgradeType _upgradeType;

    [Header("--- Texts")] 
    [SerializeField] private TMP_Text _textPrice;
    [SerializeField] private TMP_Text _textLevel;
    [SerializeField] private TMP_Text _textBtn;

    private int _currentLevel = 0;
    private int _pointsToUpgrade = 100;
    private float _bonusPower;
    private bool _isPurchased;

    public override void Start()
    {
        base.Start();
        _currentLevel = 0;
        UpdatePointsToUpgrade();
        UpdateScreenText();
    }

    public override void OnMouseDown()
    {
        if (_isOneTimePurchase && _isPurchased) return;

        CheckEnoughMoney();
    }

    private void CheckEnoughMoney()
    {
        if (PointsManager.Instance.CanBuy(_pointsToUpgrade))
        {
            if (_isOneTimePurchase)
                _isPurchased = true;
            else
                UpdatePointsToUpgrade();
         
            _currentLevel++;
            base.OnMouseDown();
            UpdateScreenText();
            ClickHandler.Instance.CreateFXRepairGood(transform.position);
            
            CarSpawner.Instance.CurrentCar.UpdateClickPower(_upgradeType, _bonusPower);
        }
        else
        {
            CantBuyAnim();
        }
    }

    private void UpdatePointsToUpgrade()
    {
        foreach (var repair in UpgradeManager.Instance.Repairlvl)
        {
            if(repair.MyUpgradeType == _upgradeType)
            {
                _pointsToUpgrade = repair.UpgradePrices[_currentLevel].PriceLevel;
                _bonusPower = repair.UpgradePrices[_currentLevel].Bonus;
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
            _textPrice.text = $"<color=green>{_pointsToUpgrade} PS";
            
            if(!_isOneTimePurchase)
                _textLevel.text = $"lvl. {_currentLevel + 1}";
            else
                _textLevel.text = $"";
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
}