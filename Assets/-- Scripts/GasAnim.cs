using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GasAnim : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _targetColor = Color.red;
    [SerializeField] private float _durationBlink = .25f;

    private Color _initialColor;
    private bool _isFilling;

    private void Start()
    {
        _initialColor = _renderer.material.color;
        StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        while (!_isFilling)
        {
            yield return StartCoroutine(LerpColor(_initialColor, _targetColor, _durationBlink));
            yield return StartCoroutine(LerpColor(_targetColor, _initialColor, _durationBlink));
        }
    }

    private IEnumerator LerpColor(Color fromColor, Color toColor, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            _renderer.material.color = Color.Lerp(fromColor, toColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _renderer.material.color = toColor;
    }
    
    public void FillGas(float duration)
    {
        _isFilling = true;
        StopCoroutine(ChangeColorCoroutine());
        
        _renderer.material.color = _targetColor;
        StartCoroutine(LerpColor(_targetColor, _initialColor, duration));
    }
}