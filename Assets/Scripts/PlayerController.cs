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

    private float coyoteTime = 2.5f;
    public float coyoteTimeCounter;

    private float jumpBufferTime = 10f;
    public float jumpBufferCounter;

    private bool isJumping = false;

    private bool jumpRequest;

    [SerializeField] private float speed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float fallMultiplier = 2f;

    private Vector3 rotation = new Vector3(0, 90, 0);
    // private Vector3[] movementMap = new Vector3[4];
    // private Vector2 inputVector = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        movement = new Vector3(0.0f, 0.0f, 0.0f);


    }

    void Start() {
        playerInput = GetComponent<PlayerInput>();

        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Jump.performed += Jump;
        playerActionsScript.Player.Look.performed += Look;

    }

    private void OnDisable() {
        playerActionsScript.Player.Disable();
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
        jumpBufferCounter -= Time.deltaTime;


        if (jumpRequest){
         
            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping ) {
                jumpBufferCounter = jumpBufferTime;
                coyoteTimeCounter = 0f;
                Debug.Log("Jump!" );
                Rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
                
                StartCoroutine(JumpCooldown());
                
                jumpRequest = false;
            }
            
        }
        
        
    }

    private void FixedUpdate() {
        Vector2 inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        if (currentCam == 0) {
            movement = new Vector3(inputVector.x, 0.0f, 0f);
        } else if (currentCam == 1) {
            movement = new Vector3(0f, 0.0f, -inputVector.x);
        } else if (currentCam == 2) {
            movement = new Vector3(-inputVector.x, 0.0f, 0f);
        } else if (currentCam == 3) {
            movement = new Vector3(0f, 0.0f, inputVector.x);
        }

        // transform.Translate(movement * speed * Time.fixedDeltaTime);
        Rb.AddForce(movement * speed * Time.fixedDeltaTime);

    
        if (Rb.velocity.y < 0) {
            Rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }

    public void Jump(InputAction.CallbackContext context) {
        jumpRequest = true;
        jumpBufferCounter = jumpBufferTime;
       
    }

    public void Look(InputAction.CallbackContext context){
       
        Debug.Log(context);
        if (context.ReadValue<Vector2>().x <= -0.5f ){
            Debug.Log("Detected LEft movement.");
            currentCam = camScript.SwitchState(1);
            
        }
        else if (context.ReadValue<Vector2>().x >= 0.5f ){
            Debug.Log("Detected Right movement.");
            currentCam = camScript.SwitchState(-1);
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

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    


}
