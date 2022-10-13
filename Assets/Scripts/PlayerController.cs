using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    private Rigidbody Rb;
    private bool isGrounded = true;
    [SerializeField] private float speed;
    [SerializeField] private float jumpVelocity;
    private PlayerInput playerInput;
    private PlayerActionsScript playerActionsScript;
    public CameraSwitchScript camScript;
    

    // Start is called before the first frame update
    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        


    }

    void Start() {

        playerInput = GetComponent<PlayerInput>();

        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Jump.performed += Jump;
        playerActionsScript.Player.Look.performed += Look;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        Vector2 inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        float verticalInput = inputVector.y;
        float horizontalInput = inputVector.x;

        // Vector3 forward = Camera.main.transform.forward;
        // Debug.Log("forward" + forward);
        // Vector3 right = Camera.main.transform.right;
        // Debug.Log("right" + right);


        // Vector3 forwardRelative = verticalInput * forward;
        // Vector3 rightRelative = horizontalInput * right;
        
        // Vector3 cameraRelativeMovement = forwardRelative + rightRelative;




        // Debug.Log(inputVector);
        // Vector3 movement = new Vector3(cameraRelativeMovement.x, 0.0f, cameraRelativeMovement.y);
        
        Vector3 movement = new Vector3(inputVector.y, 0.0f, inputVector.x);
        // swap x with y, z with x
        //Debug.Log(movement);
        // transform.Translate(movement * speed * Time.fixedDeltaTime);
        Rb.AddForce(movement * speed * Time.fixedDeltaTime);

    }

    public void Jump(InputAction.CallbackContext context) {
        Debug.Log(context);
        if (context.performed && isGrounded) {
            Debug.Log("Jump!" + context.phase);
            Rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void Look(InputAction.CallbackContext context){
       
        
        if (context.ReadValue<Vector2>().x == -1f ){
            Debug.Log("Detected LEft movement.");
            camScript.SwitchState(-1);
            
        }
        else if (context.ReadValue<Vector2>().x == 1f ){
            Debug.Log("Detected Right movement.");
            camScript.SwitchState(1);
        }
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Ground") {
            isGrounded = true;
        }
        else if (collision.gameObject.name == "KillPlane")
        {
            transform.position = new Vector3(0,1.33f,0);
            //Respawn();
        }
    }
    private void OnCollisionExit(Collision collision) {
            isGrounded = false;
    }
}
