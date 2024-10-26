using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StandsAnim : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToAnimate = new List<GameObject>();
    [SerializeField] private float _jumpPower = 1f;
    [SerializeField] private float _jumpDuration = 0.5f;
    [SerializeField] private float _delayBetweenJumps = 0.2f;
    [SerializeField] private float _delayBetweenWave = 2f;

    private void Start()
    {
        StartCoroutine(AnimateWave());
    }

    private IEnumerator AnimateWave()
    {
        foreach (GameObject obj in _objectsToAnimate)
        {
            obj.transform.DOJump(obj.transform.position, _jumpPower, 1, _jumpDuration);
            yield return new WaitForSeconds(_delayBetweenJumps);
        }

        yield return new WaitForSeconds(_delayBetweenWave);
        
        StartCoroutine(AnimateWave());
    }
    
    public void SetActiveCrowd(bool active, int index)
    {
        _objectsToAnimate[index].SetActive(active);
    }
}