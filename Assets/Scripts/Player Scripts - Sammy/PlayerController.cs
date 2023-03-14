//Written by Sammy
using UnityEngine;
using Input; 

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : InputMonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum PlayerStates { NeutralMovement, Hacking };

    [Header("Internally Referenced Components")]
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] BoxCollider2D _boxCollider2D;

    [Header("Externally Referenced Components")]
    public GameObject playerBullet;

    [Header("Current State")]
    public PlayerStates PlayerCurrentState;

    [Header("Ground Check Attributes")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 groundCheckSize = new Vector2(0.8f, 0.2f);
    [SerializeField] LayerMask groundLayer;

    [Header("Horizontal Movement Attributes")]
    [SerializeField] float _movementSpeed = 5;
    [SerializeField] float _jumpPeakMovementSpeed = 7.5f;
    [SerializeField] float _currentMovementSpeed;
    [SerializeField] float _accelerationIntensity = 10; //Change to a lower number if you want ice physics
    [SerializeField] float _deccelerationIntensity = 20; //Change to a higher number if you want ice physics
    float _movementPower = 1; //Do not change
    float _horizontalInput; //Make public if having issues with horizontal movement

    [Header("Jump Attributes")]
    [SerializeField] float _jumpForce = 12.5f;
    [SerializeField] float _maxJumpDuration = 0.5f; //Do not change
    [SerializeField] float _jumpDuration;
    float _jumpForceMultiplier = 1f; //Do not change
    [SerializeField] float _fallGravityMultiplier = 1.5f; //Do not change
    [SerializeField] float _maxFallSpeed = 20f;
    [SerializeField] float _coyoteTimeWindow = 0.2f; //Increase for a bigger window, decrease for a smaller window
    float _coyoteTimeDuration;
    [SerializeField] float _jumpBufferWindow = 0.2f; //Increase for a bigger window, decrease for a smaller window
    float _jumpBufferDuration;
    bool _currentlyJumping;
    Vector2 initialGravity;

    //NEW UNORGANISED
    public Transform playerPivotPoint;
    public GameObject playerReticle;
    Vector3 reticleRotation;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Start() {
        initialGravity = new Vector2(0, -Physics2D.gravity.y);
        PlayerCurrentState = PlayerStates.NeutralMovement;
    }

    private void PlayerAnimation() {
        if (_horizontalInput < 0) transform.eulerAngles = new Vector3(0, 180, 0);
        if (_horizontalInput > 0) transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void HorizontalMovement() {
        float maxSpeed = _horizontalInput * _currentMovementSpeed;
        float speedDifference = maxSpeed - _rigidbody2D.velocity.x;
        float accelerationRate = (Mathf.Abs(maxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float speedApplied = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, _movementPower) * Mathf.Sign(speedDifference);
        _rigidbody2D.AddForce(speedApplied * Vector2.right); 
    }

    bool PlayerCurrentlyGrounded() { 
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
    }

    private void UltimateJump() {
        if (PlayerCurrentlyGrounded()) {
            _currentMovementSpeed = _movementSpeed;
            _coyoteTimeDuration = _coyoteTimeWindow;
        }
        else _coyoteTimeDuration -= Time.deltaTime;
        
        if ((CurrentInput.GetKeyDownA || CurrentInput.GetKeyDownLT)) _jumpBufferDuration = _jumpBufferWindow;
        else _jumpBufferDuration -= Time.deltaTime;
        
        if (_jumpBufferDuration > 0 && _coyoteTimeDuration > 0) {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            _currentlyJumping = true;
            _jumpBufferDuration = 0;
            _jumpDuration = 0;
        }

        if (_rigidbody2D.velocity.y > 0 && _currentlyJumping)  {
            _jumpDuration += Time.deltaTime;
            if (_jumpDuration > _maxJumpDuration - 0.2f && _jumpDuration < _maxJumpDuration + 0.1f) _currentMovementSpeed = _jumpPeakMovementSpeed;
            if (_jumpDuration > _maxJumpDuration) _currentlyJumping = false;
            float currentJumpDuration = _jumpDuration / _maxJumpDuration;
            float currentJumpForceMultiplier = _jumpForceMultiplier;
            if (currentJumpDuration > 0.5f) currentJumpForceMultiplier = _jumpForceMultiplier * (1 - currentJumpDuration);
            _rigidbody2D.velocity += (initialGravity * currentJumpForceMultiplier) * Time.deltaTime;
        }

        if ((CurrentInput.GetKeyUpA || CurrentInput.GetKeyUpLT)) {
            _currentlyJumping = false;
            _coyoteTimeDuration = 0;
            _jumpBufferDuration = 0;
            _jumpDuration = 0;
            if (_rigidbody2D.velocity.y > 0) _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, (_rigidbody2D.velocity.y * 0.5f));
        }

        if (!(_rigidbody2D.velocity.y < 0)) return;
        _rigidbody2D.velocity -= (initialGravity * _fallGravityMultiplier) * Time.deltaTime;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Max(_rigidbody2D.velocity.y, -_maxFallSpeed));
    }

    private void ReticleRotation_ProjectileFiring() {
       
        if (PlayerCurrentState == PlayerStates.NeutralMovement) {
            if (CurrentInput.RightStick.x != 0 || CurrentInput.RightStick.y != 0) playerPivotPoint.gameObject.SetActive(true);
            else playerPivotPoint.gameObject.SetActive(false);
            if (playerPivotPoint.gameObject.activeInHierarchy) {
                reticleRotation = new Vector3(CurrentInput.RightStick.x, CurrentInput.RightStick.y);
                var appliedRotation = Quaternion.LookRotation(Vector3.forward, reticleRotation);
                playerPivotPoint.rotation = appliedRotation;
                if (CurrentInput.GetKeyDownRT) Instantiate(playerBullet, playerPivotPoint.position, Quaternion.identity);
            }
        }
        
        /*if (PlayerCurrentState == PlayerStates.neutralMovement) {
            playerPivotPoint.gameObject.SetActive(true);
            reticleRotation = new Vector3(CurrentInput.RightStick.x, CurrentInput.RightStick.y);
            var appliedRotation = Quaternion.LookRotation(Vector3.forward, reticleRotation);
            playerPivotPoint.rotation = appliedRotation;
            if (CurrentInput.GetKeyDownRT) Instantiate(playerBullet, playerPivotPoint.position, Quaternion.identity);
        }*/

        if (PlayerCurrentState == PlayerStates.Hacking) playerPivotPoint.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        if (PlayerCurrentState is PlayerStates.NeutralMovement or PlayerStates.Hacking) HorizontalMovement();
    }

    private void Update() {
        PlayerAnimation();

        if (PlayerCurrentState is PlayerStates.NeutralMovement or PlayerStates.Hacking) {
            _horizontalInput = CurrentInput.LeftStick.x;
            ReticleRotation_ProjectileFiring();
            UltimateJump();
        }
    }
}