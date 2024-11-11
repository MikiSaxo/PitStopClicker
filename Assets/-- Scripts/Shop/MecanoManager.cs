using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MecanoManager : MonoBehaviour
{
    public static MecanoManager Instance;

    [SerializeField] private List<MecanoObj> _mecanoObjs = new List<MecanoObj>();

    private List<MecanoAnim> _mecanoAnimsActivated = new List<MecanoAnim>();
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < _mecanoObjs.Count; i++)
        {
            UpdateMecanoMesh(i, 0);
            SetActiveMecano((UpgradeType)i, false);
        }
        
        ClickCarJack.Instance.OnCarJackSet += LaunchMecaAnim;
        CarSpawner.Instance.OnCarRepaired += StopMecaAnim;
    }

    public void UpdateMecanoMesh(int index, int level)
    {
        if (_mecanoObjs[index] == null
            || UpgradeManager.Instance.MecanoLvl[index].MecanoPrices.Count == 0) return;

        var lvlCount = UpgradeManager.Instance.MecanoLvl[index].MecanoPrices.Count;
        // if max level is reached
        if (level >= lvlCount)
            level = lvlCount - 1;

        var mecaMesh = UpgradeManager.Instance.MecanoLvl[index].MecanoPrices[level].MecanoMesh;
        var mecaMaterial = UpgradeManager.Instance.MecanoLvl[index].MecanoPrices[level].MecanoMaterial;
        
        foreach (var mecaMR in _mecanoObjs[index].MecanoGarage)
        {
            mecaMR.GetComponent<MeshFilter>().sharedMesh = mecaMesh;
            mecaMR.material = mecaMaterial;
        }

        foreach (var mecaAnim in _mecanoObjs[index].MecanoAnim)
        {
            mecaAnim.UpdateMeshRenderer(mecaMesh, mecaMaterial);
        }
    }

    public void SetActiveMecano(UpgradeType type, bool active)
    {
        foreach (var mecanoMesh in _mecanoObjs)
        {
            // Check same Type
            if (mecanoMesh.MecanoType == type)
            {
                // Check if there is any MecanoAnim
                if (mecanoMesh.MecanoAnim == null || mecanoMesh.MecanoAnim.Length == 0) return;

                foreach (var mecano in mecanoMesh.MecanoAnim)
                {
                    mecano.gameObject.SetActive(active);

                    // If active, launch anim
                    if (active && mecano != null)
                    {
                        _mecanoAnimsActivated.Add(mecano);
                        mecano.LaunchAnim();
                    }
                }

                break;
            }
        }
    }

    private void LaunchMecaAnim()
    {
        if(_mecanoAnimsActivated.Count == 0) return;

        foreach (var mecanoAnim in _mecanoAnimsActivated)
        {
            mecanoAnim.LaunchAnim();
        }
    }

    private void StopMecaAnim()
    {
        if(_mecanoAnimsActivated.Count == 0) return;

        foreach (var mecanoAnim in _mecanoAnimsActivated)
        {
            mecanoAnim.StopAnim();
        }
    }

    private void OnDisable()
    {
        ClickCarJack.Instance.OnCarJackSet -= LaunchMecaAnim;
        CarSpawner.Instance.OnCarRepaired -= StopMecaAnim;
    }
}

[System.Serializable]
public class MecanoObj
{
    public UpgradeType MecanoType;
    public MeshRenderer[] MecanoGarage;
    public MecanoAnim[] MecanoAnim;
}