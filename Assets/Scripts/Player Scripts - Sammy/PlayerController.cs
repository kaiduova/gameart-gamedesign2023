//Written by Sammy
using UnityEngine;
using UnityEngine.UI;
using Input;
using System.Collections;
using UnityEditor.SceneManagement;

public class PlayerController : InputMonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum PlayerStates { 
        NeutralMovement, 
        Hacking, 
        HackingBox,
        Shockwave,
        StateDelay
    };

    [Header("Internally Referenced Components")]
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] BoxCollider2D _boxCollider2D;
    
    //Used by bounce pad.
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public float JumpBufferDuration => _jumpBufferDuration;
    public float CoyoteTimeDuration => _coyoteTimeDuration;

    
    [Header("Externally Referenced Components")]
    public GameObject playerBullet;

    [Header("Current State")]
    public PlayerStates PlayerCurrentState;


    [Header("Player Health Components")]
    [SerializeField] Image _heart1Image;
    [SerializeField] Image _heart2Image;
    [SerializeField] Image _heart3Image;

    [SerializeField] Sprite _heartFull;
    [SerializeField] Sprite _heartEmpty;

    [Header("Ground Check")]
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
    float _verticalInput; //Make public if having issues with horizontal movement

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


    //Health
    public int PlayerHealth;
    public float PlayerRespawnDelay;

    //Shockwave
    public GameObject Shockwave;
    public Slider ShockwaveCharge;

    //Boxes
    public GameObject currentHackedBox;


    public PhysicsMaterial2D PhysicsMaterial2D;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Start() {
        initialGravity = new Vector2(0, -Physics2D.gravity.y);

        PlayerHealth = 3;
        Shockwave.SetActive(false);

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

    public bool PlayerCurrentlyGrounded() { 
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

    private void PlayerHealthSystem() {
        if (PlayerHealth > 3) PlayerHealth = 3;
        else if (PlayerHealth < 1) PlayerHealth = 0;

        switch (PlayerHealth) {
            case 3: {
                _heart3Image.sprite = _heartFull;
                _heart2Image.sprite = _heartFull;
                _heart1Image.sprite = _heartFull;
            } break;
            case 2: {
                _heart3Image.sprite = _heartEmpty;
                _heart2Image.sprite = _heartFull;
                _heart1Image.sprite = _heartFull;
            } break;
            case 1: {
                _heart3Image.sprite = _heartEmpty;
                _heart2Image.sprite = _heartEmpty;
                _heart1Image.sprite = _heartFull;
            } break;
            case 0:{
                _heart3Image.sprite = _heartEmpty;
                _heart2Image.sprite = _heartEmpty;
                _heart1Image.sprite = _heartEmpty;
            } break; 
        }
    }

    public void ShockwaveReset() {
        Shockwave.SetActive(false);
        _rigidbody2D.gravityScale = 3;
        PlayerCurrentState = PlayerController.PlayerStates.NeutralMovement;
    }


    private void ControllerHackedBlock(GameObject currentHackedBlock) {

        Rigidbody2D currentHackedBlockRigidBody2D = currentHackedBlock.GetComponent<Rigidbody2D>();
        currentHackedBlockRigidBody2D.gravityScale = 0;

        float horizontalMaxSpeed = _horizontalInput * (_movementSpeed * 2);
        float horizontalSpeedDifference = horizontalMaxSpeed - currentHackedBlockRigidBody2D.velocity.x;
        float horizontalAccelerationRate = (Mathf.Abs(horizontalMaxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float horizontalSpeedApplied = Mathf.Pow(Mathf.Abs(horizontalSpeedDifference) * horizontalAccelerationRate, _movementPower) * Mathf.Sign(horizontalSpeedDifference);
        currentHackedBlockRigidBody2D.AddForce(horizontalSpeedApplied * Vector2.right);

        float verticalMaxSpeed = _verticalInput * (_movementSpeed *2);
        float verticalSpeedDifference = verticalMaxSpeed - currentHackedBlockRigidBody2D.velocity.y;
        float verticalAccelerationRate = (Mathf.Abs(verticalMaxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float verticalSpeedApplied = Mathf.Pow(Mathf.Abs(verticalSpeedDifference) * verticalAccelerationRate, _movementPower) * Mathf.Sign(verticalSpeedDifference);
        currentHackedBlockRigidBody2D.AddForce(verticalSpeedApplied * Vector2.up);

        if (CurrentInput.GetKeyDownB) {
            currentHackedBlockRigidBody2D.gravityScale = 3;
            currentHackedBox = null;

            gameObject.AddComponent<Rigidbody2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _rigidbody2D.sharedMaterial = PhysicsMaterial2D;
            _rigidbody2D.simulated = true;
            _rigidbody2D.gravityScale = 3;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rigidbody2D.freezeRotation = true;
            
            PlayerCurrentState = PlayerStates.NeutralMovement;
        }
    }



    private void FixedUpdate() {
        if (PlayerCurrentState is PlayerStates.NeutralMovement or PlayerStates.Hacking) HorizontalMovement();
    }

    private void Update() {
        PlayerAnimation();

        PlayerHealthSystem();

        if (PlayerCurrentState is PlayerStates.NeutralMovement or PlayerStates.Hacking) {
            _horizontalInput = CurrentInput.LeftStick.x;
            ReticleRotation_ProjectileFiring();
            UltimateJump();
        }


        if (PlayerCurrentState == PlayerStates.NeutralMovement) {

            if (CurrentInput.GetKeyDownY)
            {
                PlayerCurrentState = PlayerStates.Shockwave;
            }
        }

        if (PlayerCurrentState == PlayerStates.Shockwave)
        {
            Shockwave.SetActive(true);

            _horizontalInput = 0;
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);

        }


       if (PlayerCurrentState == PlayerStates.HackingBox)
        {


            _horizontalInput = CurrentInput.LeftStick.x;
            _verticalInput = CurrentInput.LeftStick.y;


            ControllerHackedBlock(currentHackedBox);
        }




        if (PlayerCurrentState == PlayerStates.StateDelay) {
            _horizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);
            PlayerRespawnDelay -= Time.deltaTime;
            if (PlayerRespawnDelay < 0) {
                PlayerRespawnDelay = 0;
                PlayerCurrentState = PlayerStates.NeutralMovement;
            }
        }
    }
}