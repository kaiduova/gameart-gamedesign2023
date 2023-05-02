using System;
using Input;
using UnityEngine;

public class ArcProjectileLauncher : InputMonoBehaviour
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float expectedVelocity, turnRate;

    [SerializeField]
    private Rigidbody2D controlledProjectile;


    [SerializeField]
    private GameObject spawnObject;

    public Rigidbody2D ControlledProjectile { get => controlledProjectile; set => controlledProjectile = value; }

    public static ArcProjectileLauncher Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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
        if (controlledProjectile == null && CurrentInput.GetKeyDownRT && CurrentInput.RightStick != Vector2.zero)
        {
            controlledProjectile = Fire(spawnObject.transform.position, CurrentInput.RightStick);
        }

        if (CurrentInput.GetKeyUpRT)
        {
            if (controlledProjectile != null)
            controlledProjectile.gameObject.GetComponent<LineRenderer>().enabled = false;
            controlledProjectile = null;
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