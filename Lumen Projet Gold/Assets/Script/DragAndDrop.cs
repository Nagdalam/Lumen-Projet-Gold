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
    private void Start()
    {
        originPosition = transform.position;
        ptData = new PointerEventData(EventSystem.current);
        //m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (GameManager.numberOfLights <= 0)
        {
            Destroy(gameObject);
        }
        Vector3 cursorPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        cursorPos.z = 0;
        RaycastHit2D hit2D = Physics2D.Raycast(cursorPos, Vector3.back);
        if(hit2D.collider != null)
        {
            Debug.Log(hit2D.collider.name);
        }
        //Debug.Log(EventSystem.current.currentSelectedGameObject);
        Debug.Log(ptData.pointerEnter);
        if (selected == true)
        {
            
            //transform.position = new Vector2(cursorPos.x, cursorPos.y);
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
            
            image.sprite = darkLight;
        }
        if (GameManager.inDarkMode == false)
        {
            image.sprite = regularLight;
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
        //selected = true;
        //GameManager.objectGrabbed = true;
        GameManager.objectGrabbed = true;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.isDropped = true;
        
        transform.localPosition = target.localPosition;


        //if (selected == true)
        //{
        //    selected = false;
        //    GameManager.isDropped = true;
        //    StartCoroutine(WaitASecond());
        //}
    }
}
