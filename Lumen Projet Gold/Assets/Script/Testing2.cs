using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fait par Benjamin
public class Testing2 : MonoBehaviour
{
    private Grid2 grid;
    public int gridLength;
    public int gridHeight;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid2(gridLength, gridHeight,25f, new Vector3(-110, -58, 0)) ;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GameManager.GetMouseWorldPosition(),56);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.canLuoMove = true;
            grid.SetValue(GameManager.GetMouseWorldPosition(), 12);
        }
        if (Input.GetMouseButtonDown(2))
        {
            
        }
        if (GameManager.canLuoMove == true)
        {
            grid.Pathfinder(gridHeight, gridLength);
        }
    }
}
