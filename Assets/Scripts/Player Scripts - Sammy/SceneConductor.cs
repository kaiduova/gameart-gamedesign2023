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
        print("hellodsafd"); 
        if (collision.gameObject.tag == "Player")
        {

            print("hello1");

            Player.SetActive(false);

            LoadNextLevel();


        }
    }

    

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex) {
        animator.SetTrigger("StartFade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}