using System.Collections;
using UnityEngine;

public class GasAnim : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _targetColor = Color.red;
    [SerializeField] private float _duration = 1f;

    private Color _initialColor;

    private void Start()
    {
        _initialColor = _renderer.material.color;
        StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(LerpColor(_initialColor, _targetColor, _duration));
            yield return StartCoroutine(LerpColor(_targetColor, _initialColor, _duration));
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
}