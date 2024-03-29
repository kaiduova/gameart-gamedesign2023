﻿using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static PlayerController;
using static GhostHand;

public enum EatingEnemyState {
    Default,
    Attack,
    Swallowed,
    Bounce,
    WakeUp,

    PlayerNoticed
}

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(UniversalHealthSystem))] 
[RequireComponent(typeof(BouncePad))] 
public class EatingEnemy : MonoBehaviour {

    public EatingEnemyState State { get; set; }
    public float ReviveTimer { get => _reviveTimer; set => _reviveTimer = value; }
    public float ReviveTime { get => reviveTime; set => reviveTime = value; }

    [SerializeField]
    private GameObject[] patrolPathMarkers, furthestReachablePointMarkers, secondPatrolPathMarkerSet, secondFurthestReachablePointSet, lights;


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

    private bool _canRotate;

    public bool useSecondSet;

    [SerializeField] private Image gaugeFill;
    //[SerializeField] private Image gaugeBG, gaugeFill;

    [SerializeField] private GameObject _enemyGaugeUI;


    [SerializeField] public Animator EatingEnemyAnim;

    //Added by Sammy
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _knockbackIntensity;
    public CinemachineImpulseSource ScreenShake;

    public float _playerNoticedDelay;
    public float _playerNoticedDelayReset;

    public float _wakeUpDelay;
    public float _wakeUpRelayReset;

    public bool playerNoticed;

    public GameObject SpitPoint;

    [SerializeField] private GameObject _ghostHandObject;
    [SerializeField] private GhostHand _ghostHand;

    public bool PermaBounce;

    public Animator _animator;


    private void CallScreenShake() {
        ScreenShake.GenerateImpulse();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<UniversalHealthSystem>();
        _bouncePad = GetComponent<BouncePad>();

        _enemyGaugeUI.SetActive(false);
        //gaugeBG.enabled = false;
        //gaugeFill.enabled = false;

        _canRotate = true;


        _ghostHand = _ghostHandObject.GetComponent<GhostHand>();

        _playerController = _player.GetComponent<PlayerController>();
    }

    private void RotateAnimation(bool invert)
    {
        if (invert)
        {
            var inverted = new Quaternion
            {
                eulerAngles = new Vector3(0, 180, 0)
            };
            //EatingEnemyAnim.gameObject.transform.localRotation = inverted; //Changed by sammy, needs to rotate children objects too
            transform.localRotation = inverted;
        }
        else
        {
            //EatingEnemyAnim.gameObject.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity; 
        }
    }

    private void Update()
    {
        print(State);

        if (PermaBounce)
        {
            _enemyGaugeUI.SetActive(false);
            State = EatingEnemyState.Bounce;
            _animator.SetTrigger("PermaBounce");
            reviveTime = ReviveTime;
        }


        if (useSecondSet)
        {
            patrolPathMarkers = secondPatrolPathMarkerSet;
            furthestReachablePointMarkers = secondFurthestReachablePointSet;
        }

        _postSwallowCooldownTimer -= Time.deltaTime;
        ReviveTimer -= Time.deltaTime;
        _bouncePad.canBounce = false;

        if (State != EatingEnemyState.Swallowed && State != EatingEnemyState.WakeUp)
        {
            if (_health.Dead)
            {
                State = EatingEnemyState.Bounce;
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                _bouncePad.canBounce = true;
            }
            else if (Vector3.SqrMagnitude(_player.transform.position - transform.position) < engagementRange * engagementRange
                && _postSwallowCooldownTimer < 0f)
            {
                //State = EatingEnemyState.Attack;

                //Go to state delay, wait for animation then go to attack




                if (!playerNoticed) State = EatingEnemyState.PlayerNoticed;
                else State = EatingEnemyState.Attack;
            }
            else
            {
                playerNoticed = false;
                State = EatingEnemyState.Default;
            }
        }
        
        switch (State)
        {
            case EatingEnemyState.Default:
                foreach (var enemyLight in lights)
                {
                    enemyLight.SetActive(true);
                }
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
                    _rigidbody.velocity = new Vector2((speed * (patrolPathMarkers[nextPoint].transform.position.x < transform.position.x
                        ? Vector3.left
                        : Vector3.right)).x, _rigidbody.velocity.y);
                }

                _enemyGaugeUI.SetActive(false);
                //gaugeBG.enabled = false;
                //gaugeFill.enabled = false;

                if (_canRotate)
                {
                    if (_rigidbody.velocity.x > 0f)
                    {
                        RotateAnimation(true);
                    }
                    else
                    {
                        RotateAnimation(false);
                    }
                }

                break;


            case EatingEnemyState.PlayerNoticed:
                {
                    foreach (var enemyLight in lights)
                    {
                        enemyLight.SetActive(true);
                    }
                    _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);

                    _playerNoticedDelay += Time.deltaTime;
                    if (_playerNoticedDelay >= _playerNoticedDelayReset)
                    {
                        _playerNoticedDelay = 0;
                        playerNoticed = true;
                        State = EatingEnemyState.Attack;
                    }

                }
                break;

            case EatingEnemyState.Attack:
                foreach (var enemyLight in lights)
                {
                    enemyLight.SetActive(true);
                }
                gameObject.layer = 8;
                ReviveTimer = ReviveTime;
                //Chase within furthest reachable point markers.
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                if (transform.position.x > furthestReachablePointMarkers.Min(point => point.transform.position.x)
                    && _player.transform.position.x < transform.position.x)
                {
                    _rigidbody.velocity = new Vector2((chaseSpeed * Vector3.left).x, _rigidbody.velocity.y);
                }

                _enemyGaugeUI.SetActive(false);
                //gaugeBG.enabled = false;
                //gaugeFill.enabled = false;

                if (transform.position.x < furthestReachablePointMarkers.Max(point => point.transform.position.x)
                    && _player.transform.position.x > transform.position.x)
                {
                    _rigidbody.velocity = new Vector2((chaseSpeed * Vector3.right).x, _rigidbody.velocity.y);
                }
                
                if (_rigidbody.velocity.x > 0f)
                {
                    RotateAnimation(true);
                }
                else
                {
                    RotateAnimation(false);
                }
                
                break;
            case EatingEnemyState.Swallowed:
                foreach (var enemyLight in lights)
                {
                    enemyLight.SetActive(true);
                }
                playerNoticed = false;

                gameObject.layer = 8;
                ReviveTimer = ReviveTime;
                //Idle movement and animation.
                if (_startedSwallowCoroutine) return;
                StartCoroutine(Swallow(swallowDuration));
                _startedSwallowCoroutine = true;

                _enemyGaugeUI.SetActive(false);
                //gaugeBG.enabled = false;
                //gaugeFill.enabled = false;

                break;
            case EatingEnemyState.Bounce:
                foreach (var enemyLight in lights)
                {
                    enemyLight.SetActive(false);
                }
                gameObject.layer = 3;
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                if (ReviveTimer <= 0f && _health.MaxHealth != 0)
                {
                    _health.CurrentHealth = _health.MaxHealth;
                    StartCoroutine(WakeUp(1.1f));
                }

                if (_health.MaxHealth == 0)
                {
                    _enemyGaugeUI.SetActive(false);
                }
                else
                {
                    _enemyGaugeUI.SetActive(true);
                }
                //gaugeBG.enabled = true;
                //gaugeFill.enabled = true;

                gaugeFill.fillAmount = _reviveTimer / reviveTime;
                break;

            case EatingEnemyState.WakeUp:
                {
                    foreach (var enemyLight in lights)
                    {
                        enemyLight.SetActive(true);
                    }
                    _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);

                    _wakeUpDelay += Time.deltaTime;
                    if (_wakeUpDelay >= _wakeUpRelayReset) {
                        _wakeUpDelay = 0;
                        playerNoticed = false;
                        State = EatingEnemyState.Default;
                    }
                }
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject == _player && !_startedSwallowCoroutine && State == EatingEnemyState.Attack)
        {
            State = EatingEnemyState.Swallowed;
        }
    }

    private IEnumerator Swallow(float duration)
    {
        if (_ghostHandObject.activeInHierarchy)
        {
            _ghostHand.InitialiseTotemBlocks();
            _ghostHandObject.SetActive(false);
        }



         
        _player.SetActive(false);
        CallScreenShake();
        _canRotate = false;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        //Play animations
        EatingEnemyAnim.SetBool("Chew", true);
        yield return new WaitForSeconds(duration);
        _player.SetActive(true);
        _playerController.CurrentState = PlayerStates.Knockback;
        _player.transform.position = new Vector3(SpitPoint.transform.position.x, SpitPoint.transform.position.y, _player.transform.position.z);
        CalculatePlayerKnockback();
        _startedSwallowCoroutine = false;
        EatingEnemyAnim.SetBool("Chew", false);
        //Play more animations
        State = EatingEnemyState.Default;
        _postSwallowCooldownTimer = postSwallowCooldown;
        yield return new WaitForSeconds(0.5f);
        _canRotate = true;
    }

    private IEnumerator WakeUp(float delay)
    {
        State = EatingEnemyState.WakeUp;
        yield return new WaitForSeconds(delay);
        State = EatingEnemyState.Default;
    }

    private void CalculatePlayerKnockback()
    {
        if (transform.position.x > _player.transform.position.x || transform.position.x == _player.transform.position.x) StartCoroutine(_playerController.Knockback(-_knockbackIntensity));
        if (transform.position.x < _player.transform.position.x) StartCoroutine(_playerController.Knockback(_knockbackIntensity));
    }
}