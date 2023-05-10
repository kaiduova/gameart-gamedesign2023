using UnityEngine.SceneManagement;
using UnityEngine;
using Input;

public class LevelSelectManager : InputMonoBehaviour {

    public enum LevelSelectStates { LevelSelectScreen, CreditsScreen }

    [Header("Current State")]
    public LevelSelectStates CurrentState;

    [Header("Cursor & Cursor Screen Bounds Attributes")]
    [SerializeField] private GameObject _cursor;
    [SerializeField] private SpriteRenderer _cursorSR;
    [Space(10)]
    [SerializeField] private float _cursorMovementSpeed;
    private float _cursorHorizontalInput;
    private float _cursorVerticalInput;
    private float _cursorWidth;
    private float _cursorHeight;
    private Vector2 _screenBoundaries;
    private Vector2 _screenBoundariesOther;

    [Header("Level Select Attributes")]
    [SerializeField] private GameObject _levelSelectHolder;
    [Space(10)]
    [SerializeField] private MeshRenderer _playMR;
    [SerializeField] private MeshRenderer _level1MR;
    [SerializeField] private MeshRenderer _level2MR;
    [SerializeField] private MeshRenderer _level3MR;
    [SerializeField] private MeshRenderer _creditsMR;
    [SerializeField] private MeshRenderer _controlsMR;
    [SerializeField] private MeshRenderer _quitMR;

    [Header("Credits Select Attributes")]
    [SerializeField] private GameObject _creditsHolder;
    [SerializeField] private MeshRenderer _backMR;

    private void Awake() {
        _cursorSR = _cursor.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _cursorWidth = _cursorSR.bounds.extents.x;
        _cursorHeight = _cursorSR.bounds.extents.y;
        CurrentState = LevelSelectStates.LevelSelectScreen;
    }

    private void CursorMovement() {
        _cursorHorizontalInput = CurrentInput.LeftStick.x;
        _cursorVerticalInput = CurrentInput.LeftStick.y;

        if (_cursorHorizontalInput > 0.5f) _cursor.transform.position += new Vector3(_cursorMovementSpeed, 0, 0) * Time.deltaTime;
        if (_cursorHorizontalInput < -0.5f) _cursor.transform.position -= new Vector3(_cursorMovementSpeed, 0, 0) * Time.deltaTime;
        if (_cursorVerticalInput > 0.5f) _cursor.transform.position += new Vector3(0, _cursorMovementSpeed, 0) * Time.deltaTime;
        if (_cursorVerticalInput < -0.5f)  _cursor.transform.position -= new Vector3(0, _cursorMovementSpeed, 0) * Time.deltaTime;
    }

    private void CursorScreenBoundaries() {
        _screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        _screenBoundariesOther = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 viewPos = _cursor.transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, _screenBoundariesOther.x + _cursorWidth, _screenBoundaries.x - _cursorWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, _screenBoundariesOther.y + _cursorHeight, _screenBoundaries.y - _cursorHeight);
        _cursor.transform.position = viewPos;
    }

    private void Update() {
        CursorMovement();
        CursorScreenBoundaries();

        switch (CurrentState) {
            case LevelSelectStates.LevelSelectScreen: {
                _levelSelectHolder.SetActive(true);
                _creditsHolder.SetActive(false);

                if (_cursorSR.bounds.Intersects(_playMR.bounds)) {
                    _playMR.GetComponent<TextMesh>().color = Color.red;
                    //if (CurrentInput.GetKeyDownA) 
                } else _playMR.GetComponent<TextMesh>().color = Color.white;

                if (_cursorSR.bounds.Intersects(_level1MR.bounds)) {
                    _level1MR.GetComponent<TextMesh>().color = Color.red;
                    //if (CurrentInput.GetKeyDownA) 
                } else _level1MR.GetComponent<TextMesh>().color = Color.white;

                if (_cursorSR.bounds.Intersects(_level2MR.bounds)) {
                    _level2MR.GetComponent<TextMesh>().color = Color.red;
                    //if (CurrentInput.GetKeyDownA) 
                } else _level2MR.GetComponent<TextMesh>().color = Color.white;

                if (_cursorSR.bounds.Intersects(_level3MR.bounds)) {
                    _level3MR.GetComponent<TextMesh>().color = Color.red;
                    //if (CurrentInput.GetKeyDownA) 
                } else _level3MR.GetComponent<TextMesh>().color = Color.white;

                //Controls

                if (_cursorSR.bounds.Intersects(_creditsMR.bounds)) {
                    _creditsMR.GetComponent<TextMesh>().color = Color.red;
                    if (CurrentInput.GetKeyDownA) CurrentState = LevelSelectStates.CreditsScreen;
                } else _creditsMR.GetComponent<TextMesh>().color = Color.white;

                if (_cursorSR.bounds.Intersects(_quitMR.bounds)) {
                    _quitMR.GetComponent<TextMesh>().color = Color.red;
                    if (CurrentInput.GetKeyDownA) Application.Quit();
                } else _quitMR.GetComponent<TextMesh>().color = Color.white;
            } break;

            case LevelSelectStates.CreditsScreen: {
                _levelSelectHolder.SetActive(false);
                _creditsHolder.SetActive(true);

                if (_cursorSR.bounds.Intersects(_backMR.bounds)) {
                    _backMR.GetComponent<TextMesh>().color = Color.red;
                    if (CurrentInput.GetKeyDownA) CurrentState = LevelSelectStates.LevelSelectScreen;
                } else _backMR.GetComponent<TextMesh>().color = Color.white;
            } break;
        }
    }
}