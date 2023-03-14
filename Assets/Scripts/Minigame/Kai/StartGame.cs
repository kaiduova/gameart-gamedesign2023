using Input;

namespace Minigame.Kai
{
    public class StartGame : InputMonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentInput.GetKeyA)
            {
                MinigameModule.Instance.StartMinigame(Minigame.Hulan, 5);
            }
            
            /*
            if (CurrentInput.GetKeyX) print("x");
            if (CurrentInput.GetKeyY) print("y");
            if (CurrentInput.GetKeyA) print("a");
            if (CurrentInput.GetKeyB) print("b");
            */
        }
    }
}
