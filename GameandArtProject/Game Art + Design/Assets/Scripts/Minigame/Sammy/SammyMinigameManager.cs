//Written by Sammy
using Input; using Minigame; using UnityEngine;

public class SammyMinigameManager : InputMonoBehaviour, IMinigameManager {
    //PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    //camelCase - parameters, arguments, methodVariables, functionVariables
    //_camelCase - privateMemberVariables

    [Header("Minigame Components")]
    [SerializeField] GameObject _enemy;
    [SerializeField] SpriteRenderer _enemyHurtboxSR;
    [SerializeField] SpriteRenderer _playerHitboxSR;
    [SerializeField] TextMesh _livesAndAlignmentsText, _infoText;

    [Header("Minigame Attributes")]
    [SerializeField] string _minigameState;
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
                _enemySpeed = 5f;
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
        _minigameState = "Playing";
    }
  
    void Update() {
        if (_minigameState == "Playing") {
            _livesAndAlignmentsText.text = "LIVES: " + _lives + "    ALIGNMENTS LEFT: " + _passesToGoal;
            _infoText.text = "PRESS R3 WHEN ALIGNED";

            //Player does not align the square
            if (_movingRight) {
                _enemy.transform.position -= new Vector3(_enemySpeed, 0) * Time.deltaTime;
                if (_enemy.transform.position.x <= -2.75f) {
                    _enemy.transform.position = new Vector3(2.75f, _enemy.transform.position.y);
                    _lives--;
                }
            } else if (_movingLeft) {
                _enemy.transform.position += new Vector3(_enemySpeed, 0) * Time.deltaTime;
                if (_enemy.transform.position.x >= 2.75f) {
                    _enemy.transform.position = new Vector3(-2.75f, _enemy.transform.position.y);
                    _lives--;
                }
            }

            if (CurrentInput.GetKeyDownRightStickPress) { //Player aligns the square
                if (_playerHitboxSR.bounds.Intersects(_enemyHurtboxSR.bounds)) {
                    _passesToGoal--;
                    if (_movingRight) _enemy.transform.position = new Vector3(2.75f, _enemy.transform.position.y);
                    else if (_movingLeft) _enemy.transform.position = new Vector3(-2.75f, _enemy.transform.position.y);
                }
            }
        }

        if (_passesToGoal <= 0 && _lives != 0) { //Win
        _infoText.text = "HACK COMPLETE";
        _minigameState = "GameOver";
        _enemy.SetActive(false);
        MinigameModule.Instance.MinigameFinish(true); 
        }

        if (_passesToGoal > 0 && _lives <= 0) { //Loss
            _infoText.text = "HACK FAILED";
            _minigameState = "GameOver";
            _enemy.SetActive(false);
            MinigameModule.Instance.MinigameFinish(false); 
        }
    }
}