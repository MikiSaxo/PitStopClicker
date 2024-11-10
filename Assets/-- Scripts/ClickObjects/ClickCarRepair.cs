using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCarRepair : ClickObjects
{
    private float _fxIntensity = 1f;
    private float _power => UpgradeManager.Instance.CurrentRepairPower[(int)_myType];
    private int _clickNeeded;
    private ParticleSystem _fxToRepair;


    public override void Init(CarMovement myCar, int clickNeeded)
    {
        base.Init(myCar, clickNeeded);
        _clickNeeded = clickNeeded;

        _fxToRepair = Instantiate(_fxPrefab, _fxParent);

        var main = _fxToRepair.main;
        _initialStartSize = main.startSize.constant;

        SetFX();
        _fxToRepair.Play();
        IsActive = true;
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if (!CanClick()) return;

        if (!ClickCarJack.Instance.IsSet)
        {
            ClickCarJack.Instance.SelectMeAnim();
            return;
        }
        
        base.OnClicked(hitPoint);

        UpdateCurrentClicks(_power);
    }
    
    public override void UpdateCurrentClicks(float value)
    {
        _currentClicks += (int)value;
        SetFX();
        _myCar.OnClickFeedback();

        CheckIsRepaired();
    }

    private void CheckIsRepaired()
    {
        if (_currentClicks >= _clickNeeded && !IsRepaired)
        {
            IsRepaired = true;
            ClickHandler.Instance.CreateFXRepairGood(transform.position);
            _myCar.CheckAllRepairing();
        }
        
        if (_fxIntensity <= 0f)
        {
            _fxToRepair.Stop();
        }
    }

    public override void SetFX()
    {
        float progress = (float)_currentClicks / _clickNeeded;
        _fxIntensity = Mathf.Clamp01(1f - progress);

        var main = _fxToRepair.main;
        main.startSize = Mathf.Lerp(_initialStartSize, 0.1f, progress);
    }

    private bool CanClick()
    {
        return _myCar != null
               && _fxToRepair != null
               && IsActive
               && !IsRepaired
               && _myCar.IsAtClickPoint;
    }
}