//Written by Sammy
using UnityEngine;
using UnityEngine.UI;
using Input;

public class PlayerController : InputMonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum PlayerStates { 
        NeutralMovement, 
        Hacking, 
        HackingBox,
        Shockwave,
        StateDelay,

        SummoningGhostHand,
        GhostHandMode,
        DismissingGhostHand,
        GhostHandBufferIntro,
        GhostHandBufferOutro,
        GhostHandGeneralBuffer
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
    public PlayerStates CurrentState;


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
    [SerializeField] float _currentMovementSpeed;
    [SerializeField] float _movementSpeed = 5;
    [SerializeField] float _jumpPeakMovementSpeed = 7.5f;
    [SerializeField] float _accelerationIntensity = 10; //Change to a lower number if you want ice physics
    [SerializeField] float _deccelerationIntensity = 20; //Change to a higher number if you want ice physics
    float _movementPower = 1; //Do not change
    public float HorizontalInput; //Make public if having issues with horizontal movement
    public float VerticalInput; //Make public if having issues with horizontal movement

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


    //Ghost Hand 
    public GhostHand ghostHand;

    public GameObject PlayerCanvas, GhostHandObject;
    public Image GaugeFill;
    public float GhostHandMovementSpeed; //Affects player not hand

    public float SummonDuration;
    public float SummonInputDuration;


    private float _ghostHandBufferOutroDuration;
    [SerializeField] float _ghostHandBufferOutroDurationReset;

    private float _ghostHandBufferIntroDuration;
    [SerializeField] float _ghostHandBufferIntroDurationReset;

    public  float _ghostHandInputBufferDuration;
    [SerializeField] float _ghostHandInputBufferDurationReset;
    
    private bool _nextJumpBoosted;
    
    [SerializeField] private float additionalJumpBounceForce;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Start() {
        initialGravity = new Vector2(0, -Physics2D.gravity.y);

        PlayerHealth = 3;
        Shockwave.SetActive(false);
        PlayerCanvas.SetActive(false);

        CurrentState = PlayerStates.NeutralMovement;
    }

    private void PlayerAnimation() {
        //if (HorizontalInput < 0) transform.eulerAngles = new Vector3(0, 180, 0);
        //if (HorizontalInput > 0) transform.eulerAngles = new Vector3(0, 0, 0);

        if (!(PlayerCanvas.activeInHierarchy)) return;
        PlayerCanvas.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void HorizontalMovement() {
        float maxSpeed = HorizontalInput * _currentMovementSpeed;
        float speedDifference = maxSpeed - _rigidbody2D.velocity.x;
        float accelerationRate = (Mathf.Abs(maxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float speedApplied = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, _movementPower) * Mathf.Sign(speedDifference);
        _rigidbody2D.AddForce(speedApplied * Vector2.right); 
    }

    public bool PlayerCurrentlyGrounded() { 
        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, groundCheckSize, 0f, Vector2.zero, 0f, groundLayer);
        if (hit.collider != null && hit.collider.TryGetComponent<BouncePad>(out var bouncePad) && bouncePad.canBounce)
        {
            _nextJumpBoosted = true;
        }
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
        
        if (_jumpBufferDuration > 0 && _coyoteTimeDuration > 0)
        {
            if (_nextJumpBoosted)
            {
                _rigidbody2D.velocity += new Vector2(0, additionalJumpBounceForce + _jumpForce);
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            }

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
            _nextJumpBoosted = false;
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
        if (CurrentState == PlayerStates.NeutralMovement) {
            if (CurrentInput.RightStick.x != 0 || CurrentInput.RightStick.y != 0) playerPivotPoint.gameObject.SetActive(true);
            else playerPivotPoint.gameObject.SetActive(false);
            if (playerPivotPoint.gameObject.activeInHierarchy) {
                reticleRotation = new Vector3(CurrentInput.RightStick.x, CurrentInput.RightStick.y);
                var appliedRotation = Quaternion.LookRotation(Vector3.forward, reticleRotation);
                playerPivotPoint.rotation = appliedRotation;
                //if (CurrentInput.GetKeyDownRT) Instantiate(playerBullet, playerPivotPoint.position, Quaternion.identity);
            }
        }
        
        /*if (PlayerCurrentState == PlayerStates.neutralMovement) {
            playerPivotPoint.gameObject.SetActive(true);
            reticleRotation = new Vector3(CurrentInput.RightStick.x, CurrentInput.RightStick.y);
            var appliedRotation = Quaternion.LookRotation(Vector3.forward, reticleRotation);
            playerPivotPoint.rotation = appliedRotation;
            if (CurrentInput.GetKeyDownRT) Instantiate(playerBullet, playerPivotPoint.position, Quaternion.identity);
        }*/

        if (CurrentState == PlayerStates.Hacking) playerPivotPoint.gameObject.SetActive(false);
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
        CurrentState = PlayerController.PlayerStates.NeutralMovement;
    }

    private void ControllerHackedBlock(GameObject currentHackedBlock) {
        Rigidbody2D currentHackedBlockRigidBody2D = currentHackedBlock.GetComponent<Rigidbody2D>();
        currentHackedBlockRigidBody2D.gravityScale = 0;

        float horizontalMaxSpeed = HorizontalInput * (_movementSpeed * 2);
        float horizontalSpeedDifference = horizontalMaxSpeed - currentHackedBlockRigidBody2D.velocity.x;
        float horizontalAccelerationRate = (Mathf.Abs(horizontalMaxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float horizontalSpeedApplied = Mathf.Pow(Mathf.Abs(horizontalSpeedDifference) * horizontalAccelerationRate, _movementPower) * Mathf.Sign(horizontalSpeedDifference);
        currentHackedBlockRigidBody2D.AddForce(horizontalSpeedApplied * Vector2.right);

        float verticalMaxSpeed = VerticalInput * (_movementSpeed *2);
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
            
            CurrentState = PlayerStates.NeutralMovement;
        }
    }

    private void FixedUpdate() {
        if (CurrentState 
            is PlayerStates.NeutralMovement
            or PlayerStates.Hacking
            or PlayerStates.SummoningGhostHand
            or PlayerStates.GhostHandMode
            or PlayerStates.DismissingGhostHand) {
            HorizontalMovement();
        }
    }

    private void Update() {
        PlayerAnimation();
        PlayerHealthSystem();

        if (CurrentState == PlayerStates.NeutralMovement
            || CurrentState == PlayerStates.Hacking
            || CurrentState == PlayerStates.SummoningGhostHand
            || CurrentState == PlayerStates.GhostHandMode
            || CurrentState == PlayerStates.DismissingGhostHand)
        { HorizontalInput = CurrentInput.LeftStick.x; }

        if (CurrentState is PlayerStates.NeutralMovement or PlayerStates.Hacking or PlayerStates.SummoningGhostHand) {
            ReticleRotation_ProjectileFiring();
            UltimateJump();
        }
        if (CurrentState == PlayerStates.NeutralMovement) {
            _currentMovementSpeed = _movementSpeed;
            if (CurrentInput.GetKeyDownY) CurrentState = PlayerStates.Shockwave;

            SummonInputDuration = 0;
            PlayerCanvas.SetActive(false);

            if (PlayerCurrentlyGrounded()) if (CurrentInput.GetKeyRB) {
                PlayerCanvas.SetActive(true);
                CurrentState = PlayerStates.SummoningGhostHand;
            }
        }

        if (CurrentState == PlayerStates.SummoningGhostHand) {
            PlayerCanvas.SetActive(true);
            GaugeFill.fillAmount = SummonInputDuration;
            _currentMovementSpeed = GhostHandMovementSpeed;

            if (CurrentInput.GetKeyUpRB || !PlayerCurrentlyGrounded())  CurrentState = PlayerStates.NeutralMovement;
            
            if (CurrentInput.GetKeyRB) {
                SummonInputDuration += Time.deltaTime;
                if (SummonInputDuration > 1) {
                    SummonInputDuration = 0;
                    GhostHandObject.SetActive(true);
                    ghostHand.CurrentState = GhostHand.GhostHandStates.Summoning;
                    CurrentState = PlayerStates.GhostHandBufferIntro;
                }
            }
        }

        if (CurrentState == PlayerStates.GhostHandMode) {
            SummonInputDuration = 0;
            PlayerCanvas.SetActive(false);
            _currentMovementSpeed = GhostHandMovementSpeed;

            if (PlayerCurrentlyGrounded() && CurrentInput.GetKeyRB) {
                _ghostHandInputBufferDuration += Time.deltaTime;
                if (_ghostHandInputBufferDuration >= _ghostHandInputBufferDurationReset) {
                    PlayerCanvas.SetActive(true);
                    CurrentState = PlayerStates.DismissingGhostHand;
                    if (ghostHand.currentBlock != null) ghostHand.CurrentState = GhostHand.GhostHandStates.DroppingBlock;
                }
            } else _ghostHandInputBufferDuration = 0;
        }

        if (CurrentState == PlayerStates.DismissingGhostHand) {
            PlayerCanvas.SetActive(true);
            _currentMovementSpeed = GhostHandMovementSpeed;
            GaugeFill.fillAmount = SummonInputDuration;

            if (CurrentInput.GetKeyUpRB || !PlayerCurrentlyGrounded()) CurrentState = PlayerStates.GhostHandMode;

            if (CurrentInput.GetKeyRB) {
                SummonInputDuration += Time.deltaTime;
                if (SummonInputDuration > 1) {
                    SummonInputDuration = 0;
                    ghostHand.CurrentState = GhostHand.GhostHandStates.Dismissing;
                    CurrentState = PlayerStates.GhostHandBufferOutro;
                }
            }
        }

        if (CurrentState == PlayerStates.GhostHandBufferIntro) {
            HorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);

            SummonInputDuration = 0;
            PlayerCanvas.SetActive(false);

            _ghostHandBufferIntroDuration += Time.deltaTime;
            if(_ghostHandBufferIntroDuration >= _ghostHandBufferIntroDurationReset) {
                _ghostHandBufferIntroDuration = 0;
                ghostHand.CurrentState = GhostHand.GhostHandStates.SearchingForBlock;
                CurrentState = PlayerStates.GhostHandMode;
            }
        }

        if (CurrentState == PlayerStates.GhostHandBufferOutro) {
            HorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);

            SummonInputDuration = 0;
            PlayerCanvas.SetActive(false);

            _ghostHandBufferOutroDuration += Time.deltaTime;
            if (_ghostHandBufferOutroDuration >= _ghostHandBufferOutroDurationReset) {
                _ghostHandBufferOutroDuration = 0;
                GhostHandObject.SetActive(false);
                CurrentState = PlayerStates.NeutralMovement;
            }
        }

        if (CurrentState == PlayerStates.Shockwave) {
            Shockwave.SetActive(true);

            HorizontalInput = 0;
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);
        }

        if (CurrentState == PlayerStates.HackingBox) {
            HorizontalInput = CurrentInput.LeftStick.x;
            VerticalInput = CurrentInput.LeftStick.y;
            ControllerHackedBlock(currentHackedBox);
        }

        if (CurrentState == PlayerStates.StateDelay) {
            HorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);
            PlayerRespawnDelay -= Time.deltaTime;
            if (PlayerRespawnDelay < 0) {
                PlayerRespawnDelay = 0;
                CurrentState = PlayerStates.NeutralMovement;
            }
        }
    }
}