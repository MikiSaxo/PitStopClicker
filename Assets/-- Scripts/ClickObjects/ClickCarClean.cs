using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class ClickCarClean : ClickObjects
{
    [Header("--- Clean ")]
    [SerializeField] private GameObject _cleanFX;
    [SerializeField] private DecalProjector[] _decalProjector;

    private int _washCount = 0;
    private float _cleanProgress;
    private float _cleanToReach;
    private ParticleSystem _fxToRepair;

    public override void Init(CarMovement myCar, float cleanToReach)
    {
        base.Init(myCar, cleanToReach);

        _cleanToReach = cleanToReach;
        IsActive = true;
        CleanDirty.Instance.NewCarComing(this);
        _cleanProgress = 0;

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

    public override void UpdateCurrentClicks(float value)
    {
        if (CheckIfCleaned() == false)
        {
            _cleanProgress += value;
            UpdateDecalProjector(value / _cleanToReach);
            WashFX(transform.position);
        }
    }

    private void UpdateDecalProjector(float intensity)
    {
        foreach (var decal in _decalProjector)
        {
            decal.fadeFactor -= intensity;
        }
        SetFX();
    }

    public void WashFX(Vector3 pos)
    {
        Instantiate(_cleanFX, pos, _cleanFX.transform.rotation);
        AudioManager.Instance.PlaySound($"CleanGlass{_washCount%2}");
        _washCount++;
    }

    public bool CheckIfCleaned()
    {
        if (_cleanProgress >= _cleanToReach)
        {
            if (!IsRepaired)
            {
                IsRepaired = true;
                AudioManager.Instance.PlaySound("Ding");

                ClickHandler.Instance.CreateFXRepairGood(transform.position);
                _fxToRepair.Stop();
                _myCar.CheckAllRepairing();
            }
        }

        return IsRepaired;
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