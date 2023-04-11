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
    [SerializeField] PlayerBlankSquareLogic refToPlayerBlankSquareLogic;
    [SerializeField] SelectionFrameLogic refToSelectionFrame;
    [Header("Minigame Variables:")]
    public bool IsMyColorBlack;
    [SerializeField] bool _isMoving;
    [SerializeField] bool _movingUp, _movingDown, _movingLeft, _movingRight;
    [SerializeField] int positionNumber;
    Vector2 positionCache;
    int numberCache;

    void Start()
    {

        positionNumber = int.Parse(this.name.Remove(0, 12));
        if (positionNumber == 1)
        {
            refToSelectionFrame.transform.position = this.transform.position;
        }
    }

    void Update()
    {
        if (refToSelectionFrame.PositionNumber == positionNumber)
        {
            refToSelectionFrame.transform.position = this.transform.position;
            if (CurrentInput.GetKeyUpRightStickPress)
            {

                if (refToPlayerBlankSquareLogic.PositionNumber + 4 == positionNumber ||
                    refToPlayerBlankSquareLogic.PositionNumber - 4 == positionNumber ||
                    refToPlayerBlankSquareLogic.PositionNumber - 1 == positionNumber ||
                    refToPlayerBlankSquareLogic.PositionNumber + 1 == positionNumber)
                {
                    if (!refToPlayerBlankSquareLogic.IsMoving)
                    {
                        _isMoving = true;
                        positionCache = this.transform.position;
                        numberCache = positionNumber;

                    }
                }

            }
            if (_isMoving)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, refToPlayerBlankSquareLogic.transform.position, 0.15f);
                if (this.transform.position == refToPlayerBlankSquareLogic.transform.position)
                {
                    _isMoving = false;
                    refToPlayerBlankSquareLogic.IsMoving = false;
                    positionNumber = refToPlayerBlankSquareLogic.PositionNumber;
                    refToSelectionFrame.PositionNumber = positionNumber;
                    refToPlayerBlankSquareLogic.PositionNumber = numberCache;
                    refToPlayerBlankSquareLogic.transform.position = positionCache;

                }

            }
        }




    }
}
