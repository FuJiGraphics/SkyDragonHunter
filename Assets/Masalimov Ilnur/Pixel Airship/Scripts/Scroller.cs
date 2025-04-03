using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float speed;
    Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Time.time * -speed;
        float X = Mathf.Repeat(offset, 70);
        transform.position = startPos + new Vector3(X, 0, 0);
    }
}
