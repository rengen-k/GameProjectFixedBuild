using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRingPuzzle : MonoBehaviour
{
    public bool roll;
    public Transform target;
    //public Transform centerPeiceTarget;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    //[SerializeField] private GameObject centerPeice;

    void Update()
    {
        if(roll)
        {
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, 0));
            //Vector3 targetPosition2 = centerPeiceTarget.TransformPoint(new Vector3(0, 0, 0));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //centerPeice.transform.position = Vector3.SmoothDamp(centerPeice.transform.position, targetPosition2, ref velocity, smoothTime);
        }
    }

}