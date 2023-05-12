using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlastZones : MonoBehaviour {

    [Header("Externally Referenced Components")]
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerController _playerController;
    public Animator PlayerAnim;
    public GameObject GhostHand; 
    public Transform LinkedSpawnPoint;
    public Animator OOBMask;

    [Header("Blast Zone Attributes")]
    [SerializeField] private float _respawnDelay = 2f;

    private IEnumerator ResetScene()
    {
        OOBMask.SetTrigger("OOB");
        _playerController.PlayerRespawnDelay = _respawnDelay;
        _playerController.CurrentState = PlayerController.PlayerStates.StateDelay;
        yield return new WaitForSeconds(1);
        if (GhostHand.activeInHierarchy) {
            GhostHand.SetActive(false);
            PlayerAnim.SetBool("ghostHandMode", false);
        }
        _player.transform.position = LinkedSpawnPoint.position;
        _playerController.PlayerHealth--;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 6) { //Collides with player
            StartCoroutine(ResetScene());
        }

        if (collision.gameObject.layer == 8) { //Collides with enemy
            Destroy(collision.gameObject);
        }
    }

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }
}
