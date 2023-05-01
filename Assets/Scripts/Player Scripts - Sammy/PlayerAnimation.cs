using Unity.VisualScripting;
using UnityEngine;
using static PlayerController;

public class PlayerAnimation : MonoBehaviour {


    [SerializeField] public GameObject _player;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Animator _animator;

    private void Awake() {





        _playerController = _player.GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    void Start() {
    }

    void Update()
    {
        if (_playerController.CurrentState 
            is PlayerStates.NeutralMovement
            or PlayerStates.GhostHandMode
           ) {

            if (_playerController.HorizontalInput < 0) transform.eulerAngles = new Vector3(0, 0, 0);
            if (_playerController.HorizontalInput > 0) transform.eulerAngles = new Vector3(0, 180, 0);



            _animator.SetFloat("xVelocity", _playerController.HorizontalInput);
            _animator.SetFloat("yVelocity", _playerController.Rigidbody2D.velocity.y);
            _animator.SetBool("grounded", _playerController.PlayerCurrentlyGrounded());

        }


        if (_playerController.CurrentState == PlayerStates.GhostHandBufferIntro) _animator.SetBool("ghostHandStart", true);

        if (_playerController.CurrentState == PlayerStates.GhostHandMode) {
            _animator.SetBool("ghostHandStart", false);
            _animator.SetBool("ghostHandMode", true);
        }

        if (_playerController.CurrentState == PlayerStates.GhostHandBufferOutro) _animator.SetBool("ghostHandMode", false);
    }
}
