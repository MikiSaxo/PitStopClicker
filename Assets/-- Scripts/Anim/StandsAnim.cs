using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StandsAnim : MonoBehaviour
{
    [SerializeField] private List<GameObjectList> _objectsToAnimate = new List<GameObjectList>();
    [SerializeField] private float _jumpPower = 1f;
    [SerializeField] private float _jumpDuration = 0.5f;
    [SerializeField] private float _delayBetweenJumps = 0.2f;
    [SerializeField] private float _delayBetweenWave = 2f;

    private void Start()
    {
        //StartCoroutine(AnimateWave());
    }

    private IEnumerator AnimateWave()
    {
        foreach (var gameObjectList in _objectsToAnimate)
        {
            foreach (var go in gameObjectList.GameObjectsList)
            {
                go.transform.DOJump(go.transform.position, _jumpPower, 1, _jumpDuration);
            }
            yield return new WaitForSeconds(_delayBetweenJumps);
        }

        yield return new WaitForSeconds(_delayBetweenWave);
        
        StartCoroutine(AnimateWave());
    }
    
    public void SetActiveCrowd(bool active, int index)
    {
        //_objectsToAnimate[index].SetActive(active);
    }
}

[System.Serializable]
public class GameObjectList 
{
    public List<GameObject> GameObjectsList;
}