using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunyiMinigameTimeDown : MonoBehaviour
{
    //PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    //camelCase - parameters, arguments, methodVariables, functionVariables
    //_camelCase - privateMemberVariables

    [Header("Minigame Components:")]
    [SerializeField] SkenatMinigameManager skenatMinigameManager;
    [SerializeField] TextMesh timerText;

    [Header("Variables:")]
    [SerializeField] float initialLength, maximumLength;
    [SerializeField] float maximumTimeLeft, initialTimeLeft;
    [SerializeField] public float currentTimeLeft;
    [SerializeField] Vector3 initialLocalScale;



    void Start()
    {
        initialTimeLeft = 5;
        maximumTimeLeft = initialTimeLeft * 3;
        initialLength = this.transform.localScale.x;
        maximumLength = initialLength * 3;
        currentTimeLeft = initialTimeLeft;
        initialLocalScale = this.transform.localScale;
    }

    void Update()
    {
        if (skenatMinigameManager.lives > 0 & !skenatMinigameManager.isWin)
        {
            currentTimeLeft -= Time.deltaTime;
            this.transform.localScale = new Vector3(Mathf.Lerp(0, maximumLength, (currentTimeLeft / maximumTimeLeft)), initialLocalScale.y);
            if (currentTimeLeft > 15)
            {
                currentTimeLeft = 15;
            }
            timerText.text = "Time Left To Complete Hacking: " + currentTimeLeft.ToString("0.00");

        }
    }
}
