using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

public class SelectionFrameLogic : InputMonoBehaviour
{
    [Header("Minigame Variables:")]
    [SerializeField] public int PositionNumber;
    [SerializeField] int _positionCache;
    [SerializeField] PlayerBlankSquareLogic refToPlayerBlankSquareLogic;

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
        if (PositionNumber == refToPlayerBlankSquareLogic.PositionNumber)
        {
            PositionNumber = _positionCache;
        }
    }
}
