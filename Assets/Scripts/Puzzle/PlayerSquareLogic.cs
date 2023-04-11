using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

public class PlayerSquareLogic : InputMonoBehaviour
{
    //PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    //camelCase - parameters, arguments, methodVariables, functionVariables
    //_camelCase - privateMemberVariables
    [Header("Minigame Components:")]
    [SerializeField] PlayerBlankSquareLogic _refToPlayerBlankSquareLogic;
    [SerializeField] SelectionFrameLogic _refToSelectionFrame;
    [Header("Minigame Variables:")]
    public bool IsMyColorBlack;
    [SerializeField] bool _isMoving;
    [SerializeField] bool _movingUp, _movingDown, _movingLeft, _movingRight;
    [SerializeField] int _positionNumber;
    Vector2 _positionCache;
    int _numberCache;

    void Start()
    {

        _positionNumber = int.Parse(this.name.Remove(0, 12));
        if (_positionNumber == 1)
        {
            _refToSelectionFrame.transform.position = this.transform.position;
        }
    }


    void Update()
    {
        if (_refToSelectionFrame.PositionNumber == _positionNumber)
        {
            _refToSelectionFrame.transform.position = this.transform.position;
            if (CurrentInput.GetKeyUpRightStickPress)
            {

                if (_refToPlayerBlankSquareLogic.PositionNumber + 4 == _positionNumber ||
                    _refToPlayerBlankSquareLogic.PositionNumber - 4 == _positionNumber ||
                    _refToPlayerBlankSquareLogic.PositionNumber - 1 == _positionNumber ||
                    _refToPlayerBlankSquareLogic.PositionNumber + 1 == _positionNumber)
                {
                    if (!_refToPlayerBlankSquareLogic.IsMoving)
                    {
                        _isMoving = true;
                        _positionCache = this.transform.position;
                        _numberCache = _positionNumber;

                    }
                }

            }
            if (_isMoving)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, _refToPlayerBlankSquareLogic.transform.position, 0.15f);
                if (this.transform.position == _refToPlayerBlankSquareLogic.transform.position)
                {
                    _isMoving = false;
                    _refToPlayerBlankSquareLogic.IsMoving = false;
                    _positionNumber = _refToPlayerBlankSquareLogic.PositionNumber;
                    _refToSelectionFrame.PositionNumber = _positionNumber;
                    _refToPlayerBlankSquareLogic.PositionNumber = _numberCache;
                    _refToPlayerBlankSquareLogic.transform.position = _positionCache;

                }

            }
        }




    }
}
