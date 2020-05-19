using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Fait par Benjamin
public enum directionFaced { UP, DOWN, LEFT, RIGHT, }
public enum crystalType { BASIC, TOWERNORTH, TOWERSOUTH, TOWEREAST, TOWERWEST, BILATERALVERTICAL, BILATERALHORIZONTAL }
public class Grid2
{
    public directionFaced direction;
    public int width;
    private int height;
    private cellContent[,] gridArray;
    private float cellSize;
    private TextMesh[,] debugTextArray;
    public Vector3 originPosition;
    public int numberSize;
    int move = 0;
    void Start()
    {
        direction = directionFaced.UP;
    }
    public struct cellContent
    {
        public bool isIlluminated;
        public int value;
        public bool hasLuo;
        public bool isCrystal;
        public bool isGoal;
        public crystalType typeCrystal;
        public int usageCount;
        public bool isDark;
    }
    public Grid2(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;

        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new cellContent[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                debugTextArray[i, j] = GameManager.CreateWorldText(gridArray[i, j].value.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * 0.5f, 50, Color.white, TextAnchor.MiddleCenter);
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
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (gridArray[x, y].isCrystal == false)
            {
                gridArray[x, y].value = value;
                gridArray[x, y].isIlluminated = true;
                debugTextArray[x, y].text = gridArray[x, y].value.ToString();
            }
            else if (gridArray[x, y].isCrystal == true)
            {
                if (gridArray[x, y].typeCrystal == crystalType.BASIC)
                {
                    gridArray[x, y].value = 101;
                }
                if (gridArray[x, y].typeCrystal == crystalType.BILATERALVERTICAL)
                {
                    gridArray[x, y].value = 201;
                }
                if (gridArray[x, y].typeCrystal == crystalType.BILATERALHORIZONTAL)
                {
                    gridArray[x, y].value = 202;
                }
                if (gridArray[x, y].typeCrystal == crystalType.TOWERNORTH)
                {
                    gridArray[x, y].value = 301;
                }
                if (gridArray[x, y].typeCrystal == crystalType.TOWEREAST)
                {
                    gridArray[x, y].value = 302;
                }
                if (gridArray[x, y].typeCrystal == crystalType.TOWERSOUTH)
                {
                    gridArray[x, y].value = 303;
                }
                if (gridArray[x, y].typeCrystal == crystalType.TOWERWEST)
                {
                    gridArray[x, y].value = 304;
                }
                debugTextArray[x, y].text = gridArray[x, y].value.ToString();
            }
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);

    }

    public void SetCrystal(int x, int y, crystalType crystal)
    {
        //Debug.Log("CrystalSet");
        gridArray[x, y].isCrystal = true;
        gridArray[x, y].typeCrystal = crystal;

        SetValue(x, y, 100);


    }


    public void SetGoal(int x, int y)
    {
        gridArray[x, y].isGoal = true;
        SetValue(x, y, 876);
        gridArray[x, y].isIlluminated = true;
    }
    public void SetDark(int x, int y)
    {
        gridArray[x, y].isDark = true;
        gridArray[x, y].isIlluminated = false;
        SetValue(x, y, 000);
    }

    public void ActivateDark(int x, int y)
    {
        if (x >= 0 && y >= 0)
        {
            if (gridArray[x, y].isIlluminated == true && gridArray[x, y].isCrystal == false && gridArray[x, y].hasLuo == false)
            {
                gridArray[x, y].value = 00;
                gridArray[x, y].isIlluminated = false;
                debugTextArray[x, y].text = gridArray[x, y].value.ToString();
                GameManager.numberOfLights--;
                Debug.Log(GameManager.numberOfLights);
            }
        }
    }

    public void ActivateDark(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (gridArray[x, y].hasLuo == true && GameManager.inDarkMode == false)
        {
            GameManager.inDarkMode = true;
        }
        else if (gridArray[x, y].hasLuo == true && GameManager.inDarkMode == true)
        {
            GameManager.inDarkMode = false;
        }
        ActivateDark(x, y);
    }

    public void UseBasicCrystal(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (gridArray[x, y].usageCount <= 1 && GameManager.intensificationAllowed == true || gridArray[x, y].usageCount == 0 && GameManager.intensificationAllowed == false && gridArray[x, y].isDark == false && gridArray[x, y].isCrystal == true)
        {
            GameManager.numberOfLights--;
            if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.BASIC)
            {
                
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x + 1, y, value);
                    SetValue(x - 1, y, value);
                    SetValue(x, y + 1, value);
                    SetValue(x, y - 1, value);
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x + 2, y, value);
                    SetValue(x - 2, y, value);
                    SetValue(x, y + 2, value);
                    SetValue(x, y - 2, value);
                }

            }
            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWERNORTH)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x, y + 1, value);
                    SetValue(x, y + 2, value);
                    SetValue(x, y + 3, value);
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x, y + 4, value);
                }
            }
            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWERSOUTH)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x, y - 1, value);
                    SetValue(x, y - 2, value);
                    SetValue(x, y - 3, value);
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x, y - 4, value);
                }
            }

            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWERWEST)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x - 1, y, value);
                    SetValue(x - 2, y, value);
                    SetValue(x - 3, y, value);
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x - 4, y, value);
                }
            }

            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWEREAST)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x + 1, y, value);
                    SetValue(x + 2, y, value);
                    SetValue(x + 3, y, value);
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x + 4, y, value);
                }
            }



            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.BILATERALHORIZONTAL)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x + 1, y, value);
                    SetValue(x + 2, y, value);
                    SetValue(x - 1, y, value);
                    SetValue(x - 2, y, value);
                }

                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x + 3, y, value);
                    SetValue(x - 3, y, value);
                }
            }

            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.BILATERALVERTICAL)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x, y + 1, value);
                    SetValue(x, y + 2, value);
                    SetValue(x, y - 1, value);
                    SetValue(x, y - 2, value);
                }

                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x, y + 3, value);
                    SetValue(x, y - 3, value);
                }
            }
            gridArray[x, y].usageCount++;
            GameManager.objectGrabbed = false;
        }

    }

    public void SetLuo(int x, int y, int initialDirection)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y].hasLuo = true;
            if(initialDirection == 1) {
                direction = directionFaced.UP;
                    }
            else if(initialDirection == 2)
            {
                direction = directionFaced.RIGHT;
            }
            else if(initialDirection == 3)
            {
                direction = directionFaced.DOWN;
            }
            else if(initialDirection == 4)
            {
                direction = directionFaced.LEFT;
            }
            SetValue(x, y, 12);
        }
    }





    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y].value;
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

    public void Pathfinder(int gridHeight, int gridLength, Transform playerTransform, bool isWaiting)
    {
        
        bool foundGoal = false;
            for (int j = 0; j < gridHeight; j++)
            {
                for (int i = 0; i < gridLength; i++)
                {
                    if (gridArray[i, j].isGoal == true && gridArray[i,j].hasLuo==true )
                    {
                        Debug.Log("Niveau terminé");
                    }
                    
                    if (gridArray[i, j].hasLuo == true && direction == directionFaced.UP && gridArray[i, j].isGoal == false && foundGoal == false)
                    {
                        if (gridArray[i, j + 1].isIlluminated == true && i < GameManager.width && j < GameManager.height && gridArray[i, j + 1].isDark == false)
                        {
                            //Debug.Log("Haut");
                            direction = directionFaced.UP;
                            gridArray[i, j + 1].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i, j + 1].hasLuo = true;
                            playerTransform.position += new Vector3(0f, 8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j + 1, 12);
                        
                        
                        }

                        else if (gridArray[i + 1, j].isIlluminated == true)
                        {
                            //Debug.Log("Droite");
                            direction = directionFaced.RIGHT;
                            gridArray[i + 1, j].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i + 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i + 1, j, 12);
                        }
                        else if (gridArray[i - 1, j].isIlluminated == true)
                        {
                            //Debug.Log("Gauche");
                            direction = directionFaced.LEFT;
                            gridArray[i - 1, j].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i - 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(-8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i - 1, j, 12);
                        }
                    }

                    if (gridArray[i, j].hasLuo==true && direction == directionFaced.DOWN && gridArray[i, j].isGoal == false && foundGoal == false)
                    {
                    Debug.Log("Luo descend");
                        if (gridArray[i, j - 1].isIlluminated == true && gridArray[i,j-1].isDark == false)
                        {
                        
                            direction = directionFaced.DOWN;
                            gridArray[i, j - 1].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i, j - 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, -8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j - 1, 12);
                        }
                        else if (gridArray[i + 1, j].isIlluminated == true)
                        {
                            Debug.Log("Droite");
                            direction = directionFaced.RIGHT;
                            gridArray[i + 1, j].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i + 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i + 1, j, 12);
                        }
                        else if (gridArray[i - 1, j].isIlluminated == true)
                        {
                            //Debug.Log("Gauche");
                            direction = directionFaced.LEFT;
                            gridArray[i - 1, j].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i - 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(-8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i - 1, j, 12);
                        }
                    }

                    if (gridArray[i, j].hasLuo == true && direction == directionFaced.LEFT && gridArray[i, j].isGoal == false && foundGoal == false)
                    {

                        if (gridArray[i - 1, j].isIlluminated == true && gridArray[i-1, j].isDark == false)
                        {
                            direction = directionFaced.LEFT;
                            gridArray[i - 1, j].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i - 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(-8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i - 1, j, 12);
                        }
                        else if (gridArray[i, j + 1].isIlluminated == true && gridArray[i, j + 1].isDark == false)
                        {
                            //Debug.Log("Haut");
                            direction = directionFaced.UP;
                            gridArray[i, j + 1].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i, j + 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, 8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j + 1, 12);
                        }
                        else if (gridArray[i, j - 1].isIlluminated == true)
                        {
                            //Debug.Log("Bas");
                            direction = directionFaced.DOWN;
                            gridArray[i, j - 1].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i, j - 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, -8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j - 1, 12);
                        }

                    }

                    if (gridArray[i, j].value == 12 && direction == directionFaced.RIGHT && gridArray[i, j].isGoal == false && foundGoal == false)
                    {

                        GameManager.canLuoMove = false;
                        if (gridArray[i + 1, j].isIlluminated == true)
                        {
                            //Debug.Log("Droite");
                            direction = directionFaced.RIGHT;
                            gridArray[i + 1, j].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i + 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i + 1, j, 12);
                        }
                        else if (gridArray[i, j + 1].isIlluminated == true)
                        {
                            //Debug.Log("Haut");
                            direction = directionFaced.UP;
                            gridArray[i, j + 1].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i, j + 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, 8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j + 1, 12);
                        }
                        else if (gridArray[i, j - 1].isIlluminated == true)
                        {
                            //Debug.Log("Bas");
                            direction = directionFaced.DOWN;
                            gridArray[i, j - 1].value = 12;
                            gridArray[i, j].hasLuo = false;
                            SetValue(i, j, 0);
                            gridArray[i, j - 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, -8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j - 1, 12);
                        }

                    }

                }
            

        }
    }

    
    //public void DebugFunction(int i, int j)
    //{
    //    Debug.Log(gridArray[i, j].isCrystal);
    //}

}
