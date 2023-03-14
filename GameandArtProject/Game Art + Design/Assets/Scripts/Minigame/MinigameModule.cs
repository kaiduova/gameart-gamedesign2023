using System;
using System.Linq;
using UnityEngine;

namespace Minigame
{
    public enum Minigame
    {
        None,
        Sammy,
        Hulan,
        Skenat,
        Kai
    }
    
    public class MinigameModule : MonoBehaviour
    {
        public Minigame CurrentMinigame { get; set; }
        private MinigameInstance[] _minigameInstances;
        public static MinigameModule Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                Debug.LogError("There shouldn't be two instances of MinigameModule, the other one has been destroyed.");
            }
            Instance = this;
            _minigameInstances = GetComponentsInChildren<MinigameInstance>();
            foreach (var minigame in _minigameInstances)
            {
                minigame.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Call to start a minigame.
        /// </summary>
        /// <param name="minigame"></param>
        /// <param name="difficulty"></param>
        public void StartMinigame(Minigame minigame, int difficulty)
        {
            if (CurrentMinigame != Minigame.None) return;
            CurrentMinigame = minigame;
            var game = _minigameInstances.First(instance => instance.thisMinigame == minigame);
            game.gameObject.SetActive(true);
            game.Difficulty = difficulty;
            game.StartMinigame(difficulty);
        }
        
        /// <summary>
        /// Call to end a minigame.
        /// </summary>
        /// <param name="isWon"></param>
        public void MinigameFinish(bool isWon)
        {
            if (CurrentMinigame == Minigame.None) return;
            var game = _minigameInstances.First(instance => instance.thisMinigame == CurrentMinigame);
            game.gameObject.SetActive(false);
            print(CurrentMinigame + " finished and win is " + isWon);
            CurrentMinigame = Minigame.None;
            //do something
        }
    }
}
