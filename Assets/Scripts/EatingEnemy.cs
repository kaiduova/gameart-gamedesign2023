using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using static PlayerController;

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

    [SerializeField] private Image gaugeBG, gaugeFill;


    [SerializeField] private Animator _eatingEnemyAnim;

    //Added by Sammy
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _knockbackIntensity;
    public CinemachineImpulseSource ScreenShake;

    private void CallScreenShake() {
        ScreenShake.GenerateImpulse();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<UniversalHealthSystem>();
        _bouncePad = GetComponent<BouncePad>();

        gaugeBG.enabled = false;
        gaugeFill.enabled = false;
        _canRotate = true;


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
            _eatingEnemyAnim.gameObject.transform.localRotation = inverted;
        }
        else
        {
            _eatingEnemyAnim.gameObject.transform.localRotation = Quaternion.identity;
        }
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
            else if (Vector3.SqrMagnitude(_player.transform.position - transform.position) < engagementRange * engagementRange
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

                gaugeBG.enabled = false;
                gaugeFill.enabled = false;

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
            case EatingEnemyState.Attack:
                gameObject.layer = 8;
                ReviveTimer = ReviveTime;
                //Chase within furthest reachable point markers.
                _rigidbody.velocity = Vector3.zero;
                if (transform.position.x > furthestReachablePointMarkers.Min(point => point.transform.position.x)
                    && _player.transform.position.x < transform.position.x)
                {
                    _rigidbody.velocity = chaseSpeed * Vector3.left;
                }
                
                gaugeBG.enabled = false;
                gaugeFill.enabled = false;
                
                if (transform.position.x < furthestReachablePointMarkers.Max(point => point.transform.position.x)
                    && _player.transform.position.x > transform.position.x)
                {
                    _rigidbody.velocity = chaseSpeed * Vector3.right;
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
                gameObject.layer = 8;
                ReviveTimer = ReviveTime;
                //Idle movement and animation.
                if (_startedSwallowCoroutine) return;
                StartCoroutine(Swallow(swallowDuration));
                _startedSwallowCoroutine = true;
                gaugeBG.enabled = false;
                gaugeFill.enabled = false;
                break;
            case EatingEnemyState.Bounce:
                gameObject.layer = 3;
                _rigidbody.velocity = Vector2.zero;
                if (ReviveTimer <= 0f)
                {
                    _health.CurrentHealth = _health.MaxHealth;
                }
                gaugeBG.enabled = true;
                gaugeFill.enabled = true;
                gaugeFill.fillAmount = _reviveTimer / reviveTime;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == _player && !_startedSwallowCoroutine && State == EatingEnemyState.Attack)
        {
            State = EatingEnemyState.Swallowed;
        }
    }

    private IEnumerator Swallow(float duration)
    {
        _player.SetActive(false);
        CallScreenShake();
        _canRotate = false;
        _rigidbody.velocity = Vector2.zero;
        //Play animations
        yield return new WaitForSeconds(duration);
        _eatingEnemyAnim.SetTrigger("SpittingOutPlayer");
        _player.SetActive(true);
        _playerController.CurrentState = PlayerStates.Knockback;
        _player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 0.5f, _player.transform.position.z);
        CalculatePlayerKnockback();
        _startedSwallowCoroutine = false;
        //Play more animations
        State = EatingEnemyState.Default;
        _postSwallowCooldownTimer = postSwallowCooldown;
        yield return new WaitForSeconds(0.5f);
        _canRotate = true;
    }

    private void CalculatePlayerKnockback()
    {
        if (transform.position.x > _player.transform.position.x || transform.position.x == _player.transform.position.x) StartCoroutine(_playerController.Knockback(-_knockbackIntensity));
        if (transform.position.x < _player.transform.position.x) StartCoroutine(_playerController.Knockback(_knockbackIntensity));
    }
}