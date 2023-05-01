//Written by Sammy
using UnityEngine;
using Minigame; 
using Input;

[RequireComponent(typeof(BoxCollider2D),typeof(Animator))]
public class EnemyDummy_StartsMinigame : InputMonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum EnemyStates { neutralAggro, currentlyBeingHacked, hackSuccessful }

    [Header("Internally Referenced Components")]
    [SerializeField] BoxCollider2D _boxCollider2D;
    [SerializeField] Animator _animator;

    [Header("Externally Referenced Components")]
    public GameObject Player;

    [Header("Current State")]
    public EnemyStates EnemyCurrentState;

    [Header("Enemy Attributes")]
    [SerializeField] int EnemyID;

    private void EnemyAnimation() {
        if (EnemyCurrentState == EnemyStates.neutralAggro) {
            if (Player.transform.position.x < transform.position.x) transform.eulerAngles = new Vector3(0, 180, 0);
            if (Player.transform.position.x > transform.position.x) transform.eulerAngles = new Vector3(0, 0, 0);
        }
        //if (EnemyCurrentState == EnemyStates.neutralAggro) _animator.SetBool("_neutralAggro", true);
        if (EnemyCurrentState == EnemyStates.currentlyBeingHacked) {
            _animator.SetBool("_currentlyBeingHacked", true);
            //_animator.SetBool("_neutralAggro", false);
        }
        if (EnemyCurrentState == EnemyStates.hackSuccessful) {
            _animator.SetBool("_currentlyBeingHacked", false);
            _animator.SetBool("_hackSuccessful", true);

        }
    }

    public void StartMinigame() {
        EnemyCurrentState = EnemyStates.currentlyBeingHacked;
        Player.GetComponent<PlayerController>().CurrentState = PlayerController.PlayerStates.Hacking;
        MinigameModule.Instance.StartMinigame(Minigame.Minigame.SequenceCallAndResponce, EnemyID);
    }

    public void CommunicateWithMinigame() {
        EnemyCurrentState = EnemyStates.hackSuccessful;
        Player.GetComponent<PlayerController>().CurrentState = PlayerController.PlayerStates.NeutralMovement;
    }

    private void SetBodyToPlatform() {
        _boxCollider2D.size = new Vector2(3, 1);
        gameObject.layer = 3;
    }

    private void Awake() {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        if (EnemyID == 0 || EnemyID > 3) EnemyID = 1; //Failsafe
        EnemyCurrentState = EnemyStates.neutralAggro;
    }

    void Update() {
        EnemyAnimation();

    }
}