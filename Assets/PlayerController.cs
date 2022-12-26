using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

//-----------------------------------------//
// Player Controller
//-----------------------------------------//
// Governs Player physics, movement, jumping, basic input, player respawn, and interactions with game mechanics

public class PlayerController : MonoBehaviour
{

    //-------------------------//
    // Init
    private Rigidbody Rb;
    private Transform model;
    public PlayerInput playerInput;
    public PlayerActionsScript playerActionsScript;

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
    private RigidbodyConstraints disableConstraints;
    //-------------------------//
    // Jump
    private float jumpBufferTime = 0.4f;
    private float jumpBufferCounter;
    public bool isJumping = false;
    private bool jumpRequest;
    private float lastGrounded;
    private float jumpCutMultiplier = 0.5f;
    private float fallMultiplier = 2f;
    [SerializeField] private float jumpMultiplier = 12f;

    // Fixed Jump Values
    private float landJumpMultiplier = 12f;
    private float seaJumpMultiplier = 8f;
    private float jumpTrampolineHeight = 19.0f;

    //-------------------------//
    // Coyote Time
    private float coyoteTime = 0.06f;
    private float coyoteTimeCounter;

    //-------------------------//
    // Ground Check
    private Vector3 rotation = new Vector3(0, 90f, 0);
    private float groundRadius;
    private bool isGrounded;
    private bool isStableGrounded;
    private bool isJumpTrampoline = false;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ladderCheck;
    private Transform swimCheck;
    private float swimRadius;
    private bool inWater;
    private bool swimming;
    [SerializeField] private LayerMask whatIsWater;
    private float waterMov;
    private bool nearLedge;

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

    public LadderScript ladderScript;
    private Vector3 checkpoint;


    //-----------------------------------------//
    // Awake
    //-----------------------------------------//
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        checkpoint = transform.position;
        InitMovement();
        ConfigureGroundCheckAndRadius();



    }

    private void InitMovement() 
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
        ladderScript = GetComponent<LadderScript>();
        swimCheck = ladderCheck;
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

    public void Enable()
    {
        playerActionsScript.Player.Enable();
        Rb.constraints = disableConstraints;
    }
    public void Disable()
    {
        playerActionsScript.Player.Disable();
        disableConstraints = Rb.constraints;
        Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    //-----------------------------------------//
    // Update
    //-----------------------------------------//
    // Monitors variables associated with jumping, coyote time counter, and refreshes the respawn position
    private void Update()
    {
        JumpGroundDetection();
        ConfigCoyoteTimeCounter();
        UpdateRespawn();
    }

    private void JumpGroundDetection() 
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

    // Update respawn position when player is not near an edge and is grounded
    private void UpdateRespawn() 
    {
        if (isNotNearEdge && isStableGrounded && updateRespawnPosition && coyoteTimeCounter == coyoteTime)
        {
            lastGroundedPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, gameObject.transform.position.z);
            StartCoroutine(RespawnPositionCooldown());
        }
    }

    //-----------------------------------------//
    // FixedUpdate
    //-----------------------------------------//
    // Performs movement, jumping, ground detection, and physics while there is an input
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

        /* player counts as grounded when on the ladder but they have to be moving as well which stops them
        from jumping straight up the ladder faster than they are supposed to */
        if (ladderScript.onLadder && inputVector.x != 0)
        {
            isGrounded = true;
        }

        swimming = inWater;

        // adjusts movement values depending on if you are in water
        if (inWater)
        {
            modifyWaterMovementValues();
        }
        else
        {
            modifyLandMovementValues();
        }

        // makes it so if you are near a ledge you pop up higher in the water so you can get out
        if (nearLedge)
        {
            swimCheck = groundCheck;
            swimRadius = groundRadius;
        }
        else
        {
            swimCheck = ladderCheck;
            swimRadius = 0f;
        }
    }

    private void modifyWaterMovementValues()
    {
        acceleration = 12;
        deceleration = 12;
        speed = 6;
        frictionAmount = 0.1f;
        jumpMultiplier = seaJumpMultiplier;
    }

    private void modifyLandMovementValues()
    {
        acceleration = 17;
        deceleration = 25;
        speed = 12;
        frictionAmount = 0.45f;
        jumpMultiplier = landJumpMultiplier;
    }

    // Check whether sphere is colliding with ground or stableground
    private void CheckIfGroundedorStableGrounded() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround) || Physics.CheckSphere(groundCheck.position, groundRadius, (1 << 8));
        isStableGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (1 << 8));
        inWater = Physics.CheckSphere(swimCheck.position, swimRadius, (int)whatIsWater);
        nearLedge = Physics.CheckSphere(swimCheck.position, groundRadius + 0.3f, (int)whatIsGround);
    }

    // Set the direction of movement based on current camera used and if the player is in water
    private void SetMovementDirection (Vector2 inputVector)
    {
        if (inWater)
        {
            waterMov = inputVector.y;
        }
        else if (!inWater)
        {
            waterMov = 0.0f;
        }
        if (currentCam == 0)
        {
            movement = new Vector3(inputVector.x, waterMov, 0.0f);
        }
        else if (currentCam == 1)
        {
            movement = new Vector3(0f, waterMov, inputVector.x);
        }
        else if (currentCam == 2)
        {
            movement = new Vector3(-inputVector.x, waterMov, 0f);
        }
        else if (currentCam == 3)
        {
            movement = new Vector3(0f, waterMov, -inputVector.x);
        }
    }

    // While moving, changes the rotation of the model to be relative to the camera
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

    // Changes amount of movement in the z or x axis depending on a number of physics variables
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

        // allows for upward and downward movement while in water
        if (swimming)
        {
            float targetSpeed = movement.y * speed;
            float speedDif = targetSpeed - Rb.velocity.y;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            // multiplier for upward movement to account for gravity
            if (move > 0.01f)
            {
                move *= 3.5f;
            }
            movement.y = move;
            StartCoroutine(SwimCooldown());
        }
    }

    // Apply opposite force to player movement to imitate friction
    private void ApplyFriction(Vector2 inputVector) 
    {
        if (isGrounded && Mathf.Abs(inputVector.x) < 0.01f && (currentCam == 1 || currentCam == 3) || ladderScript.onLadder && Mathf.Abs(inputVector.x) < 0.01f && (currentCam == 1 || currentCam == 3))
        {
            float amount = Mathf.Min(Mathf.Abs(Rb.velocity.z), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(Rb.velocity.z);
            Rb.AddForce(Vector3.forward * -amount, ForceMode.Impulse);
        }
        else if (isGrounded && Mathf.Abs(inputVector.x) < 0.01f && (currentCam == 0 || currentCam == 2) || ladderScript.onLadder && Mathf.Abs(inputVector.x) < 0.01f && (currentCam == 0 || currentCam == 2))
        {
            float amount = Mathf.Min(Mathf.Abs(Rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(Rb.velocity.x);
            Rb.AddForce(Vector3.right * -amount, ForceMode.Impulse);
        }
    }

    // Execute Jump only when certain conditions are met eg. when not jumping
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
        if (Rb.velocity.y < 0 && Rb.useGravity == true && !inWater)
        {
            Rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        if (Rb.useGravity == true && !inWater)
        {
            Rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
        }
        if (inWater)
        {
            Rb.AddForce(Physics.gravity * 0.5f, ForceMode.Acceleration);
        }
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
        }
        else
        {
            nearEdge = false;
        }

        return nearEdge;
    }

    //-----------------------------------------//
    // Jump
    //-----------------------------------------//
    // Jump Input to be used when the player presses the Jump button in the input system
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
    // Look input to be used when the player rotates the camera using left and right x axis input
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
        if (currentCam == 0 | currentCam == 2)
        {
            Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
        else if (currentCam == 1 | currentCam == 3)
        {
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
            Hurt(collision.transform.position);
        }
        else if (collision.gameObject.name == "KillPlane")
        {
            Respawn();
        }
        else if (collision.gameObject.tag == "JumpTag" && !isJumpTrampoline)
        {
            coyoteTimeCounter = 0f;
            StartCoroutine(TrampolineCooldown());
            Rb.velocity = Vector3.zero;
            Rb.AddForce(Vector3.up * jumpTrampolineHeight, ForceMode.Impulse);
        }
        else if (collision.gameObject.tag == "EnemyHead")
        {
            Destroy(collision.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            setCheckpoint(collision);
        }
        else if (collision.gameObject.tag == "HurtTag1" && !isHurt)
        {
            Hurt(collision.transform.position);
        }
    }

    // Reset Player health to maxHealth
    private void ResetPlayerHealth()
    {
        if (GameObject.Find("GlobalGameState").GetComponent<GameState>().IsEasy())
        {
            maxHealth = 2;
        }
        currentHealth = maxHealth;
    }

    public void MenuIncreaseHealth()
    {
        int setHealth;
        if (GameObject.Find("GlobalGameState").GetComponent<GameState>().IsEasy())
        {
            setHealth = 2;
        }
        else
        {
            setHealth = 1;
        }
        bool hpFlag = false;
        if (maxHealth == currentHealth)
        {
            hpFlag = true;
        }
        maxHealth = setHealth;
        if (hpFlag)
        {
            currentHealth = maxHealth;
        }
    }

    // Set player to lastGroundedPosition and reset their health
    private void Respawn()
    {
        ResetPlayerHealth();
        if (GameObject.Find("GlobalGameState").GetComponent<GameState>().isNormal())
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            transform.position = lastGroundedPosition;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private void setCheckpoint(Collider other)
    {
        checkpoint = other.transform.position;
    }

    private void RespawnAtCheckpoint()
    {
        ResetPlayerHealth();
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        transform.position = checkpoint;
    }

    public void Hurt()
    {

        currentHealth -= 1;       
        StartCoroutine(HurtCooldown());
        if (currentHealth <= 0) {
            if (GameObject.Find("GlobalGameState").GetComponent<GameState>().isNormal())
            {
                RespawnAtCheckpoint();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
           
        }
        else
        {
            Rb.AddForce(Vector3.up * 12f, ForceMode.Impulse);
        }
    }

    private void Hurt(Vector3 hurter)
    {

        currentHealth -= 1;       
        StartCoroutine(HurtCooldown());
        if (currentHealth <= 0) {
            if (GameObject.Find("GlobalGameState").GetComponent<GameState>().isNormal())
            {
                RespawnAtCheckpoint();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
           
        }
        else
        {
            Rb.AddForce(((transform.position - hurter).normalized) * 12f, ForceMode.VelocityChange);
        }
    }


    //-----------------------------------------//
    // Cooldowns (Coroutines)
    //-----------------------------------------//
    // Coroutines that act as cooldowns to ensure that action is only performed at specified intervals
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

    private IEnumerator SwimCooldown()
    {
        swimming = true;
        yield return new WaitForSeconds(0.3f);
        swimming = inWater;
    }
}