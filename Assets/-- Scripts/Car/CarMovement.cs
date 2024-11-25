using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CarMovement : MonoBehaviour
{
    [Header("--- Car Models")] 
    [SerializeField] private Transform _modelParent;
    [SerializeField] private GameObject[] _models;

    [Header("--- Car Repair")] 
    [SerializeField] private ClickObjects[] _clickObjects;

    [Header("--- Timing")]
    [SerializeField] private float _spawnDuration = 2f;
    [SerializeField] private float _exitDuration = 1f;

    [Header("--- FX Drive")] 
    [SerializeField] private ParticleSystem[] _circuitFX;

    public bool IsAtClickPoint { get; private set; }

    private Transform[] _movementPoints;
    private Vector3 _initClickPos;
    private CarInfo _carInfo;

    public List<ClickObjects> Init(Transform[] movPoints, CarInfo carInfo)
    {
        _carInfo = carInfo;
        _movementPoints = movPoints;
        transform.position = _movementPoints[0].position;
        GetComponent<Collider>().enabled = false;

        _initClickPos = _movementPoints[1].position;

        AddRandomModel();

        List<ClickObjects> clickObjectsList = new List<ClickObjects>();
        clickObjectsList = AddRandomRepair();

        MoveToClickPoint();

        return clickObjectsList;
    }

    public void InitCarGarage()
    {
        foreach (var obj in _clickObjects)
        {
            obj.gameObject.SetActive(false);
        }

        AddRandomModel();
        SetActiveCircuitFX(false);
    }

    private void AddRandomModel()
    {
        int rdn = Random.Range(0, _models.Length);
        GameObject go = Instantiate(_models[rdn], _modelParent);

        CarSpawner.Instance.SaveModel = go;
    }

    private List<ClickObjects> AddRandomRepair()
    {
        foreach (var obj in _clickObjects)
        {
            obj.gameObject.SetActive(false);
        }

        int totalObjects = _clickObjects.Length;
        int randomActiveCount = Random.Range(1, totalObjects - 1);

        List<int> activeIndexes = new List<int>();
        List<ClickObjects> activeObjects = new List<ClickObjects>();

        while (activeIndexes.Count < randomActiveCount)
        {
            int randomIndex = Random.Range(0, totalObjects);
            if (!activeIndexes.Contains(randomIndex))
            {
                activeIndexes.Add(randomIndex);
                _clickObjects[randomIndex].gameObject.SetActive(true);
                _clickObjects[randomIndex].Init(this, CarPowerInit(_clickObjects[randomIndex].MyType));
                activeObjects.Add(_clickObjects[randomIndex]);
            }
        }

        return activeObjects;
    }

    private float CarPowerInit(UpgradeType type)
    {
        float power = 0f;

        switch (type)
        {
            case UpgradeType.Engine:
                power = Random.Range(_carInfo.EngineNbClicks.x, _carInfo.EngineNbClicks.y);
                break;
            case UpgradeType.Tire:
                power = Random.Range(_carInfo.TireNbClicks.x, _carInfo.TireNbClicks.y);
                break;
            case UpgradeType.Wash:
                power = _carInfo.WashCleanValue;
                break;
            case UpgradeType.Gas:
                power = _carInfo.GasDuration;
                break;
        }

        return power;
    }

    private void MoveToClickPoint()
    {
        // float distance = Vector3.Distance(transform.position, _movementPoints[1].position);
        // float linearSpeed = distance / _spawnDuration;
        // _wheelAnim.StartRotation(linearSpeed);

        AudioManager.Instance.PlaySound("CarArriving");

        transform.DOMove(_movementPoints[1].position, _spawnDuration).OnComplete(() =>
        {
            IsAtClickPoint = true;
            SetActiveCircuitFX(false);
            CarSpawner.Instance.OnCarAtClickPoint?.Invoke();
            AudioManager.Instance.PlaySound("EngineLoop");
        });
    }

    public void OnClickFeedback()
    {
        _modelParent.transform.DOKill();
        float randomAngleZ = Random.Range(-20f, 20f);
        _modelParent.transform.DOPunchScale(Vector3.one * 0.05f, 0.05f, 10, 1);
        _modelParent.transform.DORotate(new Vector3(0, 0, randomAngleZ), 0.1f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _modelParent.transform.DOMove(_initClickPos, 0.1f);
                _modelParent.transform.DORotate(new Vector3(0, 180, 0), 0.1f);
                _modelParent.transform.DOScale(Vector3.one, 0.1f);
            });
    }

    public void SetHeightCarJack(bool isUp, float height)
    {
        transform.DOComplete();
        
        if (isUp)
        {
            transform.DOMoveY(transform.position.y + height, 0.25f);
            _initClickPos = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        }
        else
            transform.DOMoveY(transform.position.y - height, 0.1f);
    }

    public void CheckAllRepairing()
    {
        foreach (var obj in _clickObjects)
        {
            if (!obj.IsActive) continue;

            if (!obj.IsRepaired) return;
        }

        ClickCarJack.Instance.ReturnFromJackPoint();
        PointsManager.Instance.AddPS(transform.position);
        CarSpawner.Instance.OnCarRepaired?.Invoke();
        
        AudioManager.Instance.PlaySound("Repair");
        AudioManager.Instance.StopSound("EngineLoop");

        StartCoroutine(MoveToExitPoint());
    }

    private IEnumerator MoveToExitPoint()
    {
        IsAtClickPoint = false;

        // float distance = Vector3.Distance(transform.position, _movementPoints[2].position);
        // float linearSpeed = distance / _exitDuration;
        // _wheelAnim.StartRotation(linearSpeed);

        AudioManager.Instance.PlaySound("CarAccelerate");
        
        SetActiveCircuitFX(true);
        
        //Wait for the CarJack to release the car
        yield return new WaitForSeconds(0.1f);

        transform.DOMove(_movementPoints[2].position, _exitDuration).SetEase(Ease.InQuart).OnComplete(DestroyCar);
    }

    private void DestroyCar()
    {
        CarSpawner.Instance.OnCarDestroyed?.Invoke();
        GoToCircuit();
    }

    private void SetActiveCircuitFX(bool isActive)
    {
        if (isActive)
        {
            foreach (var fx in _circuitFX)
            {
                fx.Play();
            }
        }
        else
        {
            foreach (var fx in _circuitFX)
            {
                fx.Stop();
            }
        }
    }

    private void GoToCircuit()
    {
        foreach (var obj in _clickObjects)
        {
            obj.gameObject.SetActive(false);
        }

        CarSpawnerCircuit.Instance.GoSpawnCar(this.gameObject);
    }
}