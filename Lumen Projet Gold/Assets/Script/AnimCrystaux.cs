﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCrystaux : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Animate()
    {
        anim.SetBool("isLit", true);
    }
}
