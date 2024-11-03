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

    [SerializeField] private TMP_Text _pointsText;
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
        SetPointsText(_points);
    }

    public void UpdatePoints(int pointsToAdd)
    {
        int targetPoints = _points + pointsToAdd;
        StartCoroutine(AnimatePoints(_points, targetPoints));
        _points = targetPoints;
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
            SetPointsText(currentPoints);
            yield return null;
        }

        SetPointsText(endPoints);
    }

    private void SetPointsText(int points)
    {
        _pointsText.SetText($"{points} PS");
    }

    public void CreatePS(Vector3 pos)
    {
        StartCoroutine(SpawnAndAnimatePS(pos));
    }

    private IEnumerator SpawnAndAnimatePS(Vector3 pos)
    {
        List<GameObject> psList = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            float rdnZ = Random.Range(-1.5f, 1.5f);
            float rdnY = Random.Range(0f, 2f);

            GameObject go = Instantiate(_psPrefab, pos, Quaternion.identity);
            go.transform.DOMove(new Vector3(pos.x, pos.y + 4f + rdnY, pos.z + rdnZ), _initialMoveDuration).SetEase(Ease.OutQuad);

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
}