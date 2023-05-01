using UnityEngine;
using static CameraController;

public class PlaceableCameraTrigger : MonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    [Header("Internal Components")]
    [SerializeField] private Transform _linkedCameraFocalPoint;

    [Header("Externally Referenced Components")]
    public CameraController CameraController;

    [Header("Trigger Attributes")]
    public float DesiredOthroSize;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) { //Player Layer
            CameraController.PassedOrthoSize = DesiredOthroSize;
            CameraController.NewFollowPos = _linkedCameraFocalPoint;
            CameraController.CurrentState = CameraStates.FramedShot;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) CameraController.CurrentState = CameraStates.FollowingPlayer;
    }

    private void Awake() {
        _linkedCameraFocalPoint = transform.GetChild(0).GetComponent<Transform>();
    }
}