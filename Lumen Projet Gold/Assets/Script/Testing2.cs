using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fait par Benjamin
public class Testing2 : MonoBehaviour
{
    private Grid2 grid;
    public bool intensificationAllowed;
    public bool isDarkTilesAllowed;
    public int gridLength;
    public int gridHeight;
    public int numberOfCrystals;
    public int[] crystalXArray = new int[1];
    public int[] crystalYArray = new int[1];
    public int numberOfDarksTiles;
    public int[] darkTileXArray = new int[1];
    public int[] darkTileYArray = new int[1];
    public crystalType[] crystalTypeArray = new crystalType[1];
    public int luoXPosition;
    public int luoYPosition;
    public int goalXPosition, goalYPosition;
    public int numberOfLights;
    public float cellSize = 32f;
    public int originX = -87;
    public int originY = -75;
    public int numberSize = 100;
    
    // Start is called before the first frame update
    void Start()
    {

        grid = new Grid2(gridLength, gridHeight, cellSize, new Vector3(originX, originY, 0)) ;
        grid.numberSize = numberSize;
        grid.originPosition = new Vector3(originX, originY, 0);
        grid.SetLuo(luoXPosition, luoYPosition);
        grid.SetGoal(goalXPosition, goalYPosition);
        for (int i = 0; i < numberOfCrystals; i++)
        {
            grid.SetCrystal(crystalXArray[i], crystalYArray[i], crystalTypeArray[i]);
        }
        
        for (int i = 0; i < numberOfDarksTiles; i++)
        {
            grid.SetDark(darkTileXArray[i], darkTileYArray[i]);
        }
        GameManager.numberOfLights = numberOfLights;
        Debug.Log(GameManager.numberOfLights);
        GameManager.intensificationAllowed = intensificationAllowed;
        GameManager.isDarkTilesAllowed = isDarkTilesAllowed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameManager.numberOfLights);

        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.numberOfLights > 0) {
                grid.UseBasicCrystal(GameManager.GetMouseWorldPosition(), 56);
                grid.ActivateDark(GameManager.GetMouseWorldPosition());
            }
            
        }

        if (GameManager.objectGrabbed == true)
        {
            if (GameManager.numberOfLights > 0)
            {
                grid.UseBasicCrystal(GameManager.GetMouseWorldPosition(), 56);
            }

        }

        if (GameManager.numberOfLights <=0)
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
