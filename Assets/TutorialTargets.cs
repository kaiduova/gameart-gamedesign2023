using UnityEngine;

public class TutorialTargets : MonoBehaviour {

    public DynamicTutorialText DynamicTutorialText;
    public GameObject LinkedObject;

    private SpriteRenderer _spriteRenderer;
    public bool Hit = false;

    public int TargetOrder;

    private float deactivateTimer;

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((DynamicTutorialText.PlayerMoved && DynamicTutorialText.PlayerJumped) && TargetOrder == 1) {
            if (collision.CompareTag("PlayerBullet")) {
                _spriteRenderer.color = Color.green;
                Destroy(collision.gameObject);
                Hit = true;
            }
        }
        else if (TargetOrder == 2) {
            if (collision.CompareTag("PlayerBullet")) {
                _spriteRenderer.color = Color.green;
                Destroy(collision.gameObject);
                Hit = true;
            }
        }
    }

    private void Awake() { _spriteRenderer = GetComponent<SpriteRenderer>(); }

    private void Update() {
        if (LinkedObject != null && Hit) {
            deactivateTimer += Time.deltaTime;
            if (deactivateTimer >= 1) {
                LinkedObject.SetActive(false);
                LinkedObject = null;
                deactivateTimer = 0;
            }
        }
    }
}