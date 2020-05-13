using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fait par Benjamin
public class Testing2 : MonoBehaviour
{
    private Grid2 grid;
    public int gridLength;

    public int gridHeight;
    public int numberOfCrystals;
    public int[] crystalXArray = new int[1];
    public int[] crystalYArray = new int[1];
    
    public int luoXPosition;
    public int luoYPosition;
    public int goalXPosition, goalYPosition;
    public int numberOfLights;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid2(gridLength, gridHeight,25f, new Vector3(-110, -58, 0)) ;
        grid.SetLuo(luoXPosition, luoYPosition);
        grid.SetGoal(goalXPosition, goalYPosition);
        for (int i = 0; i < numberOfCrystals; i++)
        {
            grid.SetCrystal(crystalXArray[i], crystalYArray[i]);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0))
        {
            if (numberOfLights > 0) {
            grid.UseBasicCrystal(GameManager.GetMouseWorldPosition(),56);
                numberOfLights--;
            }
            
        }

        if (numberOfLights <=0)
        {
            grid.Pathfinder(gridHeight, gridLength);
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    GameManager.canLuoMove = true;
        //    grid.SetValue(GameManager.GetMouseWorldPosition(), 12);
        //}
        //if (Input.GetMouseButtonDown(2))
        //{

        //}

    }
}
