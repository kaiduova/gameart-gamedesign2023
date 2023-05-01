using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlastZones : MonoBehaviour
{
    public Transform LinkedSpawnPoint;
    [SerializeField] GameObject _player;
    [SerializeField] PlayerController _playerController;

    [SerializeField] float _respawnDelay;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) { //Collides with player
            collision.gameObject.transform.position = LinkedSpawnPoint.position;
            collision.gameObject.GetComponent<PlayerController>().CurrentState = PlayerController.PlayerStates.StateDelay;
            collision.gameObject.GetComponent<PlayerController>().PlayerRespawnDelay = _respawnDelay;
            collision.gameObject.GetComponent<PlayerController>().PlayerHealth--;
        }

        if (collision.gameObject.layer == 8) { //Collides with enemy
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
