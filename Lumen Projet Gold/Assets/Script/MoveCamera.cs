using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public int max;
    private int chapterid = 1;
   

    public void Left ()
    {
        if ( chapterid > 1)
        {
            movePoint.position += new Vector3(-4.15f, 0f, 0f);
            chapterid--;
        }
       
    }

    public void right()
    {
        if (chapterid < max)
        {
            movePoint.position += new Vector3(4.15f, 0f, 0f);
            chapterid++;
        }
        
    }
    //void Start()
    //{
    //    movePoint.parent = null;
    //}

    // Update is called once per frame
    void Update()
    {
        
  
    }

}

