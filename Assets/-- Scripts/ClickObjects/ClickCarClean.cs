using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickCarClean : ClickObjects
{
    private int _requiredScrubs = 10;
    private int _currentScrubs = 0;

    
    public override void Init(CarMovement myCar, int clickNeeded)
    {
        base.Init(myCar, clickNeeded);

        IsActive = true;
        CleanDirty.Instance.ClickCarClean = this;
    }
    public override void OnClicked(Vector3 hitPoint)
    {
        if (!CanClick()) return;

        //base.OnClicked(hitPoint);

        // if (!CleanDirty.Instance.IsSet)
            // CleanDirty.Instance.MoveToCleanPoint();
    }

    public void LaunchCleanAnim(float duration)
    {
        StartCoroutine(WaitToClean(duration));
    }

    private IEnumerator WaitToClean(float duration)
    {
        yield return new WaitForSeconds(duration);

        IsRepaired = true;
        _myCar.CheckAllRepairing();
    }

    private bool CanClick()
    {
        return _myCar != null
               && IsActive
               && !IsRepaired
               && _myCar.IsAtClickPoint;
    }
}