using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGasCan : ClickObjects
{
    private CarMovement _myCar;

    public override void Init(CarMovement myCar, int clickNeeded)
    {
        _myCar = myCar;
        IsActive = true;
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if (!CanClick()) return;

        base.OnClicked(hitPoint);

        GasCan.Instance.MoveToGasPoint(transform);
    }
    
    private bool CanClick()
    {
        return _myCar != null
               && IsActive
               && !IsRepaired
               && _myCar.IsAtClickPoint;
    }
}