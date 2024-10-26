using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCarRepair : ClickObjects
{
    private CarMovement _myCar;
    private float _smokeIntensity = 1f;
    private int _clickNeeded;
    private ParticleSystem _smokeFX;
    private float _initialStartSize;

    public override void Init(CarMovement myCar, int clickNeeded)
    {
        _myCar = myCar;
        _clickNeeded = clickNeeded;

        _smokeFX = Instantiate(_fxPrefab, _fxParent);

        var main = _smokeFX.main;
        _initialStartSize = main.startSize.constant;

        SetFX();
        _smokeFX.Play();
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

        _currentClicks++;

        SetFX();
        _myCar.OnClickFeedback();

        if (_smokeIntensity <= 0f)
        {
            _smokeFX.Stop();
        }

        if (_currentClicks >= _clickNeeded && !IsRepaired)
        {
            IsRepaired = true;
            ClickHandler.Instance.CreateFXRepairGood(transform.position);
            _myCar.CheckAllRepairing();
        }
    }

    public override void SetFX()
    {
        float progress = (float)_currentClicks / _clickNeeded;
        _smokeIntensity = Mathf.Clamp01(1f - progress);

        var main = _smokeFX.main;
        main.startSize = Mathf.Lerp(_initialStartSize, 0.1f, progress);
    }

    private bool CanClick()
    {
        return _myCar != null
               && _smokeFX != null
               && IsActive
               && !IsRepaired
               && _myCar.IsAtClickPoint;
    }
}