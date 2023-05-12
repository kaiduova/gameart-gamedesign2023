using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeflectingBlock : MonoBehaviour {

    [HideInInspector] public Rigidbody2D Rigidbody2D;

    [SerializeField] private GameObject _highlight;



    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("GhostHandCollider")) _highlight.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("GhostHandCollider")) _highlight.SetActive(false);
    }

    private void Awake() {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _highlight = transform.GetChild(1).gameObject;
    }

    private void Start() {
        gameObject.layer = 13;
    }
}