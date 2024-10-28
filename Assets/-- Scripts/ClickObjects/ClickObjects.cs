using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClickObjects : MonoBehaviour, IClickable
{
    [Header("--- FX")]
    [SerializeField] protected ParticleSystem _fxPrefab;
    [SerializeField] protected Transform _fxParent;

    public bool IsActive { get; protected set; }
    public bool IsRepaired { get; protected set; }
    
    protected CarMovement _myCar;
    protected float _initialStartSize = 1f;
    protected int _currentClicks = 0;

    public virtual void Init(CarMovement myCar, int clickNeeded)
    {
        _myCar = myCar;
    }

    public virtual void OnClicked(Vector3 hitPoint)
    {
        if (!IsRepaired)
            ClickHandler.Instance.CreateFXClick(hitPoint);
    }
    
    public virtual void SelectMeAnim()
    {
    }

    public virtual void SetFX()
    {
    }
}