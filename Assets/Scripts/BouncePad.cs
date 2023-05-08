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
        PlayerController playerController;
        DeflectingBlock deflectingBlock = null;
        if (!(col.gameObject.TryGetComponent(out playerController) || col.gameObject.TryGetComponent(out deflectingBlock))) return;
        if (col.GetContact(0).normal.y > -0.9f) return;
        if (playerController != null)
        {
            if (!canBounce) return;
            playerController.Rigidbody2D.velocity += new Vector2(0, normalBounceForce);
        }
        else
        {
            if (deflectingBlock != null && deflectingBlock.TryGetComponent<Rigidbody2D>(out var outRigidbody))
            {
                outRigidbody.velocity += new Vector2(0, normalBounceForce);
            }
        }
    }
}