using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class LumenLight : MonoBehaviour
{
    public Light2D myLight;
    public Transform target;
    public Transform luo;
    public Animator lumenAnim;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, target.position, 9 * Time.deltaTime);
        if (GameManager.numberOfLights >=5) {
            myLight.intensity = 1.4f;
            transform.position = Vector2.MoveTowards(transform.position, target.position, 9 * Time.deltaTime);
        }
        else if (GameManager.numberOfLights == 4)
        {
            myLight.intensity = 1.2f;
            transform.position = Vector2.MoveTowards(transform.position, target.position, 9 * Time.deltaTime);
        }
        else if (GameManager.numberOfLights == 3)
        {
            myLight.intensity = 1f;
            transform.position = Vector2.MoveTowards(transform.position, target.position, 9 * Time.deltaTime);
        }
        else if (GameManager.numberOfLights == 2)
        {
            myLight.intensity = 0.8f;
            transform.position = Vector2.MoveTowards(transform.position, target.position, 9 * Time.deltaTime);
        }
        else if (GameManager.numberOfLights == 1)
        {
            myLight.intensity = 0.6f;
            transform.position = Vector2.MoveTowards(transform.position, target.position, 9 * Time.deltaTime);
        }
        else if (GameManager.numberOfLights <= 0)
        {
            myLight.intensity = 0.4f;
            transform.position = Vector2.MoveTowards(transform.position, luo.position, 9 * Time.deltaTime);
        }
    }
}
