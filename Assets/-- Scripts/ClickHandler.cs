using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var carMov = hit.transform.GetComponent<CarMovement>();
                if (carMov != null)
                {
                    carMov.OnCarClicked();
                }
            }
        }
    }
}