using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum CameraStates {
        FollowingPlayer,
        GhostHandActive,
        FramingShot,
        Static
    }

    [Header("Internal Components")]
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;

    [Header("Externally Referenced Components")]
    [SerializeField] private Transform _playerPos;
    public GameObject GhostHand;
    public Transform NewFollowPos;

    [Header("Current State")]
    public CameraStates CurrentState;

    [Header("Camera Attributes")]
    [SerializeField] private float _playerOrthoSize;
    [SerializeField] private float _ghostHandOrthoSize;
    [SerializeField] private float _zoomSpeed;
    public float PassedOrthoSize;





    public CinemachineConfiner CinemachineConfiner;





    private void Awake() {
        _playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start() {
        VirtualCamera.m_Lens.OrthographicSize = _playerOrthoSize;
    }

    private void CameraResising(float desiredOrthoSize) {
        if (VirtualCamera.m_Lens.OrthographicSize != desiredOrthoSize) {
            if (VirtualCamera.m_Lens.OrthographicSize > desiredOrthoSize) {
                VirtualCamera.m_Lens.OrthographicSize -= _zoomSpeed * Time.deltaTime;
                if (VirtualCamera.m_Lens.OrthographicSize <= desiredOrthoSize) VirtualCamera.m_Lens.OrthographicSize = desiredOrthoSize;
            }

            if (VirtualCamera.m_Lens.OrthographicSize < desiredOrthoSize) {
                VirtualCamera.m_Lens.OrthographicSize += _zoomSpeed * Time.deltaTime;
                if (VirtualCamera.m_Lens.OrthographicSize >= desiredOrthoSize) VirtualCamera.m_Lens.OrthographicSize = desiredOrthoSize;
            }
        }
    }

    void Update() {
        if (CurrentState == CameraStates.FollowingPlayer) {
            VirtualCamera.Follow = _playerPos;
            CameraResising(_playerOrthoSize);
            if (GhostHand.activeInHierarchy) CurrentState = CameraStates.GhostHandActive;
        }

        if (CurrentState == CameraStates.GhostHandActive) {
            VirtualCamera.Follow = _playerPos;
            CameraResising(_ghostHandOrthoSize);
            if (!GhostHand.activeInHierarchy) CurrentState = CameraStates.FollowingPlayer;
        }

        if (CurrentState == CameraStates.FramingShot) {
            VirtualCamera.Follow = NewFollowPos;
            CameraResising(PassedOrthoSize);
            if (GhostHand.activeInHierarchy) CurrentState = CameraStates.FramingShot;
        }

        if (CurrentState == CameraStates.Static) VirtualCamera.Follow = null;
    }
}