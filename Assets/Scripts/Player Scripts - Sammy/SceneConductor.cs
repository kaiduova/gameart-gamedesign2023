using UnityEngine;
using UnityEngine.SceneManagement;
using Input;
using System.Collections;

public class SceneConductor : InputMonoBehaviour
{


    public Animator animator;

    public float transitionTime;

    public GameObject Player;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            Player.SetActive(false);
            LoadNextLevel();
        }
    }

    

    public void LoadNextLevel() {
        if (SceneManager.GetActiveScene().buildIndex != 3) StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        else if (SceneManager.GetActiveScene().buildIndex == 3) StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int levelIndex) {
        animator.SetTrigger("StartFade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}