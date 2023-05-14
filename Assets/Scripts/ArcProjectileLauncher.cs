using System;
using System.Collections.Generic;
using System.Linq;
using Input;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class ArcProjectileLauncher : InputMonoBehaviour
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float expectedVelocity, turnRate, cooldown, projControlDuration;

    [SerializeField]
    private Rigidbody2D controlledProjectile;

    [SerializeField]
    private Image gaugeBg, gaugeFill;
    
    private readonly List<Vector2> _predictedTrajectoryPoints = new();


    [SerializeField]
    private GameObject spawnObject, playerAnimation;

    private float _cooldown, _projControlTimer;

    public Rigidbody2D ControlledProjectile { get => controlledProjectile; set => controlledProjectile = value; }

    public static ArcProjectileLauncher Instance { get; private set; }

    public float ProjControlDuration
    {
        get => projControlDuration;
        set => projControlDuration = value;
    }

    private PlayerController _playerController;
    
    private Rigidbody2D _rigidbody;

    private LineRenderer _lineRenderer;

    private float _projRad;

    private void Awake()
    {
        Instance = this;

        _playerController = GetComponent<PlayerController>();
        

        _rigidbody = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        var instance = Instantiate(projectile);
        _projRad = instance.GetComponent<Collider2D>().bounds.extents.x;
        Destroy(instance);
        gaugeBg.enabled = false;
        gaugeFill.enabled = false;
    }

    public Rigidbody2D Fire(Vector3 origin, Vector3 direction)
    {
        var proj = Instantiate(projectile, origin, Quaternion.identity);
        var rigidbodyProj = proj.GetComponent<Rigidbody2D>();
        rigidbodyProj.velocity = expectedVelocity * direction.normalized;
        _projControlTimer = projControlDuration;
        return rigidbodyProj;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        _projControlTimer -= Time.deltaTime;
        if (controlledProjectile == null && CurrentInput.GetKeyDownRT && _cooldown < 0f && _playerController.CurrentState == PlayerController.PlayerStates.NeutralMovement)
        {
            Vector2 direction;
            direction = CurrentInput.RightStick;
            if ((CurrentInput.RightStick - Vector2.zero).sqrMagnitude < 0.1f)
            {
                direction = playerAnimation.transform.eulerAngles.y == 0f ? Vector2.left : Vector2.right;
            }
            _cooldown = cooldown;
            controlledProjectile = Fire(spawnObject.transform.position, direction);
        }

        if (controlledProjectile != null)
        {
            var decimalTimeRemaining = _projControlTimer / ProjControlDuration;
            gaugeBg.enabled = true;
            gaugeFill.enabled = true;
            gaugeFill.fillAmount = 1 - decimalTimeRemaining;
            
            if (CurrentInput.GetKeyUpRT || (controlledProjectile.transform.position - transform.position).sqrMagnitude > ProjControlDuration * ProjControlDuration)
            {
                RemoveControl();
            }
        }
        else
        {
            gaugeBg.enabled = false;
            gaugeFill.enabled = false;
        }

        if ((CurrentInput.RightStick - Vector2.zero).sqrMagnitude > 0.2f && !CurrentInput.GetKeyRT && _playerController.CurrentState == PlayerController.PlayerStates.NeutralMovement)
        {
            _lineRenderer.enabled = true;
            Predict();
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    public void RemoveControl()
    {
        controlledProjectile.gameObject.GetComponent<LineRenderer>().enabled = false;
        controlledProjectile.gameObject.GetComponent<ArcProjectile>().IncreaseSpeedAndRemoveControl();
        controlledProjectile = null;
    }
    
    private void FixedUpdate()
    {
        if (CurrentInput.GetKeyRT && controlledProjectile != null)
        {
            var velocity = controlledProjectile.velocity;
            var originalMagnitude = velocity.magnitude;
            velocity = (velocity.normalized + turnRate * CurrentInput.RightStick).normalized * originalMagnitude;
            controlledProjectile.velocity = velocity;
        }
    }

    private void Predict()
    {
        _predictedTrajectoryPoints.Clear();
        var position = spawnObject.transform.position;
        Vector2 iterationOrigin = position;
        var iterationDirection = CurrentInput.RightStick.normalized;
        _predictedTrajectoryPoints.Add(position);
        for (var i = 0; i < 10; i++)
        {
            TryRaycastNonReflect(iterationOrigin, iterationDirection, out var hitNonReflect);
            TryRaycast(iterationOrigin, iterationDirection, out var hit);
            if (hit.collider != null && hitNonReflect.collider != null)
            {
                if (hit.distance > hitNonReflect.distance)
                {
                    var finalTarget = hitNonReflect.point;
                    _predictedTrajectoryPoints.Add(finalTarget);
                    break;
                }
            }
            else if (hitNonReflect.collider != null)
            {
                var finalTarget = hitNonReflect.point;
                _predictedTrajectoryPoints.Add(finalTarget);
                break;
            }
            
            if (hit.collider == null)
            {
                var finalTarget = iterationOrigin + iterationDirection * 500f;
                _predictedTrajectoryPoints.Add(finalTarget);
                break;
            }
            _predictedTrajectoryPoints.Add(hit.centroid);
            if (!hit.collider.gameObject.TryGetComponent<DeflectingBlock>(out _)) break;
            iterationOrigin = hit.centroid;
            iterationDirection = Vector2.Reflect(iterationDirection, hit.normal);
        }

        _lineRenderer.positionCount = _predictedTrajectoryPoints.Count;

        _lineRenderer.SetPositions(Vector2ToVector3Array(_predictedTrajectoryPoints));
    }
    
    private void TryRaycast(Vector2 origin, Vector2 direction, out RaycastHit2D hit)
    {
        var layerMask = LayerMask.GetMask("Deflecting");
        hit = Physics2D.CircleCast(origin + direction * 0.005f, _projRad, direction, 500f, layerMask);
    }
    
    private void TryRaycastNonReflect(Vector2 origin, Vector2 direction, out RaycastHit2D hit)
    {
        var layerMask = LayerMask.GetMask("Ground", "Enemy");
        hit = Physics2D.CircleCast(origin + direction * 0.005f, _projRad, direction, 500f, layerMask);
    }
    
    private static Vector3[] Vector2ToVector3Array(IEnumerable<Vector2> input)
    {
        return input.Select(element => new Vector3(element.x, element.y, 0)).ToArray();
    }
}