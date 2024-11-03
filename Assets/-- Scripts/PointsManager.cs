using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance;

    [SerializeField] private int _points = 0;
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private float _animationDuration = 0.5f; // Duration of the animation

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
        if(Input.GetKeyDown(KeyCode.K))
            UpdatePoints(100);
    }

    private IEnumerator AnimatePoints(int startPoints, int endPoints)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _animationDuration);
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
}