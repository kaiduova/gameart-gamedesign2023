using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    public DynamicTutorialText DynamicTutorialText;

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.CompareTag("Player")) DynamicTutorialText.CrossedWall = true; }
}
