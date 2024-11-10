using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ClickObjects : MonoBehaviour, IClickable
{
    [Header("--- FX")]
    [SerializeField] protected ParticleSystem _fxPrefab;
    [SerializeField] protected Transform _fxParent;
    
    [Header("--- Type")]
    [SerializeField] protected UpgradeType _myType;
    
    [Header("--- Mecano Points")]
    [SerializeField] private Transform[] _mecanoPoints;

    public Transform[] MecanoPoints => _mecanoPoints;

    public UpgradeType MyType => _myType;
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

    public virtual void UpdateCurrentClicks(float value)
    {
        
    }
    public virtual void SelectMeAnim()
    {
    }

    public virtual void SetFX()
    {
    }

    protected virtual void CantSelectAnim()
    {
        transform.DOKill();

        Vector3 reducedScale = Vector3.one * 0.95f; 

        transform.DOScale(reducedScale, 0.05f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.05f).SetEase(Ease.InOutQuad);
        });
    }
}