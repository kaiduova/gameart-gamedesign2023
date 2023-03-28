using Input;
using Minigame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulanMinigameManager : InputMonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
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
