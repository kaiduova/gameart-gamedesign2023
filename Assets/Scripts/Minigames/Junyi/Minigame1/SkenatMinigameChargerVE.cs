using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkenatMinigameChargerVE : MonoBehaviour
{
    Color tempColor;
    void Start()
    {
        tempColor = this.GetComponent<SpriteRenderer>().color;

    }

    void Update()
    {
        this.transform.localScale += new Vector3(0.09f, 0.09f, 0);
        tempColor.a -= 0.04f;
        this.GetComponent<SpriteRenderer>().color = tempColor;
        if (tempColor.a < 0.1f)
        {
            Destroy(this.gameObject);
        }
    }
}
