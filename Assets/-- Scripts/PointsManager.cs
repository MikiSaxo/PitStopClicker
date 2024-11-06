using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance;

    [Header("--- Points")]
    [SerializeField] private int _points = 0;

    [SerializeField] private TMP_Text[] _pointsText;
    [SerializeField] private float _textAnimDuration = 0.5f;

    [Header("--- PS")]
    [SerializeField] private GameObject _psPrefab;

    [SerializeField] private Transform _psTarget;

    [Header("--- Timings")]
    [SerializeField] private float _spawnDelay = 0.04f;

    [SerializeField] private float _initialMoveDuration = 0.5f;
    [SerializeField] private float _moveToTargetDuration = 0.5f;
    [SerializeField] private float _delayBetweenMovesToTarget = 0.075f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdatePoints(0);
    }

    public void UpdatePoints(int pointsToAdd)
    {
        int startPoints = _points;
        int targetPoints = _points + pointsToAdd;
        _points = targetPoints;

        StartCoroutine(AnimatePoints(startPoints, targetPoints));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            UpdatePoints(100);
    }

    private IEnumerator AnimatePoints(int startPoints, int endPoints)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _textAnimDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _textAnimDuration);
            int currentPoints = Mathf.RoundToInt(Mathf.Lerp(startPoints, endPoints, t));
            SetPointsText(currentPoints,0);
            SetPointsText(currentPoints,1);
            yield return null;
        }
    }

    private void SetPointsText(int points, int index)
    {
        _pointsText[index].SetText($"{points} PS");
    }

    public void AddPS(Vector3 pos)
    {
        StartCoroutine(SpawnAndAnimatePS(pos));
    }

    private IEnumerator SpawnAndAnimatePS(Vector3 pos)
    {
        List<GameObject> psList = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            float rdn = Random.Range(-1.5f, 1.5f);
            float rdnY = Random.Range(0f, 2f);

            GameObject go = Instantiate(_psPrefab, pos + new Vector3(0, 2f, 0), Quaternion.identity);
            go.transform.DOMove(new Vector3(pos.x + rdn, pos.y + 4f + rdnY, pos.z + rdn), _initialMoveDuration).SetEase(Ease.OutQuad);

            psList.Add(go);

            yield return new WaitForSeconds(_spawnDelay);
        }

        yield return new WaitForSeconds(_initialMoveDuration);

        foreach (var ps in psList)
        {
            ps.transform.DOMove(_psTarget.position, _moveToTargetDuration).SetEase(Ease.InExpo).OnComplete(() =>
            {
                BounceAnim();
                UpdatePoints(10);
                ps.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => { Destroy(ps); });
            });
            yield return new WaitForSeconds(_delayBetweenMovesToTarget);
        }
    }

    private void BounceAnim()
    {
        transform.DOKill();
        Vector3 upScale = Vector3.one * 1.1f;

        transform.DOScale(upScale, 0.05f).SetEase(Ease.InOutQuad).OnComplete(() => { transform.DOScale(Vector3.one, 0.05f).SetEase(Ease.InOutQuad); });
    }
    
    public bool CanBuy(int price)
    {
        if(_points >= price)
        {
            _points -= price;
            UpdatePoints(0);
            
            return true;
        }
        return false;
    }
}