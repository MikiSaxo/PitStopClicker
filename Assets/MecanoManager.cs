using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MecanoManager : MonoBehaviour
{
    public static MecanoManager Instance;

    [SerializeField] private List<MecanoMeshes> _mecanoMeshes = new List<MecanoMeshes>();
    
    private List<MecanoAnim> _mecanoAnims = new List<MecanoAnim>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ClickCarJack.Instance.OnCarJackSet += LaunchMecano;
        CarSpawner.Instance.OnCarRepaired += GoBackMecano;
        
        GetMecanoAnim();
        
        for (int i = 0; i < _mecanoMeshes.Count; i++)
        {
            UpdateMecanoMesh(i, 0);
            
            SetActiveMecano(i, false);
            
            if(_mecanoMeshes[i].MecanoMR.Length > 0)
                _mecanoMeshes[i].MecanoMR[0].gameObject.SetActive(true);
        }
    }

    private void GetMecanoAnim()
    {
        foreach (var mecanoMesh in _mecanoMeshes)
        {
            foreach (var mr in mecanoMesh.MecanoMR)
            {
                if(mr.GetComponent<MecanoAnim>() != null)
                    _mecanoAnims.Add(mr.GetComponent<MecanoAnim>());
            }
        }
    }

    public void UpdateMecanoMesh(int index, int level)
    {
        if (_mecanoMeshes[index] == null) return;

        if (level >= UpgradeManager.Instance.MecanoLvl[index].MecanoPrices.Count)
        {
            level = UpgradeManager.Instance.MecanoLvl[index].MecanoPrices.Count - 1;
        }

        foreach (var mecanoMR in _mecanoMeshes[index].MecanoMR)
        {
            mecanoMR.GetComponent<MeshFilter>().sharedMesh = UpgradeManager.Instance.MecanoLvl[index].MecanoPrices[level].MecanoMesh;
            mecanoMR.material = UpgradeManager.Instance.MecanoLvl[index].MecanoPrices[level].MecanoMaterial;
        }
    }

    public void SetActiveMecano(int index, bool active)
    {
        if (_mecanoMeshes[index] == null || _mecanoMeshes[index].MecanoMR.Length == 0) return;
        
        foreach (var mecano in _mecanoMeshes[index].MecanoMR)
        {
            mecano.gameObject.SetActive(active);
        }
    }

    private void LaunchMecano()
    {
        foreach (var meca in _mecanoAnims)
        {
            if(meca.gameObject.activeSelf)
                meca.LaunchAnim();
        }
    }

    private void GoBackMecano()
    {
        
    }

    private void OnDisable()
    {
        CarSpawner.Instance.OnCarAtClickPoint -= LaunchMecano;
        CarSpawner.Instance.OnCarRepaired -= GoBackMecano;
    }
}

[System.Serializable]
public class MecanoMeshes
{
    public MeshRenderer[] MecanoMR;
}