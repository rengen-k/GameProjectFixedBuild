using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody Rb;

    private PlayerInput playerInput;
    private PlayerActionsScript playerActionsScript;

    public CameraSwitchScript camScript;
    private int currentCam = 0;
    
    //Movement
    private Vector3 movement;

    private float coyoteTime = 0.3f;
    public float coyoteTimeCounter;

    [SerializeField] private float speed;
    [SerializeField] private float jumpVelocity;

    // private Vector3[] movementMap = new Vector3[4];
    // private Vector2 inputVector = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        movement = new Vector3(0.0f, 0.0f, 0.0f);


    }

    void Start() {
        // movementMap[0] = new Vector3(inputVector.x, 0.0f, inputVector.y);
        // movementMap[1] = new Vector3(-inputVector.y, 0.0f, -inputVector.x);
        // movementMap[2] = new Vector3(-inputVector.x, 0.0f, -inputVector.y);
        // movementMap[3] = new Vector3(inputVector.y, 0.0f, inputVector.x);

        //Taken from https://roundwide.com/physics-overlap-capsule/
        // Do I need to cite this.
        





        playerInput = GetComponent<PlayerInput>();

        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Jump.performed += Jump;
        playerActionsScript.Player.Look.performed += Look;

    }

    // Update is called once per frame
    void Update()
    {
   
        
        if (checkGround()){
            coyoteTimeCounter = coyoteTime;
        }
        else{
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        
        
    }

    private void FixedUpdate() {
        Vector2 inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        // Vector3 forward = Camera.main.transform.forward;
        // Debug.Log("forward" + forward);
        // Vector3 right = Camera.main.transform.right;
        // Debug.Log("right" + right);


        // Vector3 forwardRelative = verticalInput * forward;
        // Vector3 rightRelative = horizontalInput * right;
        
        // Vector3 cameraRelativeMovement = forwardRelative + rightRelative;

        // Debug.Log(inputVector);
        // Vector3 movement = new Vector3(cameraRelativeMovement.x, 0.0f, cameraRelativeMovement.y);

        
        
        if (currentCam == 0) {
            movement = new Vector3(inputVector.x, 0.0f, 0f);
        } else if (currentCam == 1) {
            movement = new Vector3(0f, 0.0f, -inputVector.x);
        } else if (currentCam == 2) {
            movement = new Vector3(-inputVector.x, 0.0f, 0f);
        } else if (currentCam == 3) {
            movement = new Vector3(0f, 0.0f, inputVector.x);
        }

        // Debug.Log("" + currentCam + " " + inputVector + movementMap[currentCam]);
        // swap x with y, z with x
        //Debug.Log(movement);
        // transform.Translate(movement * speed * Time.fixedDeltaTime);
        Rb.AddForce(movement * speed * Time.fixedDeltaTime);

    }

    public void Jump(InputAction.CallbackContext context) {
        
        if (context.performed && coyoteTimeCounter > 0f) {
            coyoteTimeCounter = 0f;
            Debug.Log("Jump!" + context.phase);
            Rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            
        }
    }

    public void Look(InputAction.CallbackContext context){
       
        
        if (context.ReadValue<Vector2>().x == -1f ){
            Debug.Log("Detected LEft movement.");
            currentCam = camScript.SwitchState(-1);
            
        }
        else if (context.ReadValue<Vector2>().x == 1f ){
            Debug.Log("Detected Right movement.");
            currentCam = camScript.SwitchState(1);
        }
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "KillPlane")
        {
            transform.position = new Vector3(0,1.33f,0);
            //Respawn();
        }
        
    }
    private void OnCollisionExit(Collision collision) {
        
    }

    private bool checkGround(){

        //Taken from https://roundwide.com/physics-overlap-capsule/
        // Do I need to cite this.
        var col = GetComponent<CapsuleCollider>();
        var direction = new Vector3 {[col.direction] = 1};
        var offset = col.height / 2 - col.radius;
        var localPoint0 = col.center - direction * offset;
        var localPoint1 = col.center + direction * offset;
        var point0 = transform.TransformPoint(localPoint0);
        var point1 = transform.TransformPoint(localPoint1);
        var r = transform.TransformVector(col.radius, col.radius, col.radius);
        var radius = Enumerable.Range(0, 3).Select(xyz => xyz == col.direction ? 0 : r[xyz]).Select(Mathf.Abs).Max();

        Collider[] inContact = new Collider[5];
        
        var num = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, inContact);
        
        for (int i = 0; i < num; i++)
        {
            if (inContact[i].gameObject.tag == "Ground"){
                return true;
            } 
        }
        return false;
    }

    

    


}
