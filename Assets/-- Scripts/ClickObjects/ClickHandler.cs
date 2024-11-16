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
        ClickedDown();
    }

    private void ClickedDown()
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
    
    public void CreateFXClick(Vector3 position, int power)
    {
        if (power > 5) power = 5;
        if (power <= 0) power = 1;
        
        for (int i = 0; i < power; i++)
        {
            Instantiate(_fxClick, position, _fxClick.transform.rotation);
        }
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