using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform pointPosition;
    public int speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( transform.position != pointPosition.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointPosition.position, speed * Time.deltaTime);

        }

    }
}
