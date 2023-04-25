using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

public class SelectionFrameLogic : InputMonoBehaviour
{
    [Header("Minigame Components")]
    [SerializeField] Transform _myArrow;
    [Header("Minigame Variables:")]
    public int PositionNumber;
    public bool ShouldArrowOnAble;
    [SerializeField] int _positionCache;
    [SerializeField] PlayerBlankSquareLogic _refToPlayerBlankSquareLogic;


    bool _isMoving;
    void Start()
    {
        Application.targetFrameRate = 60;
        PositionNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        _positionCache = PositionNumber;
        if (CurrentInput.RightStick.x > 0.5f)
        {
            if (!_isMoving)
            {
                _isMoving = true;
                PositionNumber++;
                if (PositionNumber > 16)
                {
                    PositionNumber = 1;
                }

            }

        }
        else if (CurrentInput.RightStick.x < -0.5f)
        {
            if (!_isMoving)
            {
                _isMoving = true;

                PositionNumber--;

                if (PositionNumber < 1)
                {
                    PositionNumber = 16;
                }
            }
        }
        else if (CurrentInput.RightStick.y > 0.5f)
        {
            if (!_isMoving)
            {
                _isMoving = true;

                PositionNumber -= 4;
                if (PositionNumber < 1)
                {
                    PositionNumber += 16;
                }
            }
        }
        else if (CurrentInput.RightStick.y < -0.5f)
        {
            if (!_isMoving)
            {
                _isMoving = true;

                PositionNumber += 4;
                if (PositionNumber > 16)
                {
                    PositionNumber -= 16;
                }
            }
        }
        else
        {
            _isMoving = false;
        }
        if (PositionNumber == _refToPlayerBlankSquareLogic.PositionNumber)
        {
            PositionNumber = _positionCache;
        }

        ShouldArrowOnAble = true;

        if (ShouldArrowOnAble)
        {
            //if(_refToPlayerBlankSquareLogic.PositionNumber + 4 == PositionNumber ||
            //        _refToPlayerBlankSquareLogic.PositionNumber - 4 == _positionNumber ||
            //        _refToPlayerBlankSquareLogic.PositionNumber - 1 == _positionNumber ||
            //        _refToPlayerBlankSquareLogic.PositionNumber + 1 == _positionNumber)
            //{

        }
        if (_refToPlayerBlankSquareLogic.PositionNumber + 4 == PositionNumber)
        {
            _myArrow.localEulerAngles=new Vector3(0, 0, 0);


        }
        else if (_refToPlayerBlankSquareLogic.PositionNumber - 4 == PositionNumber)
        {
            _myArrow.localEulerAngles=new Vector3(0, 0, 180);
        }
        else if(_refToPlayerBlankSquareLogic.PositionNumber - 1 == PositionNumber)
        {
            _myArrow.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if (_refToPlayerBlankSquareLogic.PositionNumber + 1 == PositionNumber)
        {
            _myArrow.localEulerAngles = new Vector3(0, 0, 90);
        }

    }

}

