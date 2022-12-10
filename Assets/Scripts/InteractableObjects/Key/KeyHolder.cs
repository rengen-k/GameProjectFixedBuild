using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class KeyHolder : MonoBehaviour
{
    private List<Key.KeyType> keyList;
    private PlayerActionsScript playerActionsScript;
    private bool nearDoor = false;
    private KeyDoorTrigger keyDoor;

    // Start is called before the first frame update

    private void OnEnable()
    {
        InitPlayerInput();
        ConfigPlayerInput();
    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    private void InitPlayerInput() 
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
    }

    private void ConfigPlayerInput() 
    {
        playerActionsScript.Player.Interact.performed += Interact;
    }

    private void Awake()
    {
        keyList = new List<Key.KeyType>();
    }

    public void AddKey(Key.KeyType keyType)
    {
        keyList.Add(keyType);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
    }

    public bool ContainsKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter(Collider other) {
        Key key = other.GetComponent<Key>();
        if (key != null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        keyDoor = other.GetComponent<KeyDoorTrigger>();
        if (keyDoor != null)
        {
            nearDoor = true;
            // if (ContainsKey(keyDoor.GetKeyType()))
            // {
            //     RemoveKey(keyDoor.GetKeyType())
            //     keyDoor.OpenDoor();
            // }
        } else {
            nearDoor = false;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!nearDoor) {
            return;
        }

        if (ContainsKey(keyDoor.GetKeyType()))
        {
            RemoveKey(keyDoor.GetKeyType());
            keyDoor.OpenDoor();
        }

    }
}
