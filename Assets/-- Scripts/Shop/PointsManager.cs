using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance;

    [Header("--- Points")]
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
    
    public Action OnPointsUpdated;
    
    private int _currentPoints = 0;

    private void Awake()
    {
        Instance = this;
        LoadPoints();
    }

    private void Start()
    {
        UpdatePoints(0);
    }

    public void UpdatePoints(int pointsToAdd)
    {
        int startPoints = _currentPoints;
        int targetPoints = _currentPoints + pointsToAdd;
        _currentPoints = targetPoints;

        OnPointsUpdated?.Invoke();

        StartCoroutine(AnimatePoints(startPoints, targetPoints));
        SavePoints();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            UpdatePoints(10000);
    }

    private IEnumerator AnimatePoints(int startPoints, int endPoints)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _textAnimDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _textAnimDuration);
            int currentPoints = Mathf.RoundToInt(Mathf.Lerp(startPoints, endPoints, t));

            for (int i = 0; i < _pointsText.Length; i++)
            {
                SetPointsText(currentPoints, i);
            }
            
            yield return null;
        }
    }

    private void SetPointsText(int points, int index)
    {
        _pointsText[index].SetText($"{points}");
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

            AudioManager.Instance.PlaySound("PiecePop");
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
                UpdatePoints((int)(UpgradeManager.Instance.GetCurrentMoneyWin() * .1f));
                ps.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => { Destroy(ps); });
            });
            yield return new WaitForSeconds(_delayBetweenMovesToTarget);
            AudioManager.Instance.PlaySound("CoinCollect");
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
        if(_currentPoints >= price)
        {
            return true;
        }
        return false;
    }
    
    private void OnApplicationQuit()
    {
        SavePoints();
    }

    public void SavePoints()
    {
        PlayerPrefs.SetInt("CurrentPoints", _currentPoints);
        PlayerPrefs.Save();
    }

    public void LoadPoints()
    {
        _currentPoints = PlayerPrefs.GetInt("CurrentPoints", 0);
        UpdatePoints(0); // Update the UI with the loaded points
    }
}