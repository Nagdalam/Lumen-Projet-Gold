using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool selected;

    void Update()
    {
        if (selected == true)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x, cursorPos.y);
            if(GameManager.objectGrabbed == false)
            {
                Destroy(gameObject);
            }
        }

        if (Input.GetMouseButtonUp(0) && selected == true)
        {
            selected = false;
            GameManager.isDropped = true;
            StartCoroutine(WaitASecond());
            
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selected = true;
            GameManager.objectGrabbed = true;
        }
    }

    IEnumerator WaitASecond()
    {
        
        yield return new WaitForSeconds(.1f);
        GameManager.isDropped = false;
        Destroy(gameObject);

    }

}
