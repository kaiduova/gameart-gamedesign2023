using System;
using Input;
using UnityEngine;

public class ArcProjectileLauncher : InputMonoBehaviour
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float expectedVelocity, turnRate, cooldown, maxRange;

    [SerializeField]
    private Rigidbody2D controlledProjectile;


    [SerializeField]
    private GameObject spawnObject;

    private float _cooldown;

    public Rigidbody2D ControlledProjectile { get => controlledProjectile; set => controlledProjectile = value; }

    public static ArcProjectileLauncher Instance { get; private set; }

    private PlayerController playerController;

    private void Awake()
    {
        Instance = this;

        playerController = GetComponent<PlayerController>();
    }

    public Rigidbody2D Fire(Vector3 origin, Vector3 direction)
    {
        var proj = Instantiate(projectile, origin, Quaternion.identity);
        var rigidbodyProj = proj.GetComponent<Rigidbody2D>();
        rigidbodyProj.velocity = expectedVelocity * direction.normalized;
        return rigidbodyProj;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        if (controlledProjectile == null && CurrentInput.GetKeyDownRT && CurrentInput.RightStick != Vector2.zero && _cooldown < 0f && playerController.CurrentState == PlayerController.PlayerStates.NeutralMovement)
        {
            _cooldown = cooldown;
            controlledProjectile = Fire(spawnObject.transform.position, CurrentInput.RightStick);
        }

        if (CurrentInput.GetKeyUpRT)
        {
            if (controlledProjectile != null)
            controlledProjectile.gameObject.GetComponent<LineRenderer>().enabled = false;
            controlledProjectile = null;
        }

        if (controlledProjectile != null)
        {
            if ((controlledProjectile.transform.position - transform.position).sqrMagnitude > maxRange * maxRange)
            {
                controlledProjectile.gameObject.GetComponent<LineRenderer>().enabled = false;
                controlledProjectile = null;
            }
        }
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
}