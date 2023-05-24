//Written by Sammy
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    [Header("Externally Referenced Components")]
    GameObject _player;
    PlayerController _playerController;
    GameObject _playerReticle;

    [Header("Bullet Attributes")]
    public float DamageValue;
    Vector2 _currentTrajectory;
    float _projectileSpeed = 0.25f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 3) { //Ground
            if (collision.gameObject.tag == "HackableBlock") {
                if (_playerController.PlayerCurrentlyGrounded()) { 
                    //_playerController.currentHackedBox = collision.gameObject;
                    //_playerController.CurrentState = PlayerController.PlayerStates.HackingBox;
                    Rigidbody2D rigidbody2D = _player.GetComponent<Rigidbody2D>();
                    Destroy(rigidbody2D);
                } else Destroy(gameObject);
            } else Destroy(gameObject);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 8) { //Enemy layer
            collision.GetComponent<EnemyDummy_StartsMinigame>().StartMinigame();
            Destroy(gameObject);
        }
    }

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player").gameObject;
        _playerController = _player.GetComponent<PlayerController>();
        _playerReticle = GameObject.Find("PlayerReticle");
    }

    private void Start() {
        _currentTrajectory = (_playerReticle.transform.position - transform.position).normalized * _projectileSpeed;
        Destroy(gameObject, 2.5f);
    }

    private void Update() {
        transform.position += new Vector3(_currentTrajectory.x, _currentTrajectory.y);
    }
}