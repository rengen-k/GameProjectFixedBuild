using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSettingsSingleton : MonoBehaviour
{
    public static CurrentSettingsSingleton instance;
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
