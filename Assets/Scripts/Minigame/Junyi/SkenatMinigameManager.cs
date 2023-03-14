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
    [SerializeField] Transform refToUpCharger;
    [SerializeField] Transform refToDownCharger, refToRightCharger, refToLeftCharger;
    [SerializeField] Transform refToUpMarker, refToDownMarker, refToLeftMarker, refToRightMarker;


    [Header("Minigame Variables:")]
    float chargingSpeed=0.02f;
    float upMarkerPosition, downMarkerPosition, leftMarkerPosition, rightMarkerPosition;
    float markerLength, chargerMaximumLength;
    float centerSize = 1f;
    bool upCheck, downCheck, leftCheck, rightCheck;
    int lives = 3;
    enum AllChargeDirections { chargingUp,chargingDown,chargingLeft,chargingRight,none}
    AllChargeDirections currentChargeDirection = AllChargeDirections.none;
    public void StartMinigame(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                markerLength = 0.8f;
                chargingSpeed = 0.02f;

                break;
            case 2:
                markerLength = 0.7f;
                chargingSpeed = 0.025f;

                break;
            case 3:
                markerLength = 0.6f;
                chargingSpeed = 0.03f;

                break;
        }
        chargerMaximumLength = 4f;
        upMarkerPosition = Random.Range(centerSize * 0.5f, chargerMaximumLength - markerLength - centerSize * 0.5f);
        downMarkerPosition = Random.Range(centerSize * 0.5f, chargerMaximumLength - markerLength - centerSize * 0.5f);
        rightMarkerPosition = Random.Range(centerSize * 0.5f, chargerMaximumLength - markerLength - centerSize * 0.5f);
        leftMarkerPosition = Random.Range(centerSize * 0.5f, chargerMaximumLength - markerLength - centerSize * 0.5f);

        refToUpMarker.localScale = new Vector3(refToUpMarker.localScale.x, markerLength);
        refToDownMarker.localScale = new Vector3(refToUpMarker.localScale.x, markerLength);
        refToLeftMarker.localScale = new Vector3(refToUpMarker.localScale.x, markerLength);
        refToRightMarker.localScale = new Vector3(refToUpMarker.localScale.x, markerLength);

        refToUpMarker.position = new Vector3(refToUpMarker.position.x, centerSize+upMarkerPosition);
        refToDownMarker.position = new Vector3(refToDownMarker.position.x, -centerSize - downMarkerPosition);

        refToLeftMarker.position = new Vector3(-centerSize - leftMarkerPosition, refToLeftMarker.position.y);
        refToRightMarker.position = new Vector3(centerSize + rightMarkerPosition, refToRightMarker.position.y);



    }

    void Start()
    {
        StartMinigame(3);
    }

    void Update()
    {

        if (CurrentInput.LeftStick.x < -0.5f && currentChargeDirection == AllChargeDirections.chargingLeft)
        {
            if (!leftCheck)
            {
                refToLeftCharger.localScale += new Vector3(0, chargingSpeed, 0);
                if (refToLeftCharger.localScale.y > 4 || refToLeftCharger.localScale.y < 0.05f)
                {
                    chargingSpeed = -chargingSpeed;
                }

            }
        }
        else if ( currentChargeDirection == AllChargeDirections.chargingLeft)
        {
            currentChargeDirection = AllChargeDirections.none;
            if (chargingSpeed < 0)
            {
                chargingSpeed = -chargingSpeed;
            }
            if (!leftCheck)
            {
                if (-refToLeftCharger.localScale.y < refToLeftMarker.position.x + centerSize * 0.5f && -refToLeftCharger.localScale.y > refToLeftMarker.position.x + centerSize * 0.5f - markerLength)
                {
                    refToLeftCharger.GetComponent<SpriteRenderer>().color = Color.yellow;
                    leftCheck = true;
                    print("Left:Check!");
                }
                else
                {
                    if (lives > 1)
                    {
                        refToLeftCharger.localScale = new Vector3(refToLeftCharger.localScale.x, 0.05f);
                        lives--;
                        print("Left:Try again!");

                    }
                    else
                    {
                        lives--;
                        print("You lose");
                    }

                }
            }

        }
        else if (CurrentInput.LeftStick.x < -0.5f && currentChargeDirection != AllChargeDirections.chargingLeft)
        {
            currentChargeDirection = AllChargeDirections.chargingLeft;

        }


        if (CurrentInput.LeftStick.x > 0.5f && currentChargeDirection == AllChargeDirections.chargingRight)
        {
            if (!rightCheck)
            {
                refToRightCharger.localScale += new Vector3(0, chargingSpeed, 0);
                if (refToRightCharger.localScale.y > 4 || refToRightCharger.localScale.y < 0.05f)
                {
                    chargingSpeed = -chargingSpeed;
                }
            }
        }
        else if (currentChargeDirection == AllChargeDirections.chargingRight)
        {
            currentChargeDirection = AllChargeDirections.none;
            if (chargingSpeed < 0)
            {
                chargingSpeed = -chargingSpeed;
            }
            if (!rightCheck)
            {
                if (refToRightCharger.localScale.y > refToRightMarker.position.x - centerSize * 0.5f && refToRightCharger.localScale.y < refToRightMarker.position.x - centerSize * 0.5f + markerLength)
                {
                    refToRightCharger.GetComponent<SpriteRenderer>().color = Color.yellow;
                    rightCheck = true;
                    print("Right:Check!");

                }
                else
                {

                    if (lives > 1)
                    {
                        refToRightCharger.localScale = new Vector3(refToRightCharger.localScale.x, 0.05f);
                        lives--;
                        print("Right:Try again!");
                    }
                    else
                    {
                        lives--;
                        print("You lose");
                    }
                }
            }

        }
        else if (CurrentInput.LeftStick.x > 0.5f && currentChargeDirection != AllChargeDirections.chargingRight)
        {
            currentChargeDirection = AllChargeDirections.chargingRight;

        }


        if (CurrentInput.LeftStick.y < -0.5f && currentChargeDirection == AllChargeDirections.chargingDown)
        {
            if (!downCheck)
            {
                refToDownCharger.localScale += new Vector3(0, chargingSpeed, 0);

            }
            if (refToDownCharger.localScale.y > 4 || refToDownCharger.localScale.y < 0.05f)
            {
                chargingSpeed = -chargingSpeed;
            }

        }
        else if (currentChargeDirection == AllChargeDirections.chargingDown)
        {
            currentChargeDirection = AllChargeDirections.none;
            if (chargingSpeed < 0)
            {
                chargingSpeed = -chargingSpeed;
            }
            if (!downCheck)
            {
                if (-refToDownCharger.localScale.y < refToDownMarker.position.y + centerSize * 0.5f && -refToDownCharger.localScale.y > refToDownMarker.position.y + centerSize * 0.5f - markerLength)
                {
                    refToDownCharger.GetComponent<SpriteRenderer>().color = Color.yellow;
                    downCheck = true;
                    print("Down:Check!");

                }
                else
                {
                    if (lives > 1)
                    {
                        refToDownCharger.localScale = new Vector3(refToLeftCharger.localScale.x, 0.05f);
                        lives--;
                        print("Down:Try again!");
                    }
                    else
                    {
                        lives--;
                        print("You lose");
                    }
                }
            }

        }
        else if (CurrentInput.LeftStick.y < -0.5f && currentChargeDirection != AllChargeDirections.chargingDown)
        {
            currentChargeDirection = AllChargeDirections.chargingDown;

        }


        if (CurrentInput.LeftStick.y > 0.5f && currentChargeDirection == AllChargeDirections.chargingUp)
        {
            if (!upCheck)
            {
                refToUpCharger.localScale += new Vector3(0, chargingSpeed, 0);
                if (refToUpCharger.localScale.y > 4 || refToUpCharger.localScale.y < 0.05f)
                {
                    chargingSpeed = -chargingSpeed;
                }
            }
        }
        else if (currentChargeDirection == AllChargeDirections.chargingUp)
        {
            currentChargeDirection = AllChargeDirections.none;
            if (chargingSpeed < 0)
            {
                chargingSpeed = -chargingSpeed;
            }
            if (!upCheck)
            {
                if (refToUpCharger.localScale.y > refToUpMarker.position.y - centerSize * 0.5f && refToUpCharger.localScale.y < refToUpMarker.position.y - centerSize * 0.5f + markerLength)
                {
                    refToUpCharger.GetComponent<SpriteRenderer>().color = Color.yellow;
                    upCheck = true;
                    print("Up:Check!");

                }
                else
                {
                    if (lives > 1)
                    {
                        refToUpCharger.localScale = new Vector3(refToRightCharger.localScale.x, 0.05f);
                        lives--;
                        print("Up:Try again!");
                    }
                    else
                    {
                        lives--;
                        print("You lose");
                    }
                }
            }

        }
        else if (CurrentInput.LeftStick.y > 0.5f && currentChargeDirection != AllChargeDirections.chargingUp)
        {
            currentChargeDirection = AllChargeDirections.chargingUp;

        }



    }
}
