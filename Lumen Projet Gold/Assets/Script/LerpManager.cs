using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpManager : MonoBehaviour
{
    public float timeStartedLerping, lerpTime;
    public static Vector2 endPosition;
    public Vector2 startPosition;
    bool shouldLerp;
    public static bool startLerping;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        startLerping = false;
        shouldLerp = false;
        endPosition = startPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if(startLerping == true)
        {
            Debug.Log("Prout");
            timeStartedLerping = Time.time;
            shouldLerp = true;
        }
        if(startLerping == false)
        {
            shouldLerp = false;
        }
        if (shouldLerp)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, 9 * Time.deltaTime);
        }

    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        
        float timeSinceStarted = Time.time - (timeStartedLerping);

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;
    }

    IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(1);
        
        
    }

}
