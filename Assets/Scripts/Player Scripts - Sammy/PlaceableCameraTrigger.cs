using UnityEngine;
using static CameraController;

public class PlaceableCameraTrigger : MonoBehaviour {

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
            CameraController.CurrentState = CameraStates.FramingShot;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) {
            CameraController.CurrentState = CameraStates.FollowingPlayer;
        }
    }
}