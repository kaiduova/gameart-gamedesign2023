//Written by Sammy
using Input;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using static GhostHand;

public class PlayerController : InputMonoBehaviour {

    public enum PlayerStates { 
        NeutralMovement, 
        StateDelay,
        ControllingBullet,
        Knockback,
        Dead,
        SummoningGhostHand,
        GhostHandMode,
        DismissingGhostHand,
        GhostHandBufferIntro,
        GhostHandBufferOutro,
        GhostHandGeneralBuffer
    };

    [Header("Internally Referenced Components")]
    [SerializeField] Rigidbody2D _rigidbody2D;

    [Header("Current State")]
    public PlayerStates CurrentState;

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

    [Header("Player Health Components")]
    public int PlayerHealth;
    public float PlayerRespawnDelay;
    [SerializeField] private Image _heart1Image;
    [SerializeField] private Image _heart2Image;
    [SerializeField] private Image _heart3Image;
    [SerializeField] private Sprite _heartFull;
    [SerializeField] private Sprite _heartEmpty;

    [Header("Player Projectile Components")]
    public GameObject PlayerBullet;
    [SerializeField] private Transform _playerPivotPoint;
    private Vector3 _reticleRotation;

    [Header("Ghost Hand Components")]
    public GameObject GhostHandObject;
    public GhostHand GhostHand;
    public GameObject PlayerCanvas;
    public Image GaugeFill;
    public float GhostHandMovementSpeed; //Affects player not hand
    private float _summonInputDuration;
    private float _ghostHandBufferOutroDuration;
    [SerializeField] private float _ghostHandBufferOutroDurationReset;
    private float _ghostHandBufferIntroDuration;
    [SerializeField] private float _ghostHandBufferIntroDurationReset;
    private float _ghostHandInputBufferDuration;
    [SerializeField] private float _ghostHandInputBufferDurationReset;

    [Header("Kai's Additions")]
    [SerializeField] private float additionalJumpBounceForce;
    public float controlBulletInputBuffer;
    private bool _cancelNextJump;
    //Used by Bounce Pad
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public float JumpBufferDuration => _jumpBufferDuration;
    public float CoyoteTimeDuration => _coyoteTimeDuration;

    public IEnumerator Knockback(float knockBackDirection) {
        _rigidbody2D.velocity = new Vector2(knockBackDirection, ((_jumpForce / 3) * 2));
        yield return null;
    }

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Start() {
        initialGravity = new Vector2(0, -Physics2D.gravity.y);
        PlayerHealth = 3;
        PlayerCanvas.SetActive(false);
        CurrentState = PlayerStates.NeutralMovement;
    }
   
    private void PlayerAnimation() {
        if (HorizontalInput < 0) transform.eulerAngles = new Vector3(0, 180, 0);
        if (HorizontalInput > 0) transform.eulerAngles = new Vector3(0, 0, 0);
        if (!(PlayerCanvas.activeInHierarchy)) return;
        PlayerCanvas.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void HorizontalMovement() {
        float maxSpeed = HorizontalInput * _currentMovementSpeed;
        float speedDifference = maxSpeed - _rigidbody2D.velocity.x;
        float accelerationRate = (Mathf.Abs(maxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float speedApplied = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, _movementPower) * Mathf.Sign(speedDifference);
        _rigidbody2D.AddForce(speedApplied * Vector2.right * _rigidbody2D.mass); 
    }

    public bool PlayerCurrentlyGrounded() { 
        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, groundCheckSize, 0f, Vector2.zero, 0f, groundLayer);
        if (hit.collider != null && hit.collider.TryGetComponent<BouncePad>(out var bouncePad) && bouncePad.canBounce) _cancelNextJump = true;
        else _cancelNextJump = false;
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
            if (_cancelNextJump) _rigidbody2D.velocity += new Vector2(0, -_jumpForce);
            else _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
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
            _cancelNextJump = false;
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
            if (CurrentInput.RightStick.x != 0 || CurrentInput.RightStick.y != 0) _playerPivotPoint.gameObject.SetActive(true);
            else _playerPivotPoint.gameObject.SetActive(false);
            if (_playerPivotPoint.gameObject.activeInHierarchy) {
                _reticleRotation = new Vector3(CurrentInput.RightStick.x, CurrentInput.RightStick.y);
                var appliedRotation = Quaternion.LookRotation(Vector3.forward, _reticleRotation);
                _playerPivotPoint.rotation = appliedRotation;
            }
        }       
    }

    private void PlayerHealthSystem() {
        if (PlayerHealth > 3) PlayerHealth = 3;
        else if (PlayerHealth < 1) {
            PlayerHealth = 0;
            CurrentState = PlayerStates.Dead;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

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

    private void FixedUpdate() {
        if (CurrentState 
            is PlayerStates.NeutralMovement
            or PlayerStates.ControllingBullet
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
            || CurrentState == PlayerStates.SummoningGhostHand
            || CurrentState == PlayerStates.GhostHandMode
            || CurrentState == PlayerStates.DismissingGhostHand)
        { HorizontalInput = CurrentInput.LeftStick.x; }

        if (CurrentState is PlayerStates.NeutralMovement or PlayerStates.SummoningGhostHand) {
            ReticleRotation_ProjectileFiring();
            UltimateJump();
        }

        if (CurrentState == PlayerStates.NeutralMovement) {
            _currentMovementSpeed = _movementSpeed;
            _summonInputDuration = 0;
            PlayerCanvas.SetActive(false);

            if (PlayerCurrentlyGrounded() && CurrentInput.GetKeyRT) {
                controlBulletInputBuffer = 0;
                controlBulletInputBuffer += Time.deltaTime;
            }

            if (PlayerCurrentlyGrounded() && CurrentInput.GetKeyLB) {
                PlayerCanvas.SetActive(true);
                CurrentState = PlayerStates.SummoningGhostHand;
            }
        }

        if (CurrentState == PlayerStates.Knockback) {
            if (PlayerCurrentlyGrounded()) {
                PlayerHealth--;
                CurrentState = PlayerStates.NeutralMovement;
            }
        }

        if (CurrentState == PlayerStates.ControllingBullet) {
            _currentMovementSpeed = GhostHandMovementSpeed;
            if (CurrentInput.GetKeyRT || !PlayerCurrentlyGrounded()) CurrentState = PlayerStates.NeutralMovement;
        }

        if (CurrentState == PlayerStates.SummoningGhostHand) {
            PlayerCanvas.SetActive(true);
            GaugeFill.fillAmount = _summonInputDuration * 2;
            _currentMovementSpeed = GhostHandMovementSpeed;

            if (CurrentInput.GetKeyUpLB || !PlayerCurrentlyGrounded())  CurrentState = PlayerStates.NeutralMovement;
            
            if (CurrentInput.GetKeyLB) {
                _summonInputDuration += Time.deltaTime * 2;
                if (_summonInputDuration > 0.5f) {
                    _summonInputDuration = 0;
                    GhostHandObject.SetActive(true);
                    GhostHand.CurrentState = GhostHandStates.Summoning;
                    CurrentState = PlayerStates.GhostHandBufferIntro;
                }
            }
        }

        if (CurrentState == PlayerStates.GhostHandMode) {
            _summonInputDuration = 0;
            PlayerCanvas.SetActive(false);
            _currentMovementSpeed = GhostHandMovementSpeed;

            if (PlayerCurrentlyGrounded() && CurrentInput.GetKeyLB) {
                _ghostHandInputBufferDuration += Time.deltaTime;
                if (_ghostHandInputBufferDuration >= _ghostHandInputBufferDurationReset) {
                    PlayerCanvas.SetActive(true);
                    CurrentState = PlayerStates.DismissingGhostHand;
                    if (GhostHand.currentBlock != null) GhostHand.CurrentState = GhostHandStates.DroppingBlock;
                }
            } else _ghostHandInputBufferDuration = 0;
        }

        if (CurrentState == PlayerStates.DismissingGhostHand) {
            PlayerCanvas.SetActive(true);
            _currentMovementSpeed = GhostHandMovementSpeed;
            GaugeFill.fillAmount = _summonInputDuration * 2;

            if (CurrentInput.GetKeyUpLB || !PlayerCurrentlyGrounded()) CurrentState = PlayerStates.GhostHandMode;

            if (CurrentInput.GetKeyLB) {
                _summonInputDuration += Time.deltaTime * 2;
                if (_summonInputDuration > 0.5f) {
                    _summonInputDuration = 0;
                    GhostHand.CurrentState = GhostHandStates.Dismissing;
                    CurrentState = PlayerStates.GhostHandBufferOutro;
                }
            }
        }

        if (CurrentState == PlayerStates.GhostHandBufferIntro) {
            HorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);
            _summonInputDuration = 0;
            PlayerCanvas.SetActive(false);

            _ghostHandBufferIntroDuration += Time.deltaTime;
            if(_ghostHandBufferIntroDuration >= _ghostHandBufferIntroDurationReset) {
                _ghostHandBufferIntroDuration = 0;
                GhostHand.CurrentState = GhostHandStates.SearchingForBlock;
                CurrentState = PlayerStates.GhostHandMode;
            }
        }

        if (CurrentState == PlayerStates.GhostHandBufferOutro) {
            HorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);
            _summonInputDuration = 0;
            PlayerCanvas.SetActive(false);

            _ghostHandBufferOutroDuration += Time.deltaTime;
            if (_ghostHandBufferOutroDuration >= _ghostHandBufferOutroDurationReset) {
                _ghostHandBufferOutroDuration = 0;
                GhostHandObject.SetActive(false);
                CurrentState = PlayerStates.NeutralMovement;
            }
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