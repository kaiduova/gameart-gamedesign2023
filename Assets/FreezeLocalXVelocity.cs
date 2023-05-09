using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeLocalXVelocity : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null && TryGetComponent<Rigidbody2D>(out var rigidbody2D))
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }
}
