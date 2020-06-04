using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCrystaux : MonoBehaviour
{
    public Animator anim;
    int compteur = 0;
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
        Debug.Log("Allume");
        if(compteur==1 && GameManager.intensificationAllowed == true) { 
        anim.SetBool("isLit", true);
        }
        compteur++;
    }
}
