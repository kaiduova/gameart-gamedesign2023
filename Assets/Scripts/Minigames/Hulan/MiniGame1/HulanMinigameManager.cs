using Input;
using Minigame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulanMinigameManager : InputMonoBehaviour
{
    public HulanMiniGame game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<HulanMiniGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentInput.GetKeyLeftStickPress)
        {
            MinigameModule.Instance.StartMinigame(Minigame.Minigame.Hulan, 1);
        }

    }
}
