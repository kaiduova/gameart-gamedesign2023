using Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : InputMonoBehaviour
{
    private bool _paused;

    [SerializeField]
    private Image pauseMenuImage;
    
    [SerializeField]
    private int level1Index, level2Index, level3Index;

    private void Start()
    {
        pauseMenuImage.enabled = false;
    }

    private void Update()
    {
        if (CurrentInput.GetKeyDownStart)
        {
            _paused = !_paused;
        }

        if (_paused)
        {
            pauseMenuImage.enabled = true;
            Time.timeScale = 0;
            if (CurrentInput.DPad == Vector2.left)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(level1Index);
            }
            else if (CurrentInput.DPad == Vector2.up)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(level2Index);
            }
            else if (CurrentInput.DPad == Vector2.right)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(level3Index);
            }
        }
        else
        {
            pauseMenuImage.enabled = false;
            Time.timeScale = 0;
        }
    }
}
