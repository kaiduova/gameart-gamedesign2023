using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public enum EatingEnemyState
{
    Default,
    Attack,
    Swallowed,
    Bounce
}

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(UniversalHealthSystem))] 
[RequireComponent(typeof(BouncePad))] 
public class EatingEnemy : MonoBehaviour
{
    public EatingEnemyState State { get; set; }
    public float ReviveTimer { get => _reviveTimer; set => _reviveTimer = value; }
    public float ReviveTime { get => reviveTime; set => reviveTime = value; }

    [SerializeField]
    private GameObject[] patrolPathMarkers, furthestReachablePointMarkers;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float tolerance, speed, chaseSpeed, swallowDuration, postSwallowCooldown, engagementRange, damage, reviveTime;

    [SerializeField]
    private int nextPoint;

    private float _postSwallowCooldownTimer;

    private bool _startedSwallowCoroutine;
    
    private Rigidbody2D _rigidbody;

    private UniversalHealthSystem _health;

    private BouncePad _bouncePad;

    private float _reviveTimer;
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<UniversalHealthSystem>();
        _bouncePad = GetComponent<BouncePad>();
    }

    private void Update()
    {
        _postSwallowCooldownTimer -= Time.deltaTime;
        ReviveTimer -= Time.deltaTime;
        _bouncePad.canBounce = false;

        if (State != EatingEnemyState.Swallowed)
        {
            if (_health.Dead)
            {
                State = EatingEnemyState.Bounce;
                _rigidbody.velocity = Vector2.zero;
                _bouncePad.canBounce = true;
            }
            else if (Vector3.SqrMagnitude(player.transform.position - transform.position) < engagementRange * engagementRange
                && _postSwallowCooldownTimer < 0f)
            {
                State = EatingEnemyState.Attack;
            }
            else
            {
                State = EatingEnemyState.Default;
            }
        }
        
        switch (State)
        {
            case EatingEnemyState.Default:
                gameObject.layer = 8;
                ReviveTimer = ReviveTime;
                //Move between patrol path markers.
                if (transform.position.x > patrolPathMarkers[nextPoint].transform.position.x - tolerance
                    && transform.position.x < patrolPathMarkers[nextPoint].transform.position.x + tolerance)
                {
                    nextPoint = nextPoint == 0 ? 1 : 0;
                }
                else
                {
                    _rigidbody.velocity = speed * (patrolPathMarkers[nextPoint].transform.position.x < transform.position.x
                        ? Vector3.left
                        : Vector3.right);
                }
                break;
            case EatingEnemyState.Attack:
                gameObject.layer = 8;
                ReviveTimer = ReviveTime;
                //Chase within furthest reachable point markers.
                _rigidbody.velocity = Vector3.zero;
                if (transform.position.x > furthestReachablePointMarkers.Min(point => point.transform.position.x)
                    && player.transform.position.x < transform.position.x)
                {
                    _rigidbody.velocity = chaseSpeed * Vector3.left;
                }
                
                if (transform.position.x < furthestReachablePointMarkers.Max(point => point.transform.position.x)
                    && player.transform.position.x > transform.position.x)
                {
                    _rigidbody.velocity = chaseSpeed * Vector3.right;
                }
                
                break;
            case EatingEnemyState.Swallowed:
                gameObject.layer = 8;
                ReviveTimer = ReviveTime;
                //Idle movement and animation.
                if (_startedSwallowCoroutine) return;
                StartCoroutine(Swallow(swallowDuration));
                _startedSwallowCoroutine = true;
                break;
            case EatingEnemyState.Bounce:
                gameObject.layer = 3;
                _rigidbody.velocity = Vector2.zero;
                if (ReviveTimer <= 0f)
                {
                    _health.CurrentHealth = _health.MaxHealth;
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == player && !_startedSwallowCoroutine && State == EatingEnemyState.Attack)
        {
            State = EatingEnemyState.Swallowed;
        }
    }

    private IEnumerator Swallow(float duration)
    {
        player.SetActive(false);
        _rigidbody.velocity = Vector2.zero;
        //Play animations
        yield return new WaitForSeconds(duration);
        player.SetActive(true);
        _startedSwallowCoroutine = false;
        player.GetComponent<PlayerController>().PlayerHealth--;
        //Play more animations
        State = EatingEnemyState.Default;
        _postSwallowCooldownTimer = postSwallowCooldown;
    }
}