//Written by Sammy
using static EnemyDummy_StartsMinigame;
using UnityEngine;
using Minigame; 
using Input;
using TMPro;

public class InputTimingMinigameManager : InputMonoBehaviour, IMinigameManager {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    public enum MinigameStates { Playing, GameOver };

    [Header("Externally Referenced Components (Linked Enemy)")]
    public GameObject Enemy;

    [Header("Minigame Components")]
    [SerializeField] GameObject _enemy;
    [SerializeField] SpriteRenderer _enemyHurtboxSR;
    [SerializeField] SpriteRenderer _playerHitboxSR;
    [SerializeField] TextMeshPro _livesAndAlignmentsTextPro, _infoTextPro;

    [Header("Minigame Attributes")]
    [SerializeField] MinigameStates _minigameCurrentState;
    [SerializeField] int _lives, _passesToGoal;
    [SerializeField] float _enemySpeed;
    [SerializeField] bool _movingLeft, _movingRight;

    public void StartMinigame(int difficulty) {
        //Setting these here for some reason isn't working so I need to fix that

        //_enemy = gameObject.transform.GetChild(1).GetComponent<GameObject>();
        //_enemyHurtboxSR = _enemy.GetComponent<SpriteRenderer>();
        //_playerHitboxSR = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //_livesText = gameObject.transform.GetChild(2).GetComponent<TextMesh>();
        //_infoText = gameObject.transform.GetChild(3).GetComponent<TextMesh>();

        switch (difficulty) {
            case 1: {
                _passesToGoal = 3;
                _enemySpeed = 3f;
            } break;
            case 2: {
                _passesToGoal = 5;
                _enemySpeed = 4f;
            } break;
            case 3: {
                _passesToGoal = 7;
                _enemySpeed = 6f;
            } break;
        }

        int randomiseDirection = Random.Range(0, 2);
        switch(randomiseDirection){
            case 0: {
                _movingRight = true;
                _movingLeft = false;
                _enemy.transform.position = new Vector3(2.75f, _enemy.transform.position.y);
            } break;
            case 1: {
                _movingRight = false;
                _movingLeft = true;
                _enemy.transform.position = new Vector3(-2.75f, _enemy.transform.position.y);
            } break;
        }

        _lives = 3;
        _minigameCurrentState = MinigameStates.Playing;
    }

    public void CommunicateWithEnemy() {
        Enemy.GetComponent<EnemyDummy_StartsMinigame>().CommunicateWithMinigame();
    }
  
    private void Update() {
        if (_minigameCurrentState == MinigameStates.Playing) {
            _livesAndAlignmentsTextPro.text = "LIVES: " + _lives + "    ALIGNMENTS LEFT: " + _passesToGoal;
            _infoTextPro.text = "PRESS R3 WHEN ALIGNED";

            //Player does not align the square
            if (_movingRight) {
                _enemy.transform.position -= new Vector3(_enemySpeed, 0) * Time.deltaTime;
                if (_enemy.transform.position.x <= -2.75f) {
                    _movingLeft = true;
                    _movingRight = false;
                }
            } else if (_movingLeft) {
                _enemy.transform.position += new Vector3(_enemySpeed, 0) * Time.deltaTime;
                if (_enemy.transform.position.x >= 2.75f) {
                    _movingRight = true;
                    _movingLeft = false;
                }
            }

            if (CurrentInput.GetKeyDownRightStickPress) { //Player aligns the square
                if (_playerHitboxSR.bounds.Intersects(_enemyHurtboxSR.bounds)) {
                    _passesToGoal--;
                } else _lives--;
            }
        }

        if (_passesToGoal <= 0 && _lives != 0) { //Win
            _infoTextPro.text = "HACK COMPLETE";
            _minigameCurrentState = MinigameStates.GameOver;
            _enemy.SetActive(false);
            CommunicateWithEnemy();
            MinigameModule.Instance.MinigameFinish(true);
        }

        if (_passesToGoal > 0 && _lives <= 0) { //Loss
            _infoTextPro.text = "HACK FAILED";
            _minigameCurrentState = MinigameStates.GameOver;
            _enemy.SetActive(false);
            MinigameModule.Instance.MinigameFinish(false); 
        }
    }
}