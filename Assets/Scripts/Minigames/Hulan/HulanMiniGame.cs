using Input;
using Minigame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulanMiniGame : InputMonoBehaviour, IMinigameManager 
{
    public GameObject square, circle, hexagon, triangle;
    GameObject spawnPoint;
    GameObject player;
    GameObject toSpawn;
    public int shapes, timer, framerate, shapeID;
    public int PlayerChoice, shape, Correct;

    public void StartMinigame(int difficulty)
    {
        player = GameObject.Find("Player");
        spawnPoint = GameObject.Find("Spawn");
        framerate = 60;
        timer = 10;
        shapeID = 1;
        shape = 0;
        Correct = 0;
        PlayerChoice = 0;
        ShapePick();
    }

    public void ShapePick()
    {
        shapes = Random.Range(1, 5);
        switch (shapes)
        {
            case 1:
                toSpawn = square;
                shape = 2;
                break;
            case 2:
                toSpawn = circle;
                shape = 1;
                break;
            case 3:
                toSpawn = hexagon;
                shape = 3;
                break;
            case 4:
                toSpawn = triangle;
                shape = 4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        framerate--;
        if (framerate < 0)
        {
            timer--;
            framerate = 60;
            if (timer < 0)
            {
                ShapePick();
                GameObject game = Instantiate(toSpawn, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0), Quaternion.identity);
                game.name = shapeID.ToString();
                shapeID++;
                timer = 10;
                framerate = 60;
            }
        }

        if (CurrentInput.RightStick.x < -0.5f)
        {
            player.transform.position = new Vector3(0, 0, 0);
            PlayerChoice = 1;
        }
        else if (CurrentInput.RightStick.y < -0.5f)
        {
            player.transform.position = new Vector3(2, -2, 0);
            PlayerChoice = 2;
        }
        else if (CurrentInput.RightStick.x > 0.5f)
        {
            player.transform.position = new Vector3(2, -2, 0);
            PlayerChoice = 4;
        }
        else if (CurrentInput.RightStick.y > 0.5f)
        {
            player.transform.position = new Vector3(4, 0, 0);
            PlayerChoice = 3;
        }
        else
        {
            player.transform.position = new Vector3(2, 0, 0);
            PlayerChoice = 0;
        }


        if (PlayerChoice == shapes)
        {
            Correct++;
        }
    }
}
