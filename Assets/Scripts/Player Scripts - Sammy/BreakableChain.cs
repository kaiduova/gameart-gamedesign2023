using UnityEngine;
using static ChainBlock;

public class BreakableChain : MonoBehaviour {

    /* PascalCase - ClassNames, PublicMemberVariables, ProtectedMemberVariables, Methods & Functions
    camelCase - parameters, arguments, methodVariables, functionVariables
    _camelCase - privateMemberVariables */

    [Header("Internally Referenced Components")]
    [SerializeField] private UniversalHealthSystem _healthSystem;

    [Header("Externally Referenced Components")]
    public GameObject LinkedChainBlock;
    [SerializeField] private ChainBlock _linkedChainBlockScript;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerBullet")  {
            _healthSystem.TakeDamage(collision.GetComponent<PlayerBullet>().DamageValue);
            Destroy(collision.gameObject);
        }
    }

    private void Awake() {
        _healthSystem = GetComponent<UniversalHealthSystem>();
        _linkedChainBlockScript = LinkedChainBlock.GetComponent<ChainBlock>();
    }

    private void Start() {
        _linkedChainBlockScript.CurrentState = ChainBlockStates.Chained;
    }

    private void Update() {
        if (_healthSystem.CurrentHealth <= 0 && _linkedChainBlockScript.CurrentState == ChainBlockStates.Chained) _linkedChainBlockScript.CurrentState = ChainBlockStates.FreeFall;
    }
}