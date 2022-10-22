using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

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
    
    // Damage

    private bool isHurt = false;

    //Movement

    private Vector3 movement;

    private float coyoteTime = 0.04f;
    public float coyoteTimeCounter;

    private float jumpBufferTime = 0.4f;
    public float jumpBufferCounter;
    private bool isJumping = false;
    private bool jumpRequest;
    [SerializeField] private float lastGrounded;
    [SerializeField] private float jumpCutMultiplier;

    [SerializeField] private float speed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float fallMultiplier = 2f;

    private Vector3 rotation = new Vector3(0, 90f, 0);

    [SerializeField] private Transform groundCheck;
    private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;

    private Transform model;

    // private Vector3 originalPos;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        movement = new Vector3(0.0f, 0.0f, 0.0f);

        var col = GetComponent<CapsuleCollider>();
        var direction = new Vector3 {[col.direction] = 1};
        var offset = (col.height) / 2 - col.radius;
        groundRadius = col.radius+0.1f;
        var localPoint0 = col.center - direction * offset;
        groundCheck.position = transform.TransformPoint(localPoint0);

    }

    void Start()
    {
        currentHealth = maxHealth;
        // originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, gameObject.transform.position.z);

        playerInput = GetComponent<PlayerInput>();
        model = transform.Find("Model");
        //playerActionsScript = new PlayerActionsScript();
        //playerActionsScript.Player.Enable();
        //playerActionsScript.Player.Jump.performed += Jump;
        //playerActionsScript.Player.Look.performed += Look;

    }

    private void OnEnable()
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Jump.started += Jump;
        playerActionsScript.Player.Jump.canceled += Jump;
        playerActionsScript.Player.Look.performed += Look;
    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferCounter -= Time.deltaTime;
        lastGrounded -= Time.deltaTime;

    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround);
        
        Vector2 inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        if (currentCam == 0)
        {
            movement = new Vector3(inputVector.x, 0.0f, 0.0f);
        }
        else if (currentCam == 1)
        {
            movement = new Vector3(0f, 0.0f, inputVector.x);
        }
        else if (currentCam == 2)
        {
            movement = new Vector3(-inputVector.x, 0.0f, 0f);
        }
        else if (currentCam == 3)
        {
            movement = new Vector3(0f, 0.0f, -inputVector.x);
        }

        // character model looks left and right
        if (movement.x > 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
            //model.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (movement.x < 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            //model.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (movement.z < 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, 0, 0);
            //model.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (movement.z > 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            //model.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (currentCam == 1 || currentCam == 3)
        {
            //Debug.Log("in z loop");
            float targetSpeed = movement.z * speed;
            float speedDif = targetSpeed - Rb.velocity.z;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            movement.z = move;
            movement.x = 0f;
        }
        else if (currentCam == 0 || currentCam == 2)
        {
            //Debug.Log("in x loop");
            float targetSpeed = movement.x * speed;
            float speedDif = targetSpeed - Rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            movement.x = move;
            movement.z = 0f;
        }

        // transform.Translate(movement * speed * Time.fixedDeltaTime);
        //Debug.Log(movement);
        Rb.AddForce(movement * Time.fixedDeltaTime);

        if (jumpRequest)
        {
            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
            {

                jumpBufferCounter = jumpBufferTime;
                coyoteTimeCounter = 0f;
                jumpRequest = false;

                Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
                Rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
                StartCoroutine(JumpCooldown());

            }

        }

        if (Rb.velocity.y < 0)
        {
            Rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        Rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);

    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("in jump");
        jumpRequest = true;
        jumpBufferCounter = jumpBufferTime;
        if (context.canceled)
        {
            jumpRequest = false;
            if (Rb.velocity.y > 0 && isJumping)
            {
                Rb.AddForce(Vector3.down * Rb.velocity.y * (1 - jumpCutMultiplier), ForceMode.Impulse);
            }
        }
    }

    public void Look(InputAction.CallbackContext context)
    {

        //Debug.Log(context);
        if (context.ReadValue<Vector2>().x <= -0.5f)
        {
            currentCam = camScript.SwitchState(-1);
            //Debug.Log("currentCam: " + currentCam);
            model.transform.Rotate(rotation);
        }
        else if (context.ReadValue<Vector2>().x >= 0.5f)
        {
            currentCam = camScript.SwitchState(1);
            //Debug.Log("currentCam: " + currentCam);
            model.transform.Rotate(-rotation);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "HurtTag1" && !isHurt) {
            //Debug.Log("Collided with HurtTag1");
            currentHealth -= 1;
            StartCoroutine(HurtCooldown());
            if (currentHealth <= 0) {
            Respawn();
            } else {
                Debug.Log("currentHealth: " + currentHealth);
                Rb.AddForce(Vector3.up * 12f, ForceMode.Impulse);
            }
        }
        else if (collision.gameObject.name == "KillPlane")
        {
            Respawn();
        }
        else if(collision.gameObject.tag == "JumpTag"){
            //Debug.Log("Should be forced up");
            coyoteTimeCounter = 0f;

            //For now, trampoline forces you up with twice the force of the jump. When the carryable tag is entered, this should instead query the value the trampoline says
            // Rb.AddForce(Vector3.up * jumpVelocity * collision.gameObject.getJumpMult(), ForceMode.Impulse);
            // No need to have jumpVelocity in calculation.
            Rb.AddForce(Vector3.up * 14f, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {

    }

    private void ResetPlayerHealth() {
        currentHealth = maxHealth;
    }

    private void Respawn() {
        ResetPlayerHealth();
        // respawn from originalPos
        // transform.position = originalPos;
        // respawn by reloading the level to reset the placement of other objects
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private IEnumerator HurtCooldown()
    {
        isHurt = true;
        yield return new WaitForSeconds(0.4f);
        isHurt = false;
    }



}