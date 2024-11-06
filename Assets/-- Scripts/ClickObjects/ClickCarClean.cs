using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class ClickCarClean : ClickObjects
{
    [SerializeField] private DecalProjector _decalProjector;
    
    private ParticleSystem _fxToRepair;
    
    public override void Init(CarMovement myCar, int clickNeeded)
    {
        base.Init(myCar, clickNeeded);

        IsActive = true;
        CleanDirty.Instance.NewCarComing(this);
        
        _fxToRepair = Instantiate(_fxPrefab, _fxParent);
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if (!CanClick()) return;
        
        if (ClickCarJack.Instance.IsSet == false)
        {
            ClickCarJack.Instance.SelectMeAnim();
            return;
        }
        
        CleanDirty.Instance.SelectMeAnim();
    }

    public void UpdateDecalProjector(float intensity)
    {
        _decalProjector.fadeFactor -= intensity;
        SetFX();
    }

    public void CleanFinished()
    {
        IsRepaired = true;
        _fxToRepair.Stop();
        _myCar.CheckAllRepairing();
    }
    
    public override void SetFX()
    {
        float progress = 1 / (float)_currentClicks;
        var main = _fxToRepair.main;
        main.startSize = Mathf.Lerp(_initialStartSize, 0.1f, progress);
    }

    private bool CanClick()
    {
        return _myCar != null
               && IsActive
               && !IsRepaired
               && _myCar.IsAtClickPoint;
    }
}