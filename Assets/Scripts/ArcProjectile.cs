using System.Collections.Generic;
using System.Linq;
using Input;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class ArcProjectile : InputMonoBehaviour
{

    private bool _bounced;

    [SerializeField]
    private float postBounceSpeedMultiplier, lifetime, additionalLifetimeOnBounce, damage;

    private Rigidbody2D _rigidbody;

    private LineRenderer _lineRenderer;

    private CircleCollider2D _collider;

    private readonly Queue<Vector2> _cachedVelocities = new();
    private Vector2 _lateVelocity;

    private readonly List<Vector2> _predictedTrajectoryPoints = new();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
        
        _predictedTrajectoryPoints.Clear();
        var position = transform.position;
        Vector2 iterationOrigin = position;
        var iterationDirection = _rigidbody.velocity.normalized;
        _predictedTrajectoryPoints.Add(position);
        for (var i = 0; i < 10; i++)
        {
            TryRaycast(iterationOrigin, iterationDirection, out var hit);
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
        hit = Physics2D.CircleCast(origin + direction * 0.005f, _collider.bounds.extents.x, direction, 500f, layerMask);
        return;
    }
    
    private void FixedUpdate()
    {
        _cachedVelocities.Enqueue(_rigidbody.velocity);
        if (_cachedVelocities.Count > 2)
        {
            _lateVelocity = _cachedVelocities.Dequeue();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        UniversalHealthSystem.TryDealDamage(col.gameObject, damage);
        if (col.collider.gameObject.TryGetComponent<ArcProjectileLauncher>(out _)) return;
        if (!col.collider.gameObject.TryGetComponent<DeflectingBlock>(out _))
        {
            Destroy(gameObject);
            return;
        }

        var newVelocity = _lateVelocity;
            
        if (!_bounced)
        {
            _lateVelocity *= postBounceSpeedMultiplier;
        }
        _bounced = true;
        gameObject.GetComponent<LineRenderer>().enabled = false;
        lifetime = additionalLifetimeOnBounce;
        if (ArcProjectileLauncher.Instance.ControlledProjectile != null && 
            ArcProjectileLauncher.Instance.ControlledProjectile.gameObject != null && 
            ArcProjectileLauncher.Instance.ControlledProjectile.gameObject == gameObject)
        {
            ArcProjectileLauncher.Instance.ControlledProjectile = null;
        }
        
        _lateVelocity = Vector2.Reflect(_lateVelocity, col.GetContact(0).normal).normalized * _lateVelocity.magnitude;
        _rigidbody.velocity = _lateVelocity;
    }

    private static Vector3[] Vector2ToVector3Array(IEnumerable<Vector2> input)
    {
        return input.Select(element => new Vector3(element.x, element.y, 0)).ToArray();
    }
}