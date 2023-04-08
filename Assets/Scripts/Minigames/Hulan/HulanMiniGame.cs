using Input;
using Minigame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulanMiniGame : InputMonoBehaviour, IMinigameManager 
{

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
camelCase - parameters, arguments, methodVariables, functionVariables
_camelCase - privateMemberVariables */

    //public GameObject Square, Circle, Hexagon, Triangle;
    public TextMesh liveText;
    public GameObject spawnPoint;
    GameObject player;
    GameObject toSpawn;
    public int shapes, timer, Lives;
    public float shapeTimer,posX,posY;
    public int PlayerChoice, shape, Correct;
    bool correct, incorrect;
    SpriteRenderer shapeGame;

    public Sprite[] spriteArray;

    public void StartMinigame(int difficulty)
    {
        shapeGame = spawnPoint.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        spawnPoint = GameObject.Find("Spawn");
        shapeTimer = 2;
        shape = 0;
        Lives = 3;
        Correct = 0;
        PlayerChoice = 0;
        correct = false;
        incorrect = false;
        ShapePick();
    }

    public void ShapePick()
    {
        shapes = Random.Range(1, 5);
        switch (shapes)
        {
            case 1:
                shapeGame.sprite = spriteArray[0];
                shapeGame.color = Color.white;
                shape = 1;
                break;
            case 2:
                shapeGame.sprite = spriteArray[1];
                shapeGame.color = Color.white;
                shape = 2;
                break;
            case 3:
                shapeGame.sprite = spriteArray[2];
                shapeGame.color = Color.white;
                shape = 3;
                break;
            case 4:
                shapeGame.sprite = spriteArray[3];
                shapeGame.color = Color.magenta;
                shape = 4;
                break;
        }
    }

    public void ChoiceCheck()
    {
        switch(PlayerChoice)
        {
            case 0:
                break;
            case 1:
                if (shape == 1) Correct++;
                else incorrect = true; 
                break;
            case 2:
                if (shape == 2) Correct++;
                else incorrect = true;
                break;
            case 3:
                if (shape == 3) Correct++;
                else incorrect = true;
                break;
            case 4:
                if (shape == 4) Correct++;
                else incorrect = true;
                break;
        }

        if (incorrect == true)
        {
            Lives--;
            incorrect = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //randomise the position of the spawned in shapes
        incorrect = false;
        liveText.text = "Lives: " + Lives.ToString();
        shapeTimer -= Time.deltaTime;
        if (shapeTimer <= 0)
        {
            ShapePick();
            posX = Random.Range(-2, -4);
            posY = Random.Range(-2, 2);
            spawnPoint.transform.position = new Vector2(posX, posY);
            shapeTimer = 2;
        }

        if (CurrentInput.RightStick.x < -0.5f)
        {
            player.transform.position = new Vector3(0, 0, 0);
            PlayerChoice = 1;
            ChoiceCheck();
        }
        else if (CurrentInput.RightStick.x > 0.5f)
        {
            player.transform.position = new Vector3(4, 0, 0);
            PlayerChoice = 3;
            ChoiceCheck();
        }
        else if (CurrentInput.RightStick.y < -0.5f)
        {
            player.transform.position = new Vector3(2, -2, 0);
            PlayerChoice = 4;
            ChoiceCheck();
        }
        else if (CurrentInput.RightStick.y > 0.5f)
        {
            player.transform.position = new Vector3(2, 2, 0);
            PlayerChoice = 2;
            ChoiceCheck();
        }
        else
        {
            player.transform.position = new Vector3(2, 0, 0);
            PlayerChoice = 0;
        }


        /*if (PlayerChoice == shapes) correct = !correct;
        if(PlayerChoice != shapes) incorrect = !incorrect;
        
        if (correct == true)
        {
            Correct++;
            shapeTimer = 0;
            correct = false;
        }
        if (incorrect == true)
        {
            Lives--;
            shapeTimer = 0;
            incorrect = false;
        }*/
    }
}
