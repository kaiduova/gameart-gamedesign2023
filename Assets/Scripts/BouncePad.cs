using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BouncePad : MonoBehaviour
{

    [SerializeField]
    private float normalBounceForce;

    [HideInInspector]
    public bool canBounce;

    private EatingEnemy _controller;

    private void Awake()
    {
        _controller = GetComponent<EatingEnemy>();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        PlayerController playerController;
        DeflectingBlock deflectingBlock = null;
        if (!(col.gameObject.TryGetComponent(out playerController) || col.gameObject.TryGetComponent(out deflectingBlock))) return;
        if (col.GetContact(0).normal.y > -0.9f) return;
        if (!canBounce) return;
        if (playerController != null)
        {
            playerController.Rigidbody2D.velocity = new Vector2(0, normalBounceForce);
           // _controller.EatingEnemyAnim.SetTrigger("Bounced");
        }
        else
        {
            if (deflectingBlock != null && deflectingBlock.TryGetComponent<Rigidbody2D>(out var outRigidbody))
            {
                _controller.ReviveTimer = _controller.ReviveTime;
                outRigidbody.velocity = new Vector2(0, normalBounceForce);
                //_controller.EatingEnemyAnim.SetTrigger("Bounced");
            }
        }
        _controller.EatingEnemyAnim.SetTrigger("Bounced");
    }
}