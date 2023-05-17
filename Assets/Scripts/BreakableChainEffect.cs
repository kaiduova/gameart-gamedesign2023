using System.Collections;
using UnityEngine;

public class BreakableChainEffect : MonoBehaviour {
    [SerializeField] private UniversalHealthSystem _health;
    [SerializeField] private Animator _animator;
    public ParticleSystem ChainEffect;
    public GameObject ChainShock;

    private void Awake() {
        _health = transform.GetComponentInParent<UniversalHealthSystem>();
        _animator = GetComponent<Animator>();
        ChainEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void BreakChainEffect() {
        ChainEffect.Play();
        if (ChainShock != null) StartCoroutine(ChainShockEffect());
    }

    private IEnumerator ChainShockEffect() {
        ChainShock.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        ChainShock.SetActive(false);
    }

    private void Update() {
        if (_health.Dead) _animator.SetTrigger("ChainBreak");
    }
}
