using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HealthPack : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) collision.gameObject.GetComponent<PlayerController>().PlayerHealth++;
        Destroy(gameObject);
    }
}
