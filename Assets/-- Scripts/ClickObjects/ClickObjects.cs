using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObjects : MonoBehaviour, IClickable
{
    [Header("--- FX")]
    [SerializeField] protected ParticleSystem _fxPrefab;
    [SerializeField] protected Transform _fxParent;

    protected CarMovement _myCar;
    
    public bool IsRepaired { get; protected set; }
    public bool IsActive { get; protected set; }

    protected int _currentClicks = 0;
    protected bool _isClicked;


    public virtual void Init(CarMovement myCar, int clickNeeded)
    {
        _myCar = myCar;
    }

    public virtual void OnClicked(Vector3 hitPoint)
    {
        _isClicked = true;
        
        if (!IsRepaired)
            ClickHandler.Instance.CreateFXClick(hitPoint);
    }

    public virtual void OnClickedUp()
    {
        _isClicked = false;
    }

    public virtual void SetFX()
    {
    }
}