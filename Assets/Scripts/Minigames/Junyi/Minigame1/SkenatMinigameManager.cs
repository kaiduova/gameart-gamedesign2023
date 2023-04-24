using Input;
using Minigame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkenatMinigameManager : InputMonoBehaviour, IMinigameManager
{
    //PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    //camelCase - parameters, arguments, methodVariables, functionVariables
    //_camelCase - privateMemberVariables

    [Header("Minigame Components:")]
    [SerializeField] Transform refToCharger;
    [SerializeField] Transform refToMarker;
    [SerializeField] GameObject refToPrefabChargerVE;
    [SerializeField] TextMesh refToText;
    [SerializeField] JunyiMinigameTimeDown timerScript;


    [Header("Minigame Variables:")]
    float chargingSpeed=0.02f;
    float markerPosition;
    float markerLength, chargerMaximumLength;
    public int lives = 3;
    int hitCount;
    bool isCombo, isHit;
    int comboCount;
    int targetHitCount;
    public bool isWin;
    Color initialColor;

    public void StartMinigame(int difficulty)
    {
        initialColor = refToCharger.GetComponent<SpriteRenderer>().color;
        chargerMaximumLength = 6.5f;
        chargingSpeed = 0.025f;

        switch (difficulty)
        {
            case 1:
                markerLength = 1.6f;
                targetHitCount = 10;
                break;
            case 2:
                markerLength = 1.3f;
                targetHitCount = 10;

                break;
            case 3:
                markerLength = 1f;
                targetHitCount = 10;

                break;
        }

        markerPosition = Random.Range(refToCharger.position.y, refToCharger.position.y + chargerMaximumLength - markerLength * 1.2f);
        refToMarker.localScale = new Vector3(refToMarker.localScale.x, markerLength);
        refToMarker.position = new Vector3(refToMarker.position.x, markerPosition);




    }

    void Start()
    {
        StartMinigame(3);
    }

    void Update()
    {
        if (!isWin)
        {
            if (targetHitCount <= hitCount)
            {
                isWin = true;
            }
            if (lives > 0)
            {
                refToCharger.localScale += new Vector3(0, chargingSpeed, 0);
                if (refToCharger.localScale.y > chargerMaximumLength || refToCharger.localScale.y < 0.05f)
                {
                    chargingSpeed = -chargingSpeed;
                    if (!isHit)
                    {
                        isCombo = false;
                        comboCount = 0;
                    }
                    isHit = false;

                }

                if (CurrentInput.GetKeyDownRightStickPress)
                {
                    if (refToCharger.localScale.y + refToCharger.position.y > refToMarker.position.y && refToCharger.localScale.y + refToCharger.position.y < refToMarker.position.y + markerLength)
                    {
                        isCombo = true;
                        isHit = true;
                        print("Up:Check!");
                        hitCount++;
                        comboCount++;
                        timerScript.currentTimeLeft += 5f;

                        GameObject newVE = Instantiate(refToPrefabChargerVE, refToMarker.transform.position, Quaternion.identity);
                        newVE.transform.localScale = refToMarker.transform.localScale;
                        markerPosition = Random.Range(refToCharger.position.y, refToCharger.position.y + chargerMaximumLength - markerLength * 1.2f);
                        refToMarker.position = new Vector3(refToMarker.position.x, markerPosition);
                    }
                    else
                    {
                        lives--;

                        isCombo = false;
                        comboCount = 0;
                    }

                }

                if (lives < 1)
                {
                    print("lose!");
                }
                if (comboCount >= 2)
                {
                    refToCharger.GetComponent<SpriteRenderer>().color = Color.yellow;

                }
                else
                {
                    refToCharger.GetComponent<SpriteRenderer>().color = initialColor;
                }
                refToText.text = "Combo: " + comboCount + "\nHit: " + hitCount + "\nLives: " + lives;

            }
            else
            {
                refToText.text = "Hacking Failed!";

            }

        }
        else
        {
            refToText.text = "Hack Complete";
        }
    }
}
