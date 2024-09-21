using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryProj : MonoBehaviour
{
    private float OutOfBounds = -0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < OutOfBounds)
        {
            Destroy(gameObject);
        }
    }
}
