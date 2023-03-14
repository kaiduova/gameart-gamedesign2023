using System;
using System.Linq;
using Minigame;
using UnityEngine;

namespace Kai_Minigame
{
    [Serializable]
    public struct ShotData
    {
        public Vector2 shotVector;
        public int difficulty;
        public float cooldown;
    }
    
    public class BallGameManager : MonoBehaviour, IMinigameManager
    {
        [SerializeField]
        private ShotData[] possibleShots;

        public ShotData[] DifficultyFilteredShots { get; private set; }

        public void StartMinigame(int difficulty)
        {
            DifficultyFilteredShots = possibleShots.Where(data => data.difficulty == difficulty).ToArray();
        }
    }
}
