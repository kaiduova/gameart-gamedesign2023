//Written by Sammy
using Input; using UnityEngine;

public class PlayerController : InputMonoBehaviour {
    //PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    //camelCase - parameters, arguments, methodVariables, functionVariables
    //_camelCase - privateMemberVariables

    [Header("Internally Referenced Components")]
    public BoxCollider2D thisBC2D;
    public Rigidbody2D thisRB2D;

    [Header("Externally Referenced Components")]

    [Header("Current State")]
    public string playerCurrentState;

    [Header("Ground Check Attributes")]
    public LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius = 0.2f; //Do not change
    [SerializeField] bool currentlyGrounded;

    [Header("Horizontal Movement Attributes")]
    public float movementSpeed = 5;
    public float accelerationIntensity = 10; //Change to a lower number if you want ice physics
    public float deccelerationIntensity = 20; //Change to a higher number if you want ice physics
    float movementPower = 1;
    public float horizontalInput; //Make public if having issues with horizontal movement

    [Header("Jump Attributes")]
    public float jumpForce = 15;
    [SerializeField] private float jumpBufferReset = 0.2f;
    private float jumpBufferWindow;
    [SerializeField] private float coyoteTimeReset = 0.2f;
    private float coyoteTimeWindow;
    [SerializeField] private bool currentlyJumping;

    private void Awake() {
        thisRB2D = GetComponent<Rigidbody2D>();
        thisBC2D = GetComponent<BoxCollider2D>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Start() {
        playerCurrentState = "neutralMovement";
    }

    private void HorizontalMovement() {
        float maxSpeed = horizontalInput * movementSpeed;
        float speedDifference = maxSpeed - thisRB2D.velocity.x;
        float accelerationRate = (Mathf.Abs(maxSpeed) > 0.01f) ? accelerationIntensity : deccelerationIntensity;
        float speedApplied = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, movementPower) * Mathf.Sign(speedDifference);
        thisRB2D.AddForce(speedApplied * Vector2.right);
    }

    private void DeluxeJump() {
        if (currentlyGrounded) {
            currentlyJumping = false;
            coyoteTimeWindow = coyoteTimeReset;
        }
        else {
            currentlyJumping = true;
            coyoteTimeWindow -= Time.deltaTime;
            if (coyoteTimeWindow < 0) coyoteTimeWindow = 0;
        }
        if (CurrentInput.GetKeyDownA) jumpBufferWindow = jumpBufferReset;
        else {
            jumpBufferWindow -= Time.deltaTime;
            if (jumpBufferWindow < 0) jumpBufferWindow = 0;
        }
        if (coyoteTimeWindow > 0f && jumpBufferWindow > 0f) {
            thisRB2D.velocity = new Vector2(thisRB2D.velocity.x, jumpForce);
            jumpBufferWindow = 0;
        }
        if (CurrentInput.GetKeyDownA && thisRB2D.velocity.y > 0f) {
            thisRB2D.velocity = new Vector2(thisRB2D.velocity.x, thisRB2D.velocity.y * 0.5f);
            coyoteTimeWindow = 0;
        }
    }

    private void FixedUpdate() {
        currentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        /// Important Information Regarding The HorizontalMovement Function
        /// 
        /// HorizontalMovement needs to be called in FixedUpdate as there may funky physics stuff otherwise (it happened in Toubou Tako) 
        /// So if you want to disable player movement keep in mind that it is called here.
        /// However if the state is not "neutralMovement" then it will be stopped.
        /// But when hacking for example, I'll make a new state where the movement function runs as well as the hacking code locking out the intented abilites.
        /// 
    
        if (playerCurrentState == "neutralMovement") HorizontalMovement();
    }

    private void Update() {
        if (playerCurrentState == "neutralMovement") {
            horizontalInput = CurrentInput.LeftStick.x;
            DeluxeJump();
        }

        if (CurrentInput.GetKeyDownA) print("Hello");
    }
}