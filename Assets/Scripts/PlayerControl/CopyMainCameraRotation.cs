using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMainCameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
    Transform cam;
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 rotation = cam.rotation.eulerAngles; 
        rotation.z = Mathf.Clamp(rotation.z, -0.1f, 0.1f);
        rotation.x = Mathf.Clamp(rotation.x, -0.1f, 0.1f);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
