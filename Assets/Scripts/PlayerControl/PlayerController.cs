using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //-------------------------//
    // Init
    private Rigidbody Rb;
    private Transform model;
    private PlayerInput playerInput;
    private PlayerActionsScript playerActionsScript;

    //-------------------------//
    // Camera
    public CameraSwitchScript camScript;
    private int currentCam = 0;

    //-------------------------//
    // Stats
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 1;

    //-------------------------//
    // Damage
    private bool isHurt = false;
    
    //-------------------------//
    // Movement
    private Vector3 movement;
    private float velPower = 1.5f;
    [SerializeField] private float speed = 12;
    [SerializeField] private float frictionAmount = 0.45f;
    [SerializeField] private float acceleration = 17;
    [SerializeField] private float deceleration = 25;
    
    //-------------------------//
    // Coyote Time
    private float coyoteTime = 0.06f;
    private float coyoteTimeCounter;

    //-------------------------//
    // Jump
    private float jumpBufferTime = 0.4f;
    private float jumpBufferCounter;
    private bool isJumping = false;
    private bool jumpRequest;
    private float lastGrounded;
    private float jumpCutMultiplier = 0.5f;
    private float fallMultiplier = 2f;
    [SerializeField] private float jumpMultiplier = 10.5f;

    private float jumpTrampolineHeight = 19.0f;
    
    //-------------------------//
    // Ground Check
    private Vector3 rotation = new Vector3(0, 90f, 0);
    private float groundRadius;
    private bool isGrounded;
    private bool isStableGrounded;
    private bool isJumpTrampoline = false;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    
    //-------------------------//
    // Respawn
    private Vector3 lastGroundedPosition;
    private bool updateRespawnPosition = true;
    private bool isNotNearEdge;
    private Vector3 originalPos;

    private bool RespawnRaycastMinusX;
    private bool RespawnRaycastMinusZ;
    private bool RespawnRaycastPlusX;
    private bool RespawnRaycastPlusZ;


    //-----------------------------------------//
    // ####################################### //
    // -------------  Methods  --------------- //
    // ####################################### //
    //-----------------------------------------//


    //-----------------------------------------//
    // Awake
    //-----------------------------------------//
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        ConfigureMovement();
        ConfigureGroundCheckAndRadius();
    }

    private void ConfigureMovement() 
    {
        movement = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void ConfigureGroundCheckAndRadius() 
    {
        var col = GetComponent<CapsuleCollider>();
        var direction = new Vector3 {[col.direction] = 1};
        var offset = (col.height) / 2 - col.radius;
        var localPoint0 = col.center - direction * (offset+0.1f);
        groundRadius = col.radius - 0.01f;
        groundCheck.position = transform.TransformPoint(localPoint0);
    }

    //-----------------------------------------//
    // Start
    //-----------------------------------------//
    private void Start()
    {
        lastGroundedPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, gameObject.transform.position.z);
        ResetPlayerHealth();
        playerInput = GetComponent<PlayerInput>();
        model = transform.Find("Model");
    }

    //-----------------------------------------//
    // OnEnable and OnDisable
    //-----------------------------------------//
    private void OnEnable()
    {
        InitPlayerInput();
        ConfigPlayerInput();
    }

    private void InitPlayerInput() 
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
    }

    private void ConfigPlayerInput() 
    {
        playerActionsScript.Player.Jump.started += Jump;
        playerActionsScript.Player.Jump.canceled += Jump;
        playerActionsScript.Player.Look.performed += Look;
    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    //-----------------------------------------//
    // Update
    //-----------------------------------------//
    private void Update()
    {
        InitJumpGroundDetection();
        ConfigCoyoteTimeCounter();
        UpdateRespawn();
    }

    private void InitJumpGroundDetection() 
    {
        jumpBufferCounter -= Time.deltaTime;
        lastGrounded -= Time.deltaTime;
    }

    private void ConfigCoyoteTimeCounter() 
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void UpdateRespawn() 
    {
        if (isNotNearEdge && isStableGrounded && updateRespawnPosition && coyoteTimeCounter == coyoteTime) {
            lastGroundedPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, gameObject.transform.position.z);
            StartCoroutine(RespawnPositionCooldown());
        }
    }


    //-----------------------------------------//
    // FixedUpdate
    //-----------------------------------------//
    private void FixedUpdate()
    {
        CheckIfGroundedorStableGrounded();
        Vector2 inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        SetMovementDirection(inputVector);
        ConfigPlayerModelRotationDirection();
        ConfigMovementAmount();

        ApplyFriction(inputVector);  

        Rb.AddForce(movement * Time.fixedDeltaTime);

        if (jumpRequest)
        {
            ExecuteJump();
        }

        ModifyFallSpeed();

        isNotNearEdge = CheckIfPlayerNotNearEdge();
    }

    // Check whether sphere is colliding with ground or stableground
    private void CheckIfGroundedorStableGrounded() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround) || Physics.CheckSphere(groundCheck.position, groundRadius, (1 << 8));
        isStableGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (1 << 8));
    }

    private void SetMovementDirection(Vector2 inputVector) {
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
    }

    private void ConfigPlayerModelRotationDirection() 
    {
        if (movement.x > 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
        }
        else if (movement.x < 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        }
        else if (movement.z < 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, 0, 0);
        }
        else if (movement.z > 0)
        {
            model.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        }
    }

    private void ConfigMovementAmount() 
    {
        if (currentCam == 1 || currentCam == 3)
        {
            float targetSpeed = movement.z * speed;
            float speedDif = targetSpeed - Rb.velocity.z;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            movement.z = move;
            movement.x = 0f;
        }
        else if (currentCam == 0 || currentCam == 2)
        {
            float targetSpeed = movement.x * speed;
            float speedDif = targetSpeed - Rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            movement.x = move;
            movement.z = 0f;
        }
    }

    // Apply opposite force to player movement to imitate friction
    private void ApplyFriction(Vector2 inputVector) 
    {
        if (isGrounded && Mathf.Abs(inputVector.x) < 0.01f && (currentCam == 1 || currentCam == 3))
        {
            float amount = Mathf.Min(Mathf.Abs(Rb.velocity.z), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(Rb.velocity.z);
            Rb.AddForce(Vector3.forward * -amount, ForceMode.Impulse);
        }
        else if (isGrounded && Mathf.Abs(inputVector.x) < 0.01f && (currentCam == 0 || currentCam == 2))
        {
            float amount = Mathf.Min(Mathf.Abs(Rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(Rb.velocity.x);
            Rb.AddForce(Vector3.right * -amount, ForceMode.Impulse);
        }
    }

    private void ExecuteJump() 
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
            {
                jumpBufferCounter = jumpBufferTime;
                coyoteTimeCounter = 0f;
                jumpRequest = false;

                Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
                Rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
                StartCoroutine(JumpCooldown());
            }
    }

    // Modifies fall speed to become faster or slower
    private void ModifyFallSpeed() 
    {
        if (Rb.velocity.y < 0)
        {
            Rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        Rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
    }

    // Determines whether player is not near the edge - main use is to respawn at correct locations
    private bool CheckIfPlayerNotNearEdge() 
    {
        int layerMask = 1 << 8;

        RespawnRaycastMinusX = Physics.Raycast(transform.position + new Vector3(-0.3f, 0.0f, 0.0f), transform.TransformDirection(Vector3.down), 1, layerMask);
        RespawnRaycastPlusX = Physics.Raycast(transform.position + new Vector3(0.3f, 0.0f, 0.0f), transform.TransformDirection(Vector3.down), 1, layerMask);
        RespawnRaycastMinusZ = Physics.Raycast(transform.position + new Vector3(0.0f, 0.0f, -0.3f), transform.TransformDirection(Vector3.down), 1, layerMask);
        RespawnRaycastPlusZ = Physics.Raycast(transform.position + new Vector3(0.0f, 0.0f, 0.3f), transform.TransformDirection(Vector3.down), 1, layerMask);

        bool nearEdge = true;

        if (RespawnRaycastMinusX && RespawnRaycastPlusX && RespawnRaycastMinusZ && RespawnRaycastPlusZ)
        {
            nearEdge = true;
        } else {
            nearEdge = false;
        }

        return nearEdge;
    }

    //-----------------------------------------//
    // Jump
    //-----------------------------------------//
    public void Jump(InputAction.CallbackContext context)
    {
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

    //-----------------------------------------//
    // Look
    //-----------------------------------------//
    public void Look(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x <= -0.5f)
        {
            currentCam = camScript.SwitchState(-1);
            model.transform.Rotate(rotation);
        }
        else if (context.ReadValue<Vector2>().x >= 0.5f)
        {
            currentCam = camScript.SwitchState(1);
            model.transform.Rotate(-rotation);
        }
    
        ModifyConstraintsBasedOnCamera();
    }

    // Rigidbody constraints to prevent movement in an axis that is not intended to be moved in
    private void ModifyConstraintsBasedOnCamera() 
    {
        if (currentCam == 0 | currentCam == 2) {
            Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        } else if (currentCam == 1 | currentCam == 3) {
            Rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }

    //-----------------------------------------//
    // OnCollisionEnter
    //-----------------------------------------//
    // OnCollisionEnter - behaviour changes depending on tag that player collides with
    // --Tags:--
    // HurtTag1 - take damage
    // KillPlane - Respawn
    // JumpTag - Bounce Up
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "HurtTag1" && !isHurt) 
        {
            currentHealth -= 1;
            StartCoroutine(HurtCooldown());
            if (currentHealth <= 0) {
            Respawn();
            } else {
                Rb.AddForce(Vector3.up * 12f, ForceMode.Impulse);
            }
        }

        else if (collision.gameObject.name == "KillPlane")
        {
            Respawn();
        }

        else if (collision.gameObject.tag == "JumpTag" && !isJumpTrampoline){
            coyoteTimeCounter = 0f;
            StartCoroutine(TrampolineCooldown());
            Rb.velocity = Vector3.zero;
            Rb.AddForce(Vector3.up * jumpTrampolineHeight, ForceMode.Impulse);
        }
    }

    // Reset Player health to maxHealth
    private void ResetPlayerHealth() {
        currentHealth = maxHealth;
    }

    // Set player to lastGroundedPosition and reset their health
    private void Respawn() {
        ResetPlayerHealth();
        transform.position = lastGroundedPosition;
    }

    //-----------------------------------------//
    // Cooldowns (Coroutines)
    //-----------------------------------------//
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

    private IEnumerator TrampolineCooldown()
    {
        isJumpTrampoline = true;
        yield return new WaitForSeconds(0.4f);
        isJumpTrampoline = false;
    }

    private IEnumerator RespawnPositionCooldown()
    {
        updateRespawnPosition = false;
        yield return new WaitForSeconds(0.5f);
        updateRespawnPosition = true;
    }
}