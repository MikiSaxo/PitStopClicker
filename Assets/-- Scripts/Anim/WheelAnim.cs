using System;
using UnityEngine;

public class WheelAnim : MonoBehaviour
{
    [SerializeField] private Transform[] _wheels;
    
    private const float WHEEL_RADIUS = 1.5f;
    private float _angularSpeed;

    public void StartRotation(float linearSpeed)
    {
        _angularSpeed = (linearSpeed / WHEEL_RADIUS) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        foreach (var wheel in _wheels)
        {
            wheel.Rotate(Vector3.right * _angularSpeed * Time.deltaTime);
        }
    }

    public void StopRotation()
    {
        _angularSpeed = 0;
    }

    // private void OnDrawGizmos()
    // {
    //     foreach (var wheel in _wheels)
    //     {
    //         Gizmos.DrawWireSphere(wheel.position, WHEEL_RADIUS);
    //     }
    // }
}