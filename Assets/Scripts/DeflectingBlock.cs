using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class DeflectingBlock : MonoBehaviour
{

    [HideInInspector] public Rigidbody2D Rigidbody2D;

    private void Awake() {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        gameObject.layer = 13;
    }
}