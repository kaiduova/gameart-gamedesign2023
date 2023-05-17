﻿using System;
using System.Collections.Generic;
using System.Linq;
using Input;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(LineRenderer))]
public class ArcProjectile : InputMonoBehaviour
{
    [SerializeField]
    private float postBounceSpeedMultiplier, lifetime, additionalLifetimeOnBounce, damage;

    [SerializeField]
    private Material uncontrolledMaterial;

    private Rigidbody2D _rigidbody;

    private LineRenderer _lineRenderer;

    private CircleCollider2D _collider;

    private readonly Queue<Vector2> _cachedVelocities = new();
    private Vector2 _lateVelocity;

    private readonly List<Vector2> _predictedTrajectoryPoints = new();
    
    [SerializeField]
    private Image gaugeBg, gaugeFill;
    
    private float _projControlTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        gaugeBg.enabled = true;
        gaugeFill.enabled = true;
        _projControlTimer = ArcProjectileLauncher.Instance.ProjControlDuration;
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.forward);
        transform.rotation = rotation;


        lifetime -= Time.deltaTime;
        _projControlTimer -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
        
        var decimalTimeRemaining = _projControlTimer / ArcProjectileLauncher.Instance.ProjControlDuration;
        gaugeFill.fillAmount = decimalTimeRemaining;
        
        Predict();
    }

    public void IncreaseSpeedAndRemoveControl()
    {
        //GetComponent<MeshRenderer>().material = uncontrolledMaterial;
        _rigidbody.velocity *= postBounceSpeedMultiplier;
        gaugeBg.enabled = false;
        gaugeFill.enabled = false;
    }

    private void Predict()
    {
        _predictedTrajectoryPoints.Clear();
        var position = transform.position;
        Vector2 iterationOrigin = position;
        var iterationDirection = _rigidbody.velocity.normalized;;
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
        hit = Physics2D.CircleCast(origin + direction * 0.005f, _collider.bounds.extents.x, direction, 500f, layerMask);
    }
    
    private void TryRaycastNonReflect(Vector2 origin, Vector2 direction, out RaycastHit2D hit)
    {
        var layerMask = LayerMask.GetMask("Ground", "Enemy");
        hit = Physics2D.CircleCast(origin + direction * 0.005f, _collider.bounds.extents.x, direction, 500f, layerMask);
    }
    
    private void FixedUpdate()
    {
        _cachedVelocities.Enqueue(_rigidbody.velocity);
        if (_cachedVelocities.Count > 0)
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
        
        gameObject.GetComponent<LineRenderer>().enabled = false;
        lifetime = additionalLifetimeOnBounce;
        _lateVelocity = Vector2.Reflect(_lateVelocity, col.GetContact(0).normal).normalized * _lateVelocity.magnitude;
        _rigidbody.velocity = _lateVelocity;
        if (ArcProjectileLauncher.Instance.ControlledProjectile != null && 
            ArcProjectileLauncher.Instance.ControlledProjectile.gameObject != null && 
            ArcProjectileLauncher.Instance.ControlledProjectile.gameObject == gameObject)
        {
            ArcProjectileLauncher.Instance.RemoveControl();
        }
    }

    private static Vector3[] Vector2ToVector3Array(IEnumerable<Vector2> input)
    {
        return input.Select(element => new Vector3(element.x, element.y, 0)).ToArray();
    }
}