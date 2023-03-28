using Input;

namespace Minigame.Kai
{
    public class KaiMinigameManager : InputMonoBehaviour, IMinigameManager
    {
        public void StartMinigame(int difficulty)
        {
            print("Do reset stuff here.");
            print("Set difficulty: " + difficulty);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentInput.GetKeyDownA)
            {
                MinigameModule.Instance.MinigameFinish(true);
            }
        }
    }
}
