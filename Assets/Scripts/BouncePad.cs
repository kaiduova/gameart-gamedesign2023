using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BouncePad : MonoBehaviour
{

    [SerializeField]
    private float normalBounceForce;

    [HideInInspector]
    public bool canBounce;
    
    private void OnCollisionStay2D(Collision2D col)
    {
        if (!col.gameObject.TryGetComponent<PlayerController>(out var playerController) || col.gameObject.TryGetComponent<DeflectingBlock>(out var deflectingBlock)) return;
        if (col.GetContact(0).normal.y > -0.9f) return;
        if (!canBounce) return;
        playerController.Rigidbody2D.velocity += new Vector2(0, normalBounceForce);
    }
}