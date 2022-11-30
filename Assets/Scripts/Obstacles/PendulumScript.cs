using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumScript : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float swingAngle = 80;
    private float angle;

    private void Update()
    {
        angle = swingAngle * Mathf.Sin(Time.time * speed);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
