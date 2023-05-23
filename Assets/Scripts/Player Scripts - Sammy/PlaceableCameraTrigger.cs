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
    [SerializeField] private CompositeCollider2D _currentCameraConfiner;
    [SerializeField] private CompositeCollider2D _cameraConfinerToChangeTo;

    [Header("Camera Trigger Type")]
    [SerializeField] private bool _triggerType;
    [TextArea] [SerializeField] private string _triggerTypeNote = "TRUE = Camera Trigger, used to frame shots\n" +
                                                                  "FALSE = Confiner Trigger, used to swap the camera's confining bounds";

    [Header("Trigger Attributes")]
    public float DesiredOthroSize;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) { //Player Layer
            if (_triggerType) {
                CameraController.PassedOrthoSize = DesiredOthroSize;
                CameraController.NewFollowPos = _linkedCameraFocalPoint;
                CameraController.CurrentState = CameraStates.FramingShot;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!_triggerType) CameraController.CinemachineConfiner.m_BoundingShape2D = _cameraConfinerToChangeTo;
    }
   
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) {
            if (_triggerType) CameraController.CurrentState = CameraStates.FollowingPlayer;
            if (!_triggerType) CameraController.CinemachineConfiner.m_BoundingShape2D = _currentCameraConfiner;
        }
    }

    private void Awake() {
        if (_triggerType) _linkedCameraFocalPoint = transform.GetChild(0).GetComponent<Transform>();
    }
}