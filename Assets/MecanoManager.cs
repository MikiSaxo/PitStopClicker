using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanoManager : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> MecanoMeshRenderers = new List<MeshRenderer>();

    private void Start()
    {
        for (int i = 0; i < MecanoMeshRenderers.Count; i++)
        { 
            MecanoMeshRenderers[i].GetComponent<MeshFilter>().sharedMesh = UpgradeManager.Instance.MecanoLvl[i].UpgradePrices[0].MecanoMesh;
            MecanoMeshRenderers[i].material = UpgradeManager.Instance.MecanoLvl[i].UpgradePrices[0].MecanoMaterial;
        }
    }
  
}
