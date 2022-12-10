using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    [SerializeField] private KeyType keyType;

    public enum KeyType
    {
        Red,
        Green,
        Blue,
        Purple
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public KeyType GetKeyType()
    {
        return keyType;
    }
}
