using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private bool _byPass;
    
    [Header("--- Setup")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _logo;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Transform _logoTitle;
    [SerializeField] private Transform _logoTitleTarget;

    [Header("--- Timings")]
    [SerializeField] private float _waitLogoAppear = .5f;
    [SerializeField] private float _fadeLogoDuration = 1f;
    [SerializeField] private float _waitLogoTitle = 1f;
    [SerializeField] private float _fadeTextDuration = 1f;
    [SerializeField] private float _logoTitleMoveDuration = .5f;
    [SerializeField] private float _waitGoGame = 1f;
    [SerializeField] private float _fadeCanvasGroup = .5f;

    private void Start()
    {
        if (!_byPass)
        {
            _canvasGroup.alpha = 1f;
            _logo.DOFade(0, 0);
            _title.DOFade(0, 0);
            StartCoroutine(AnimateLogoAndText());
        }
        else
        {
            _canvasGroup.alpha = 0;
        }
    }

    private IEnumerator AnimateLogoAndText()
    {
        yield return new WaitForSeconds(_waitLogoAppear);
        
        _logo.DOFade(1, _fadeLogoDuration);
        
        yield return new WaitForSeconds(_fadeLogoDuration + _waitLogoTitle);
        
        _logoTitle.DOMoveX(_logoTitleTarget.position.x, _logoTitleMoveDuration).SetEase(Ease.OutSine);
        _title.DOFade(1, _fadeTextDuration);

        yield return new WaitForSeconds(_fadeTextDuration + _waitGoGame);
        
        _canvasGroup.DOFade(0, _fadeCanvasGroup).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
