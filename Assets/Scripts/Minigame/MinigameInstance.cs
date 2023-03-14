using System;
using UnityEngine;

namespace Minigame
{
    public class MinigameInstance : MonoBehaviour
    {
        public Minigame thisMinigame;

        public int Difficulty { get; set; }

        private IMinigameManager[] _minigameManager;

        private void Awake()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.layer = 10;
            }
            _minigameManager = GetComponentsInChildren<IMinigameManager>();
        }

        public void StartMinigame(int difficulty)
        {
            foreach (var manager in _minigameManager)
            {
                manager.StartMinigame(difficulty);
            }
        }
    }
}
