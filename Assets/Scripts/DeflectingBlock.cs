using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class DeflectingBlock : MonoBehaviour
{
    private void Start()
    {
        gameObject.layer = 13;
    }
}