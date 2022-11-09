using UnityEngine;
using UnityEngine.InputSystem;

// Exists to attempt to suppress a non fatal error.
// Credit user erikDo
// https://forum.unity.com/threads/type-of-instance-in-array-does-not-match-expected-type.1320564/#post-8396772
public class PlayerInputErrorWorkaround : MonoBehaviour
{
    private PlayerInput Input;
   
    private void Start()
    {
        Input = GetComponent<PlayerInput>();
    }
 
    private void OnDisable()
    {
        Input.actions = null;
    }
}