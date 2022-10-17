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

    //Stats

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 1;
    
    //Movement

    private Vector3 movement;
    private float lastMove = 0f;

    private float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    private float jumpBufferTime = 0.3f;
    public float jumpBufferCounter;

    private bool isJumping = false;

    private bool jumpRequest;

    [SerializeField] private float speed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float fallMultiplier = 2f;

    [SerializeField] private Transform model;

    private Vector3 rotation = new Vector3(0, 90, 0);

    private Vector3 originalPos;


    // Start is called before the first frame update
    void Awake()
    {
        
        Rb = GetComponent<Rigidbody>();
        movement = new Vector3(0.0f, 0.0f, 0.0f);

    }

    void Start() {
        currentHealth = maxHealth;
        originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);

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
 
    }

    private void FixedUpdate() {
        Vector2 inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

    

        // character model looks left and right
        if (lastMove != inputVector.x && inputVector.x != 0f){
            lastMove = inputVector.x;
            if (inputVector.x > 0)
            {
                
                model.transform.RotateAround(transform.position, Vector3.up, -180f);
            }
            else if (inputVector.x < 0)
            {
                model.transform.RotateAround(transform.position, Vector3.up, 180f);
            }
        }

        if (currentCam == 0) {
            movement = new Vector3(inputVector.x, 0.0f, 0.0f);
        } else if (currentCam == 1) {
            movement = new Vector3(0f, 0.0f, inputVector.x);
        } else if (currentCam == 2) {
            movement = new Vector3(-inputVector.x, 0.0f, 0f);
        } else if (currentCam == 3) {
            movement = new Vector3(0f, 0.0f, -inputVector.x);
        }

        // transform.Translate(movement * speed * Time.fixedDeltaTime);
        Rb.AddForce(movement * speed * Time.fixedDeltaTime);

        if (jumpRequest){
            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping ) {

                jumpBufferCounter = jumpBufferTime;
                coyoteTimeCounter = 0f;
                jumpRequest = false;

                Rb.velocity = new Vector3(Rb.velocity.x,0f,Rb.velocity.z);
                Rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
                StartCoroutine(JumpCooldown());
                
            }
            
        }
    
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
            currentCam = camScript.SwitchState(-1);
            transform.Rotate(rotation);
            
        }
        else if (context.ReadValue<Vector2>().x >= 0.5f ){
            currentCam = camScript.SwitchState(1);
            transform.Rotate(-rotation);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "HurtTag1") {
            Debug.Log("Collided with HurtTag1");
            currentHealth -= 1;
            if (currentHealth <= 0) {
            Respawn();
            } else {
                Debug.Log("currentHealth: " + currentHealth);
                Rb.AddForce(Vector3.up * jumpVelocity * 1, ForceMode.Impulse);
            }
            
        } 
        else if (collision.gameObject.name == "KillPlane")
        {
            Respawn();
        }
        else if(collision.gameObject.tag == "JumpTag"){
            Debug.Log("Should be forced up");
            coyoteTimeCounter = 0f;

            //For now, trampoline forces you up with twice the force of the jump. When the carryable tag is entered, this should instead query the value the trampoline says
            // Rb.AddForce(Vector3.up * jumpVelocity* collision.gameObject.getJumpMult(), ForceMode.Impulse);
            Rb.AddForce(Vector3.up * jumpVelocity*2, ForceMode.Impulse);
        }

        
        
    }

    private void ResetPlayerHealth() {
        currentHealth = maxHealth;
    }

    private void Respawn() {
        ResetPlayerHealth();
        transform.position = originalPos;
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
