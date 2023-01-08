using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionComplete : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localRotation.eulerAngles.z > 9 && transform.localRotation.eulerAngles.z < 10){
            cube.SetActive(true);
        }
    }
}
