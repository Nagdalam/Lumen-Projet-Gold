using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class LumenLight : MonoBehaviour
{
    public Light2D myLight;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.numberOfLights >=5) {
            myLight.intensity = 1.4f;
        }
        else if (GameManager.numberOfLights == 4)
        {
            myLight.intensity = 1.2f;
        }
        else if (GameManager.numberOfLights == 3)
        {
            myLight.intensity = 1f;
        }
        else if (GameManager.numberOfLights == 2)
        {
            myLight.intensity = 0.8f;
        }
        else if (GameManager.numberOfLights == 1)
        {
            myLight.intensity = 0.6f;
        }
        else if (GameManager.numberOfLights <= 0)
        {
            myLight.intensity = 0.4f;
        }
    }
}
