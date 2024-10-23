using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEngine : MonoBehaviour, IClickable
{
    [SerializeField] private Transform _smokeFXParent;
    [SerializeField] private ParticleSystem _smokeFXPrefab;

    private CarMovement _myCar;
    private float _smokeIntensity = 1f;
    private int _clickNeeded;
    private int _currentClicks = 0;
    private ParticleSystem _smokeFX;
    private bool _isRepaired;

    public void Init(CarMovement myCar, int clickNeeded)
    {
        _myCar = myCar;
        _clickNeeded = clickNeeded;

        _smokeFX = Instantiate(_smokeFXPrefab, _smokeFXParent);
        SetFX();
        _smokeFX.Play();
    }

    public void OnClicked()
    {
        if( _currentClicks >= _clickNeeded && !_isRepaired)
        {
            _isRepaired = true;
            _myCar.MoveToExitPoint();
            return;
        }
        
        if (_myCar == null || _smokeFX == null) return;

        print("click engine");
        _currentClicks++;
        SetFX();

        if (_smokeIntensity <= 0f)
        {
            _smokeFX.Stop();
        }
    }

    private void SetFX()
    {
        float progress = (float)_currentClicks / _clickNeeded;
        _smokeIntensity = Mathf.Clamp01(1f - progress);

        var main = _smokeFX.main;
        main.startColor = new Color(1f, 1f, 1f, _smokeIntensity);
        main.startSize = Mathf.Lerp(1f, 0.1f, progress);
    }
}