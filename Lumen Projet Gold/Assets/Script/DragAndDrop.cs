using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private bool selected;
    SpriteRenderer m_SpriteRenderer;
    Color m_NewColor;
    public GraphicRaycaster grphRaycast;
    PointerEventData ptData = new PointerEventData(null);
    Vector2 originPosition;
    public Sprite regularLight, darkLight;
    public Image image;
    public Transform target;
    public Animator myAnim;
    public Transform luoTarget;
    private void Start()
    {
        originPosition = transform.position;
        ptData = new PointerEventData(EventSystem.current);
    }
    void Update()
    {
        

        if (GameManager.numberOfLights <= 0)
        {
            Destroy(gameObject);
        }
       
        if (selected == true)
        {
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
        if(GameManager.inDarkMode == true)
        {
            myAnim.SetBool("isDark", true);
        }
        if (GameManager.inDarkMode == false)
        {
            myAnim.SetBool("isDark", false);
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
        if (GameManager.numberOfLights > 0)
        {
            GameManager.canSpawn = true;
        }
        Destroy(gameObject);

    }

    public void OnDrag(PointerEventData eventData)
    {
        GameManager.objectGrabbed = true;
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.isDropped = true;
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (hit.collider != null && hit.transform.tag == "Crystal")
        {
            hit.collider.gameObject.GetComponent<AnimCrystaux>().Animate();
        }
        transform.position = target.position;
        luoTarget.position = GameManager.GetMouseWorldPosition();

    }
}
