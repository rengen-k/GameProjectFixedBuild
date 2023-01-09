using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton1 : MonoBehaviour
{
    public static Singleton1 instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
        }
    }
}
