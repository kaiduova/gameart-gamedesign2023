//Written by Sammy
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerBullet : MonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    [Header("Externally Referenced Components")]
    GameObject _playerReticle;

    [Header("Bullet Attributes")]
    Vector2 _currentTrajectory;
    float _projectileSpeed = 0.25f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 3) Destroy(gameObject);
        if (collision.gameObject.layer == 8) { //Enemy layer
            collision.GetComponent<EnemyDummy_StartsMinigame>().StartMinigame();
            Destroy(gameObject);
        }
    }

    private void Awake() {
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