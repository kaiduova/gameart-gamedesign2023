//Written by Sammy
using Input; using Minigame; using UnityEngine;

public class EnemyDummy_StartsMinigame : InputMonoBehaviour {
    //PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    //camelCase - parameters, arguments, methodVariables, functionVariables
    //_camelCase - privateMemberVariables

    public int EnemyID;

    private void Awake() {
        if (EnemyID == 0 || EnemyID > 3) EnemyID = 1; //Failsafe
    }

    void Update() {
        if (CurrentInput.GetKeyDownLB) MinigameModule.Instance.StartMinigame(Minigame.Minigame.Sammy, EnemyID);
    }
}