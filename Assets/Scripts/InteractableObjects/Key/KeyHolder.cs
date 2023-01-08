using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class KeyHolder : MonoBehaviour
{
    public event EventHandler OnKeysChanged;
    private List<Key.KeyType> keyList;
    private PlayerActionsScript playerActionsScript;
    private bool nearDoor = false;
    private KeyDoorTrigger keyDoor;
    private AudioSource soundManager;
    public AudioClip keyCollect;
    public AudioClip openDoor;

    public List<Key.KeyType> GetKeyList()
    {
        return keyList;
    }

    private void OnEnable()
    {
        InitPlayerInput();
        ConfigPlayerInput();
        soundManager = GameObject.Find("SoundManager").GetComponent<AudioSource>();
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
        soundManager.PlayOneShot(keyCollect);
        keyList.Add(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);

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
            // uncomment this if you want the door to auto open when you touch the trigger
            // if (ContainsKey(keyDoor.GetKeyType()))
            // {
            //     RemoveKey(keyDoor.GetKeyType())
            //     keyDoor.OpenDoor();
            // }
        } else {
            nearDoor = false;
        }
    }

    // comment this out if you want the door to auto open when you touch the trigger
    public void Interact(InputAction.CallbackContext context)
    {
        if (!nearDoor) {
            return;
        }

        if (ContainsKey(keyDoor.GetKeyType()))
        {
            RemoveKey(keyDoor.GetKeyType());
            soundManager.PlayOneShot(openDoor);
            keyDoor.OpenDoor();
        }

    }
}
