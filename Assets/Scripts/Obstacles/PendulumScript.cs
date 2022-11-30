using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumScript : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float swingAngle = 65;
    [SerializeField] private bool onZAxis;
    private float angle;

    private void Update()
    {
        angle = swingAngle * Mathf.Sin(Time.time * speed);

        if (onZAxis)
        {
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }
    }
}
