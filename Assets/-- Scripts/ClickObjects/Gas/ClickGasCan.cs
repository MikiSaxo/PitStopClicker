using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGasCan : ClickObjects
{
    public float FillDurationCar { get; set; }
    private GasAnim _gasAnim;
    private void Start()
    {
        _gasAnim = GetComponent<GasAnim>();
    }

    public override void Init(CarMovement myCar, float fillDurationCar)
    {
        base.Init(myCar, fillDurationCar);

        FillDurationCar = fillDurationCar;
        IsActive = true;
        GasCan.Instance.ClickGasCan = this;
    }

    public override void OnClicked(Vector3 hitPoint)
    {
        if (!CanClick()) return;
        
        if (ClickCarJack.Instance.IsSet == false)
        {
            ClickCarJack.Instance.SelectMeAnim();
            return;
        }

        GasCan.Instance.SelectMeAnim();
    }

    public void LaunchGasAnim(float duration)
    {
        _gasAnim.FillGas(duration);

        StartCoroutine(WaitToFill(duration));
    }

    private IEnumerator WaitToFill(float duration)
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