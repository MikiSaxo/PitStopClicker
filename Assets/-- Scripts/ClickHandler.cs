using System;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public static ClickHandler Instance;
    
    [Header("--- FX")]
    [SerializeField] private GameObject _fxClick;
    [SerializeField] private GameObject _fxRepairGood;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                IClickable clickable = hit.transform.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClicked(hit.point);
                }
            }
        }
    }
    
    public void CreateFXClick(Vector3 position)
    {
        Instantiate(_fxClick, position, Quaternion.identity);
    }
    public void CreateFXRepairGood(Vector3 position)
    {
        Instantiate(_fxRepairGood, position, _fxRepairGood.transform.rotation);
    }
}

public interface IClickable
{
    void OnClicked(Vector3 hitPoint);
}