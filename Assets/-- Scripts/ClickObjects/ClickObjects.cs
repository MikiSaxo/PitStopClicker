using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObjects : MonoBehaviour, IClickable
{
    [Header("--- FX")]
    [SerializeField] protected ParticleSystem _fxPrefab;
    [SerializeField] protected Transform _fxParent;
    
    public bool IsRepaired { get; protected set; }
    public bool IsActive { get; protected set; }

    protected int _currentClicks = 0;


    public virtual void Init(CarMovement myCar, int clickNeeded)
    {
    }

    public virtual void OnClicked(Vector3 hitPoint)
    {
        if (!IsRepaired)
            ClickHandler.Instance.CreateFXClick(hitPoint);
    }

    public virtual void SetFX()
    {
    }
}