using UnityEngine;
using Cinemachine;


public class ChainBlock : MonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum ChainBlockStates { Chained, FreeFall, Static, Deactivated }

    [Header("Internally Referenced Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Externally Referenced Components")]
    [SerializeField] private PhysicsMaterial2D _chainBlockPhysicsMaterial2D;

    [Header("Current State")]
    public ChainBlockStates CurrentState;

    [Header("Chain Block Attributes")]
    [SerializeField] private float yStoppingPoint;

    [Header("Linked Camera Trigger Attributes")]
    [SerializeField] private GameObject _linkedCameraTrigger;
    [SerializeField] private float _triggerDisableDelay;

    public CinemachineImpulseSource ScreenShake;

    private void CallScreenShake() {
        ScreenShake.GenerateImpulse();
    }

    private void InitialiseRigidbody2D() {
        _rigidbody2D.sharedMaterial = _chainBlockPhysicsMaterial2D;
        _rigidbody2D.simulated = true;
        _rigidbody2D.mass = 10;
        _rigidbody2D.drag = 0;
        _rigidbody2D.angularDrag = 0;
        _rigidbody2D.gravityScale = 3;
        _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rigidbody2D.freezeRotation = true;
    } 

    private void Awake() {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D)) _rigidbody2D = rigidbody2D;
    }

    private void Start() {
        CurrentState = ChainBlockStates.Chained;
    }

    private void Update() {

        if (CurrentState == ChainBlockStates.Chained) {
            if (!(_rigidbody2D != null)) return;
            Destroy(_rigidbody2D);
        }

        if (CurrentState == ChainBlockStates.FreeFall) {
            if (transform.position.y <= (yStoppingPoint + 0.1f)) {
                transform.position = new Vector3(transform.position.x, yStoppingPoint);
                CurrentState = ChainBlockStates.Static;
            }
            if (!(_rigidbody2D == null)) return;
            gameObject.AddComponent<Rigidbody2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            InitialiseRigidbody2D();
        }

        if (CurrentState == ChainBlockStates.Static) {
            if (ScreenShake != null) CallScreenShake();
            if (_linkedCameraTrigger != null) {
                _triggerDisableDelay -= Time.deltaTime;
                if (_triggerDisableDelay <= 0) {
                    _linkedCameraTrigger.SetActive(false);
                    _triggerDisableDelay = 0;
                }
            }
            transform.position = new Vector3(transform.position.x, yStoppingPoint);
            if (!(_rigidbody2D != null)) return;
            Destroy(_rigidbody2D);
            CurrentState = ChainBlockStates.Deactivated;
        }

        if (CurrentState == ChainBlockStates.Deactivated) transform.position = new Vector3(transform.position.x, yStoppingPoint);
    }
}