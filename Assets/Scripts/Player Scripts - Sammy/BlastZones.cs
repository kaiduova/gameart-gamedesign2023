using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlastZones : MonoBehaviour
{
    public Transform LinkedSpawnPoint;
    [SerializeField] GameObject _player;
    [SerializeField] PlayerController _playerController;

    [SerializeField] private float _respawnDelay = 1f;



    public Animator OOBMask;


    private IEnumerator ResetScene()
    {
        OOBMask.SetTrigger("OOB");
        _playerController.PlayerRespawnDelay = _respawnDelay;
        _playerController.CurrentState = PlayerController.PlayerStates.StateDelay;
        yield return new WaitForSeconds(1);
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

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
