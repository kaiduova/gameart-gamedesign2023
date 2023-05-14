using Input;
using UnityEngine;
using static PlayerController;

public class PlayerAnimation : InputMonoBehaviour {


    [SerializeField] public GameObject _player;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private ArcProjectileLauncher _arcLauncher;
    [SerializeField] private Animator _animator;

    public ParticleSystem DustJumpRunEffect;

    public ParticleSystem JumpEffect;
    public ParticleSystem LandEffect;


    private void Awake() {





        _playerController = _player.GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    void Start() {
    }

    private void PlayJumpEffect()
    {
        JumpEffect.Play();
    }


    private void PlayLandEffect()
    {
        LandEffect.Play();
    }



    void Update()
    {
        if (_playerController.CurrentState 
            is PlayerStates.NeutralMovement
            or PlayerStates.GhostHandMode
           ) {

            if (_playerController.HorizontalInput < 0) transform.eulerAngles = new Vector3(0, 0, 0);
            if (_playerController.HorizontalInput > 0) transform.eulerAngles = new Vector3(0, 180, 0);


            if (CurrentInput.GetKeyDownRT && _arcLauncher.Cooldown < 0f) _animator.SetTrigger("shoot");

            _animator.SetFloat("xVelocity", _playerController.HorizontalInput);
            _animator.SetFloat("yVelocity", _playerController.Rigidbody2D.velocity.y);
            _animator.SetBool("grounded", _playerController.PlayerCurrentlyGrounded());
            _animator.SetBool("knockback", false);


        }

        if (_playerController.CurrentState == PlayerStates.Knockback)
        {
            _animator.SetBool("knockback", true);
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
