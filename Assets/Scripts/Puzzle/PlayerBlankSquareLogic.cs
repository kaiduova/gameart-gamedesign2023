using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlankSquareLogic : MonoBehaviour
{
    //PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    //camelCase - parameters, arguments, methodVariables, functionVariables
    //_camelCase - privateMemberVariables

    [Header("Minigame Components:")]

    [Header("Minigame Variables:")]
    public int PositionNumber;
    public bool IsMoving;
    void Start()
    {
        PositionNumber = int.Parse(this.name.Remove(0, 12));

    }

    void Update()
    {
        
    }
}
