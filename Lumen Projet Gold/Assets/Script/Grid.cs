using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fait par Benjamin
public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    private TextMesh[,] debugTextArray;
    private bool isBilateral = true;
    private bool isBasic = false;
    private bool isTower = false;
    

    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height] ;

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                debugTextArray[i,j] = GameManager.CreateWorldText(gridArray[i, j].ToString(), null, GetWorldPosition(i,j) + new Vector3(cellSize, cellSize)*0.5f ,20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        SetValue(2, 1, 0);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }
    
    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >=0 && x<width && y < height) { 
        gridArray[x, y] = value;
        debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        //start crysto basic
        if (isBasic == true)
        {
            SetValue(x+1, y, value);
            SetValue(x-1, y, value);
            SetValue(x , y+1, value);
            SetValue(x , y-1, value);
        }
        //end crysto basic

        //tower crytal
        if (isTower == true)
        {
            SetValue(x , y + 1, value);
            SetValue(x , y + 2, value);
            SetValue(x , y + 3, value);
        }
        //end tower crytal
        //bilateral crytal X
        if (isBilateral == true)
        {
            SetValue(x + 1, y, value);
            SetValue(x + 2, y, value);
            SetValue(x - 1, y, value);
            SetValue(x - 2, y, value);
        }
      




    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }


}
