using UnityEngine;

public class PlayerShockwave : MonoBehaviour {

    [SerializeField] GameObject _player;
    [SerializeField] PlayerController _playerController;

    private void Awake() {
        _player = transform.parent.gameObject;
        _playerController = _player.GetComponent<PlayerController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void ResetPlayer() {
        //_playerController.ShockwaveReset();
    }
}
