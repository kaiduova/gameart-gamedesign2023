using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BouncePad : MonoBehaviour
{
    private BoxCollider2D _collider;
    
    [SerializeField]
    private float normalBounceForce, additionalJumpBounceForce;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out var playerController)) return;
        playerController.Rigidbody2D.velocity += new Vector2(0, normalBounceForce);
        if (playerController.JumpBufferDuration > 0 && playerController.CoyoteTimeDuration > 0)
        {
            playerController.Rigidbody2D.velocity += new Vector2(0, additionalJumpBounceForce);
        }
    }
}