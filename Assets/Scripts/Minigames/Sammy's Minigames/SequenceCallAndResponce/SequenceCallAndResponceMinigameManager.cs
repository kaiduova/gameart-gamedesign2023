//Written by Sammy
using Input; using Minigame; using TMPro; using UnityEngine;

public class SequenceCallAndResponceMinigameManager : InputMonoBehaviour, IMinigameManager { 

    /*
    PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables 
    */

    public enum MinigameStates { Playing, Win, Lose, GameOver };

    [Header("Externally Referenced Components (Linked Enemy)")]
    public GameObject Enemy;

    [Header("Minigame Components")]
    public SpriteRenderer space1;
    Sprite space1Correct, space1Incorrect, space1Default;
    public SpriteRenderer space2;
    Sprite space2Correct, space2Incorrect, space2Default;
    public SpriteRenderer space3;
    Sprite space3Correct, space3Incorrect, space3Default;
    public SpriteRenderer space4;
    Sprite space4Correct, space4Incorrect, space4Default;
    public SpriteRenderer space5;
    Sprite space5Correct, space5Incorrect, space5Default;
    public Sprite[] spritePool = new Sprite[15];
    public GameObject SliderParent;
    public SpriteRenderer SliderFill;
    public TextMeshPro LivesTextPro, InfoTextPro;

    [Header("Minigame Attributes")]
    [SerializeField] private MinigameStates _minigameCurrentState;
    private int _lives, _difficultyReference, _inputUp, _inputLockUp, _inputDown, _inputLockDown, _inputLeft, _inputLockLeft, _inputRight, _inputLockRight;
    [SerializeField] private string _sequenceToInput, _inputtedSequence;
    private float _inputDuration, _inputDurationReset, flashTimer = 1.2f;

    public void StartMinigame(int difficulty) {
        _difficultyReference = difficulty;

        switch (difficulty) {
            case 1: {
                for (int i = 0; i < 3; i++) {
                    int sequenceNode = Random.Range(1, 6);
                    _sequenceToInput += sequenceNode;
                }
                space4.gameObject.SetActive(false);
                space5.gameObject.SetActive(false);
                space1.transform.position = new Vector2(-1.1f, space1.transform.position.y);
                space2.transform.position = new Vector2(0f, space2.transform.position.y);
                space3.transform.position = new Vector2(1.1f, space3.transform.position.y);
                space4.transform.position = new Vector2(1.5f, space4.transform.position.y);
                _inputDurationReset = 5f;
            } break;
            case 2: {
                for (int i = 0; i < 4; i++) {
                    int sequenceNode = Random.Range(1, 6);
                    _sequenceToInput += sequenceNode;
                }
                space5.gameObject.SetActive(false);
                space1.transform.position = new Vector2(-1.5f, space1.transform.position.y);
                space2.transform.position = new Vector2(-0.5f, space2.transform.position.y);
                space3.transform.position = new Vector2(0.5f, space3.transform.position.y);
                space4.transform.position = new Vector2(1.5f, space4.transform.position.y);
                _inputDurationReset = 7.5f;
            } break;
            case 3: {
                for (int i = 0; i < 5; i++) {
                    int sequenceNode = Random.Range(1, 6);
                    _sequenceToInput += sequenceNode;
                }
                _inputDurationReset = 10f;
            } break;
        }

        switch (difficulty) {
            case 1: {
                switch (_sequenceToInput[0]) {
                    case '1': {
                        space1Default = spritePool[0];
                        space1Correct = spritePool[1];
                        space1Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space1Default = spritePool[3];
                        space1Correct = spritePool[4];
                        space1Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space1Default = spritePool[6];
                        space1Correct = spritePool[7];
                        space1Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space1Default = spritePool[9];
                        space1Correct = spritePool[10];
                        space1Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space1Default = spritePool[12];
                        space1Correct = spritePool[13];
                        space1Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[1]) {
                    case '1': {
                        space2Default = spritePool[0];
                        space2Correct = spritePool[1];
                        space2Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space2Default = spritePool[3];
                        space2Correct = spritePool[4];
                        space2Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space2Default = spritePool[6];
                        space2Correct = spritePool[7];
                        space2Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space2Default = spritePool[9];
                        space2Correct = spritePool[10];
                        space2Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space2Default = spritePool[12];
                        space2Correct = spritePool[13];
                        space2Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[2]) {
                    case '1': {
                        space3Default = spritePool[0];
                        space3Correct = spritePool[1];
                        space3Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space3Default = spritePool[3];
                        space3Correct = spritePool[4];
                        space3Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space3Default = spritePool[6];
                        space3Correct = spritePool[7];
                        space3Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space3Default = spritePool[9];
                        space3Correct = spritePool[10];
                        space3Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space3Default = spritePool[12];
                        space3Correct = spritePool[13];
                        space3Incorrect = spritePool[14];
                    } break;
                }
            } break;
            case 2: {
                switch (_sequenceToInput[0]) {
                    case '1': {
                        space1Default = spritePool[0];
                        space1Correct = spritePool[1];
                        space1Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space1Default = spritePool[3];
                        space1Correct = spritePool[4];
                        space1Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space1Default = spritePool[6];
                        space1Correct = spritePool[7];
                        space1Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space1Default = spritePool[9];
                        space1Correct = spritePool[10];
                        space1Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space1Default = spritePool[12];
                        space1Correct = spritePool[13];
                        space1Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[1]) {
                    case '1': {
                        space2Default = spritePool[0];
                        space2Correct = spritePool[1];
                        space2Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space2Default = spritePool[3];
                        space2Correct = spritePool[4];
                        space2Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space2Default = spritePool[6];
                        space2Correct = spritePool[7];
                        space2Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space2Default = spritePool[9];
                        space2Correct = spritePool[10];
                        space2Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space2Default = spritePool[12];
                        space2Correct = spritePool[13];
                        space2Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[2]) {
                    case '1': {
                        space3Default = spritePool[0];
                        space3Correct = spritePool[1];
                        space3Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space3Default = spritePool[3];
                        space3Correct = spritePool[4];
                        space3Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space3Default = spritePool[6];
                        space3Correct = spritePool[7];
                        space3Incorrect = spritePool[8];
                    } break;
                    case '4': {
                            space3Default = spritePool[9];
                            space3Correct = spritePool[10];
                            space3Incorrect = spritePool[11];
                        } break;
                    case '5': {
                        space3Default = spritePool[12];
                        space3Correct = spritePool[13];
                        space3Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[3]) {
                    case '1': {
                        space4Default = spritePool[0];
                        space4Correct = spritePool[1];
                        space4Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space4Default = spritePool[3];
                        space4Correct = spritePool[4];
                        space4Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space4Default = spritePool[6];
                        space4Correct = spritePool[7];
                        space4Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space4Default = spritePool[9];
                        space4Correct = spritePool[10];
                        space4Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space4Default = spritePool[12];
                        space4Correct = spritePool[13];
                        space4Incorrect = spritePool[14];
                    } break;
                }
            } break;
            case 3: {
                switch (_sequenceToInput[0]) {
                    case '1': {
                        space1Default = spritePool[0];
                        space1Correct = spritePool[1];
                        space1Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space1Default = spritePool[3];
                        space1Correct = spritePool[4];
                        space1Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space1Default = spritePool[6];
                        space1Correct = spritePool[7];
                        space1Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space1Default = spritePool[9];
                        space1Correct = spritePool[10];
                        space1Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space1Default = spritePool[12];
                        space1Correct = spritePool[13];
                        space1Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[1]) {
                    case '1': {
                        space2Default = spritePool[0];
                        space2Correct = spritePool[1];
                        space2Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space2Default = spritePool[3];
                        space2Correct = spritePool[4];
                        space2Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space2Default = spritePool[6];
                        space2Correct = spritePool[7];
                        space2Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space2Default = spritePool[9];
                        space2Correct = spritePool[10];
                        space2Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space2Default = spritePool[12];
                        space2Correct = spritePool[13];
                        space2Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[2]) {
                    case '1': {
                        space3Default = spritePool[0];
                        space3Correct = spritePool[1];
                        space3Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space3Default = spritePool[3];
                        space3Correct = spritePool[4];
                        space3Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space3Default = spritePool[6];
                        space3Correct = spritePool[7];
                        space3Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space3Default = spritePool[9];
                        space3Correct = spritePool[10];
                        space3Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space3Default = spritePool[12];
                        space3Correct = spritePool[13];
                        space3Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[3]) {
                    case '1': {
                        space4Default = spritePool[0];
                        space4Correct = spritePool[1];
                        space4Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space4Default = spritePool[3];
                        space4Correct = spritePool[4];
                        space4Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space4Default = spritePool[6];
                        space4Correct = spritePool[7];
                        space4Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space4Default = spritePool[9];
                        space4Correct = spritePool[10];
                        space4Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space4Default = spritePool[12];
                        space4Correct = spritePool[13];
                        space4Incorrect = spritePool[14];
                    } break;
                }
                switch (_sequenceToInput[4]) {
                    case '1': {
                        space5Default = spritePool[0];
                        space5Correct = spritePool[1];
                        space5Incorrect = spritePool[2];
                    } break;
                    case '2': {
                        space5Default = spritePool[3];
                        space5Correct = spritePool[4];
                        space5Incorrect = spritePool[5];
                    } break;
                    case '3': {
                        space5Default = spritePool[6];
                        space5Correct = spritePool[7];
                        space5Incorrect = spritePool[8];
                    } break;
                    case '4': {
                        space5Default = spritePool[9];
                        space5Correct = spritePool[10];
                        space5Incorrect = spritePool[11];
                    } break;
                    case '5': {
                        space5Default = spritePool[12];
                        space5Correct = spritePool[13];
                        space5Incorrect = spritePool[14];
                    } break;
                }
            } break;
        }

        space1.sprite = space1Default;
        space2.sprite = space2Default;
        space3.sprite = space3Default;
        if (difficulty == 2 || difficulty == 3) space4.sprite = space4Default;
        if (difficulty == 3) space5.sprite = space5Default;

        _lives = 3;
        _inputDuration = _inputDurationReset;
        _minigameCurrentState = MinigameStates.Playing;
    }

    private void SequenceInput() {
        if (CurrentInput.RightStick.y > 0.9f) _inputUp = 1;
        else _inputUp = 0;
        if (CurrentInput.RightStick.y < -0.9f) _inputDown = 1;
        else _inputDown = 0;
        if (CurrentInput.RightStick.x < -0.9f) _inputLeft = 1;
        else _inputLeft = 0;
        if (CurrentInput.RightStick.x > 0.9f) _inputRight = 1;
        else _inputRight = 0;

        if (_inputUp == 1) {
            _inputLockUp++;
            if (_inputLockUp < 2) _inputtedSequence += 1;
            if (_inputLockUp > 2) _inputLockUp = 2;
        } else if (_inputUp == 0) _inputLockUp = 0;
        if (_inputDown == 1) {
            _inputLockDown++;
            if (_inputLockDown < 2) _inputtedSequence += 2;
            if (_inputLockDown > 2) _inputLockDown = 2;
        }  else if (_inputDown == 0) _inputLockDown = 0;
        if (_inputLeft == 1) {
            _inputLockLeft++;
            if (_inputLockLeft < 2) _inputtedSequence += 3;
            if (_inputLockLeft > 2) _inputLockLeft = 2;
        } else if (_inputLeft == 0) _inputLockLeft = 0;
        if (_inputRight == 1) {
            _inputLockRight++;
            if (_inputLockRight < 2) _inputtedSequence += 4;
            if (_inputLockRight > 2) _inputLockRight = 2;
        } else if (_inputRight == 0) _inputLockRight = 0;
        if (CurrentInput.GetKeyDownRightStickPress) _inputtedSequence += 5;
    }

    private void CheckSequence() {
        switch (_difficultyReference) {
            case 1: {
                if (_inputtedSequence.Length == 1) {
                    if (_inputtedSequence[0] == _sequenceToInput[0]) space1.sprite = space1Correct;
                    else if (_inputtedSequence[0] != _sequenceToInput[0]) {
                        space1.sprite = space1Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 2) {
                    if (_inputtedSequence[1] == _sequenceToInput[1]) space2.sprite = space2Correct;
                    else if (_inputtedSequence[1] != _sequenceToInput[1]) {
                        space2.sprite = space2Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 3) {
                    if (_inputtedSequence[2] == _sequenceToInput[2]) space3.sprite = space3Correct;
                    else if (_inputtedSequence[2] != _sequenceToInput[2]) {
                        space3.sprite = space3Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
            } break;
            case 2: {
                if (_inputtedSequence.Length == 1) {
                    if (_inputtedSequence[0] == _sequenceToInput[0]) space1.sprite = space1Correct;
                    else if (_inputtedSequence[0] != _sequenceToInput[0]) {
                        space1.sprite = space1Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 2) {
                    if (_inputtedSequence[1] == _sequenceToInput[1]) space2.sprite = space2Correct;
                    else if (_inputtedSequence[1] != _sequenceToInput[1]) {
                        space2.sprite = space2Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 3) {
                    if (_inputtedSequence[2] == _sequenceToInput[2]) space3.sprite = space3Correct;
                    else if (_inputtedSequence[2] != _sequenceToInput[2]) {
                        space3.sprite = space3Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 4) {
                    if (_inputtedSequence[3] == _sequenceToInput[3]) space4.sprite = space4Correct;
                    else if (_inputtedSequence[3] != _sequenceToInput[3]) {
                        space4.sprite = space4Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
            } break;
            case 3: {
                if (_inputtedSequence.Length == 1) {
                    if (_inputtedSequence[0] == _sequenceToInput[0]) space1.sprite = space1Correct;
                    else if (_inputtedSequence[0] != _sequenceToInput[0]) {
                        space1.sprite = space1Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 2) {
                    if (_inputtedSequence[1] == _sequenceToInput[1]) space2.sprite = space2Correct;
                    else if (_inputtedSequence[1] != _sequenceToInput[1]) {
                        space2.sprite = space2Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 3) { 
                    if (_inputtedSequence[2] == _sequenceToInput[2]) space3.sprite = space3Correct;
                    else if (_inputtedSequence[2] != _sequenceToInput[2]) {
                        space3.sprite = space3Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 4) {
                    if (_inputtedSequence[3] == _sequenceToInput[3]) space4.sprite = space4Correct;
                    else if (_inputtedSequence[3] != _sequenceToInput[3]) {
                        space4.sprite = space4Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
                if (_inputtedSequence.Length == 5) {
                    if (_inputtedSequence[4] == _sequenceToInput[4]) space5.sprite = space5Correct;
                    else if (_inputtedSequence[4] != _sequenceToInput[4]) {
                        space5.sprite = space5Incorrect;
                        _minigameCurrentState = MinigameStates.Lose;
                    }
                }
            } break;
        }
        if (_inputtedSequence == _sequenceToInput) _minigameCurrentState = MinigameStates.Win;
    }

    private void ScaleTimerBar() {
        if (_inputDuration > 0) SliderParent.transform.localScale -= new Vector3(((1 / _inputDurationReset) / 60), 0);
        if (SliderParent.transform.localScale.x > 0.5f) SliderFill.color = Color.green;
        if (SliderParent.transform.localScale.x < 0.5f && SliderParent.transform.localScale.x > 0.25f) SliderFill.color = Color.yellow;
        if (SliderParent.transform.localScale.x < 0.25f) SliderFill.color = Color.red;
    }

    private void AnimationNull() {
        space1.sprite = space1Default;
        space2.sprite = space2Default;
        space3.sprite = space3Default;
        if (_difficultyReference == 2 || _difficultyReference == 3) space4.sprite = space4Default;
        if (_difficultyReference == 3) space5.sprite = space5Default;
    }
    private void AnimationCorrect() {
        space1.sprite = space1Correct;
        space2.sprite = space2Correct;
        space3.sprite = space3Correct;
        if (_difficultyReference == 2 || _difficultyReference == 3) space4.sprite = space4Correct;
        if (_difficultyReference == 3) space5.sprite = space5Correct;
    }
    private void AnimationIncorrect() {
        space1.sprite = space1Incorrect;
        space2.sprite = space2Incorrect;
        space3.sprite = space3Incorrect;
        if (_difficultyReference == 2 || _difficultyReference == 3) space4.sprite = space4Incorrect;
        if (_difficultyReference == 3) space5.sprite = space5Incorrect;
    }

    private void WinAnimation() {
        InfoTextPro.text = "CORRECT";
        flashTimer -= Time.deltaTime;
        if (flashTimer < 0) {
            flashTimer = 0;
            _minigameCurrentState = MinigameStates.GameOver;
        }
        if (flashTimer < 1.2f && flashTimer > 1f) AnimationNull();
        if (flashTimer < 1f && flashTimer > 0.8f) AnimationCorrect();
        if (flashTimer < 0.8f && flashTimer > 0.6f) AnimationNull();
        if (flashTimer < 0.6f && flashTimer > 0.4f) AnimationCorrect();
        if (flashTimer < 0.4f && flashTimer > 0.2f) AnimationNull();
        if (flashTimer < 0.2f) AnimationCorrect();
    }

    private void LoseAnimation() {
        InfoTextPro.text = "INCORRECT";
        flashTimer -= Time.deltaTime;
        if (flashTimer < 0) {
            _lives--;
            if (_lives > 0) {
                flashTimer = 1.2f;
                _inputtedSequence = "";
                _inputDuration = _inputDurationReset;
                SliderParent.transform.localScale = new Vector3(1, SliderParent.transform.localScale.y);
                space1.sprite = space1Default;
                space2.sprite = space2Default;
                space3.sprite = space3Default;
                if (_difficultyReference == 2 || _difficultyReference == 3) space4.sprite = space4Default;
                if (_difficultyReference == 3) space5.sprite = space5Default;
                _minigameCurrentState = MinigameStates.Playing;
            }
            else if (_lives == 0) _minigameCurrentState = MinigameStates.GameOver;
        }
        if (flashTimer < 1.2f && flashTimer > 1f) AnimationNull();
        if (flashTimer < 1f && flashTimer > 0.8f) AnimationIncorrect();
        if (flashTimer < 0.8f && flashTimer > 0.6f) AnimationNull();
        if (flashTimer < 0.6f && flashTimer > 0.4f) AnimationIncorrect();
        if (flashTimer < 0.4f && flashTimer > 0.2f) AnimationNull();
        if (flashTimer < 0.2f) AnimationIncorrect();
    }

    public void CommunicateWithEnemy() {
        Enemy.GetComponent<EnemyDummy_StartsMinigame>().CommunicateWithMinigame();
    }

    private void Update() {
        LivesTextPro.text = "LIVES: " + _lives;
        if (_minigameCurrentState == MinigameStates.Playing) {
            InfoTextPro.text = "INPUT THE SEQUENCE";
            _inputDuration -= Time.deltaTime;
            if (_inputDuration < 0) {
                _inputDuration = 0;
                SliderParent.transform.localScale = new Vector3(0, SliderParent.transform.localScale.y);
                _minigameCurrentState = MinigameStates.Lose;
            }
            ScaleTimerBar();

            if (!(CurrentInput.RightStick.x != 0 || CurrentInput.RightStick.y != 0 || CurrentInput.GetKeyDownRightStickPress)) return;
            SequenceInput();
            CheckSequence();
        }

        if (_minigameCurrentState == MinigameStates.Win) WinAnimation();
        if (_minigameCurrentState == MinigameStates.Lose) LoseAnimation();
        if (_minigameCurrentState == MinigameStates.GameOver) {
            CommunicateWithEnemy();
            MinigameModule.Instance.MinigameFinish(true);
        }
    }
}