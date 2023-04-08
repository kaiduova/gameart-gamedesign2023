using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HackableBlockChild : MonoBehaviour {
    [SerializeField] GameObject _linkedHackableBlock;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) { //Player
            collision.gameObject.transform.SetParent(_linkedHackableBlock.transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) { //Player
            collision.gameObject.transform.SetParent(null, true);
        }
    }

    private void Awake() {
        _linkedHackableBlock = transform.parent.gameObject;
    }
}