using Input;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostHand : InputMonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum GhostHandStates {
        Summoning,
        SearchingForBlock,
        GrabbingBlock,
        CarryingBlock,
        DroppingBlock,
        Dismissing,
    }

    [Header("Internal Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private SpriteRenderer _thisSR;

    [Header("Externally Referenced Components")]
    [SerializeField] private Sprite _openHand;
    [SerializeField] private Sprite _closedHand;
    public GameObject currentBlock;
    [SerializeField] private PhysicsMaterial2D _blockPhysicsMaterial2D;

    [Header("Current State")]
    public GhostHandStates CurrentState;

    [Header("Hand Attributes")]
    [SerializeField] private float _handMovementSpeed;
    [SerializeField] private float _accelerationIntensity = 1;
    [SerializeField] private float _deccelerationIntensity = 2.5f;
    private float _movementPower = 1; //Do not change
    private float _rightStickHorizontalInput;
    private float _rightStickVerticalInput;
    //Using low numbers for this to make the hand feel floaty and a little ghostly
    private float _grabDuration;
    [SerializeField] float _grabDurationReset;
    private float _dropDuration;
    [SerializeField] float _dropDurationReset;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "HackableBlock") {
            if (CurrentState == GhostHandStates.GrabbingBlock) {
                currentBlock = collision.gameObject;
                collision.gameObject.transform.SetParent(transform, true);
            }
        }
    }  

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _thisSR = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _thisSR.sprite = _openHand;
    }

    private void GhostHandAnimation() {
        if (_rightStickHorizontalInput < 0) transform.eulerAngles = new Vector3(0, 180, 0);
        if (_rightStickHorizontalInput > 0) transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void GhostHandMovement()  {
        float horizontalMaxSpeed = _rightStickHorizontalInput * _handMovementSpeed;
        float verticalMaxSpeed = _rightStickVerticalInput * _handMovementSpeed;
        float horizontalSpeedDifference = horizontalMaxSpeed - _rigidbody2D.velocity.x;
        float verticalSpeedDifference = verticalMaxSpeed - _rigidbody2D.velocity.y;
        float horizontalAccelerationRate = (Mathf.Abs(horizontalMaxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float verticalAccelerationRate = (Mathf.Abs(verticalMaxSpeed) > 0.01f) ? _accelerationIntensity : _deccelerationIntensity;
        float horizontalSpeedApplied = Mathf.Pow(Mathf.Abs(horizontalSpeedDifference) * horizontalAccelerationRate, _movementPower) * Mathf.Sign(horizontalSpeedDifference);
        float verticalSpeedApplied = Mathf.Pow(Mathf.Abs(verticalSpeedDifference) * verticalAccelerationRate, _movementPower) * Mathf.Sign(verticalSpeedDifference);
        _rigidbody2D.AddForce(horizontalSpeedApplied * Vector2.right);
        _rigidbody2D.AddForce(verticalSpeedApplied * Vector2.up);
    }

    private void FixedUpdate() {
        if (CurrentState
            is GhostHandStates.SearchingForBlock
            or GhostHandStates.CarryingBlock) {
            GhostHandMovement();
        }
    }

    void Update() {
        GhostHandAnimation();

        if (CurrentState == GhostHandStates.Summoning || CurrentState == GhostHandStates.Dismissing) {
            _rightStickHorizontalInput = 0;
            _rightStickHorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);
        }

        if (CurrentState == GhostHandStates.SearchingForBlock) {
            _rightStickHorizontalInput = CurrentInput.RightStick.x;
            _rightStickVerticalInput = CurrentInput.RightStick.y;

            _thisSR.sprite = _openHand;
            if (CurrentInput.GetKeyDownRB) CurrentState = GhostHandStates.GrabbingBlock;
        }

        if (CurrentState == GhostHandStates.GrabbingBlock) {
            _thisSR.sprite = _closedHand;
            _rightStickHorizontalInput = 0;
            _rightStickHorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);

            _grabDuration += Time.deltaTime;
            if (_grabDuration >= _grabDurationReset) {
                _grabDuration = 0;

                if (currentBlock != null) {
                    CurrentState = GhostHandStates.CarryingBlock;
                } else CurrentState = GhostHandStates.SearchingForBlock;
            }
        }

        if (CurrentState == GhostHandStates.CarryingBlock) {
            _rightStickHorizontalInput = CurrentInput.RightStick.x;
            _rightStickVerticalInput = CurrentInput.RightStick.y;

            if (currentBlock != null) {
                if (currentBlock.GetComponent<Rigidbody2D>()) {
                    Rigidbody2D blockRigidbody2D = currentBlock.GetComponent<Rigidbody2D>();
                    Destroy(blockRigidbody2D);
                }
            }

            if (CurrentInput.GetKeyDownRB) CurrentState = GhostHandStates.DroppingBlock;
        }

        if (CurrentState == GhostHandStates.DroppingBlock) {
            _rightStickHorizontalInput = 0;
            _rightStickHorizontalInput = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);

            _dropDuration += Time.deltaTime;
            if (_dropDuration >= _dropDurationReset) {
                _dropDuration = 0;

                currentBlock.AddComponent<Rigidbody2D>();
                Rigidbody2D blockRigidbody2D = currentBlock.GetComponent<Rigidbody2D>();
                blockRigidbody2D.sharedMaterial = _blockPhysicsMaterial2D;
                blockRigidbody2D.simulated = true;
                blockRigidbody2D.mass = 10;
                blockRigidbody2D.drag = 0;
                blockRigidbody2D.angularDrag = 0;
                blockRigidbody2D.gravityScale = 3;
                blockRigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                blockRigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
                blockRigidbody2D.freezeRotation = true;
                currentBlock.transform.SetParent(null, true);
                currentBlock = null;

                _thisSR.sprite = _openHand;
                CurrentState = GhostHandStates.SearchingForBlock;
            }
        }
    }
}