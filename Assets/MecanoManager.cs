using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MecanoManager : MonoBehaviour
{
    public static MecanoManager Instance;

    [SerializeField] private List<MeshRenderer> _mecanoMeshesGarage = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> _mecanoMeshesCircuit = new List<MeshRenderer>();
    [SerializeField] private List<MecanoMeshes> _mecanoMeshes = new List<MecanoMeshes>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < _mecanoMeshesGarage.Count; i++)
        {
            UpdateMecanoMesh(i, 0);
        }

        // for (int i = 0; i < MecanoMeshesCircuit.Count; i++)
        // {
        //     SetActiveMecano(i, false);
        // }
    }

    public void UpdateMecanoMesh(int index, int level)
    {
        if (_mecanoMeshesGarage[index] == null) return;

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
        if (_mecanoMeshesCircuit[index] == null) return;

        //MecanoMeshesCircuit[index].gameObject.SetActive(active);
    }
}

[System.Serializable]
public class MecanoMeshes
{
    public MeshRenderer[] MecanoMR;
}