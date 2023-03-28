//Written by Sammy
using UnityEngine; using Minigame; using Input;

public class TestDummyScript : InputMonoBehaviour {
    
    /* 
    PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables 
    */

    public enum SammyMinigames { TopDownProjectileDodge, SequenceCallAndResponce, ButtonMasher, InputTiming, CoilWinder}
    public SammyMinigames MinigameToPlay;
    public int MinigameDifficulty;

    public void StartMinigame() {
        MinigameModule.Instance.StartMinigame(Minigame.Minigame.SequenceCallAndResponce, MinigameDifficulty);
    }

    void Update() {
        if (CurrentInput.GetKeyDownA) StartMinigame();
    }
}
