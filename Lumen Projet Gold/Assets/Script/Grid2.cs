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
    private int width;
    private int height;
    private cellContent[,] gridArray;
    private float cellSize;
    private TextMesh[,] debugTextArray;
    private Vector3 originPosition;

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
                debugTextArray[i, j] = GameManager.CreateWorldText(gridArray[i, j].value.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * 0.5f, 100, Color.white, TextAnchor.MiddleCenter);
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
                gridArray[x, y].value = 555;
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
        Debug.Log("CrystalSet");
        gridArray[x, y].isCrystal = true;
        gridArray[x, y].typeCrystal = crystal;
        SetValue(x, y, 555);
    }


    public void SetGoal(int x, int y)
    {
        gridArray[x, y].isGoal = true;
        SetValue(x, y, 876);
    }
    public void SetDark(int x, int y)
    {
        gridArray[x, y].isDark = true;
        SetValue(x, y, 000);
    }

    public void ActivateDark(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (gridArray[x, y].isIlluminated == true && gridArray[x, y].isCrystal == false)
            {
                gridArray[x, y].value = 00;
                gridArray[x, y].isIlluminated = false;
                debugTextArray[x, y].text = gridArray[x, y].value.ToString();
                GameManager.numberOfLights--;
            }
        }
    }

    public void ActivateDark(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        ActivateDark(x, y);
    }

    public void UseBasicCrystal(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (gridArray[x, y].usageCount <= 1 && GameManager.intensificationAllowed == true || gridArray[x, y].usageCount == 0 && GameManager.intensificationAllowed == false && gridArray[x, y].isDark == false && gridArray[x,y].isCrystal==true)
        {
            if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.BASIC)
            {
                GameManager.numberOfLights--;
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
                    SetValue(x - 1,y , value);
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
                    SetValue(x + 1,y , value);
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
                    SetValue(x, y +1, value);
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

    public void SetLuo(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y].hasLuo = true;
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

    public void Pathfinder(int gridHeight, int gridLength)
    {
        for (int j = 0; j < gridHeight; j++)
        {
            for (int i = 0; i < gridLength; i++)
            {
                if (gridArray[i, j].isGoal == true)
                {
                    Debug.Log("Niveau terminé");
                }
                if (gridArray[i, j].value == 12 && direction == directionFaced.UP && gridArray[i, j].isGoal == false)
                {
                    if (gridArray[i, j + 1].isIlluminated == true/* && j + 1 < gridHeight && i < gridLength && j > 0 && i > 0*/)
                    {
                        Debug.Log("Haut");
                        direction = directionFaced.UP;
                        gridArray[i, j + 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j + 1].hasLuo = true;

                        SetValue(i, j + 1, 12);
                    }
                    //else if(gridArray[i, j - 1].value == 56 && j < gridHeight && i < gridLength && j - 1 >0 && i >0)
                    //{
                    //    Debug.Log("Bas");
                    //    gridArray[i, j - 1].value = 12;
                    //    gridArray[i, j].hasLuo = false;
                    //    SetValue(i, j, 0);
                    //    gridArray[i, j - 1].hasLuo = true;

                    //    SetValue(i, j - 1, 12);
                    //}
                    else if (gridArray[i + 1, j].isIlluminated == true/* && j < gridHeight && i + 1 < gridLength && j > 0 && i > 0*/)
                    {
                        Debug.Log("Droite");
                        direction = directionFaced.RIGHT;
                        gridArray[i + 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i + 1, j].hasLuo = true;

                        SetValue(i + 1, j, 12);
                    }
                    else if (gridArray[i - 1, j].isIlluminated == true/* && j < gridHeight && i < gridLength && j > 0 && i - 1 > 0*/)
                    {
                        Debug.Log("Gauche");
                        direction = directionFaced.LEFT;
                        gridArray[i - 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i - 1, j].hasLuo = true;

                        SetValue(i - 1, j, 12);
                    }
                }

                if (gridArray[i, j].value == 12 && direction == directionFaced.DOWN && gridArray[i, j].isGoal == false)
                {
                    //    Debug.Log("Luo est aux coordonnées" + j + "," + i);
                    //    GameManager.canLuoMove = false;
                    //    if (gridArray[i, j + 1].value == 56 && j + 1 < gridHeight && i < gridLength && j > 0 && i > 0)
                    //    {
                    //        Debug.Log("Haut");
                    //        gridArray[i, j + 1].value = 12;
                    //        gridArray[i, j].hasLuo = false;
                    //        SetValue(i, j, 0);
                    //        gridArray[i, j + 1].hasLuo = true;

                    //        SetValue(i, j + 1, 12);
                    //    }
                    if (gridArray[i, j - 1].isIlluminated == true/* && j < gridHeight && i < gridLength && j - 1 > 0 && i > 0*/)
                    {
                        Debug.Log("Bas");
                        direction = directionFaced.DOWN;
                        gridArray[i, j - 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j - 1].hasLuo = true;

                        SetValue(i, j - 1, 12);
                    }
                    else if (gridArray[i + 1, j].isIlluminated == true /*&& j < gridHeight && i + 1 < gridLength && j > 0 && i > 0*/)
                    {
                        Debug.Log("Droite");
                        direction = directionFaced.RIGHT;
                        gridArray[i + 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i + 1, j].hasLuo = true;

                        SetValue(i + 1, j, 12);
                    }
                    else if (gridArray[i - 1, j].isIlluminated == true /*&& j < gridHeight && i < gridLength && j > 0 && i - 1 > 0*/)
                    {
                        Debug.Log("Gauche");
                        direction = directionFaced.LEFT;
                        gridArray[i - 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i - 1, j].hasLuo = true;

                        SetValue(i - 1, j, 12);
                    }
                }

                if (gridArray[i, j].value == 12 && direction == directionFaced.LEFT && gridArray[i, j].isGoal == false)
                {

                    Debug.Log("Luo est aux coordonnées" + j + "," + i);
                    GameManager.canLuoMove = false;

                    if (gridArray[i - 1, j].isIlluminated == true/* && j < gridHeight && i < gridLength && j > 0 && i - 1 > 0*/)
                    {
                        Debug.Log("Gauche");
                        direction = directionFaced.LEFT;
                        gridArray[i - 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i - 1, j].hasLuo = true;

                        SetValue(i - 1, j, 12);
                    }
                    else if (gridArray[i, j + 1].isIlluminated == true/* && j + 1 < gridHeight && i < gridLength && j > 0 && i > 0*/)
                    {
                        Debug.Log("Haut");
                        direction = directionFaced.UP;
                        gridArray[i, j + 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j + 1].hasLuo = true;

                        SetValue(i, j + 1, 12);
                    }
                    else if (gridArray[i, j - 1].isIlluminated == true /*&& j < gridHeight && i < gridLength && j - 1 > 0 && i > 0*/)
                    {
                        Debug.Log("Bas");
                        direction = directionFaced.DOWN;
                        gridArray[i, j - 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j - 1].hasLuo = true;

                        SetValue(i, j - 1, 12);
                    }
                    //else if (gridArray[i + 1, j].value == 56 && j < gridHeight && i + 1 < gridLength && j > 0 && i > 0)
                    //{
                    //    Debug.Log("Droite");
                    //    gridArray[i + 1, j].value = 12;
                    //    gridArray[i, j].hasLuo = false;
                    //    SetValue(i, j, 0);
                    //    gridArray[i + 1, j].hasLuo = true;

                    //    SetValue(i + 1, j, 12);
                    //}

                }

                if (gridArray[i, j].value == 12 && direction == directionFaced.RIGHT && gridArray[i, j].isGoal == false)
                {

                    Debug.Log("Luo est aux coordonnées" + j + "," + i);
                    GameManager.canLuoMove = false;
                    if (gridArray[i + 1, j].isIlluminated == true/* && j < gridHeight && i + 1 < gridLength && j > 0 && i > 0*/)
                    {
                        Debug.Log("Droite");
                        direction = directionFaced.RIGHT;
                        gridArray[i + 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i + 1, j].hasLuo = true;

                        SetValue(i + 1, j, 12);
                    }
                    else if (gridArray[i, j + 1].isIlluminated == true/* && j + 1 < gridHeight && i < gridLength && j > 0 && i > 0*/)
                    {
                        Debug.Log("Haut");
                        direction = directionFaced.UP;
                        gridArray[i, j + 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j + 1].hasLuo = true;

                        SetValue(i, j + 1, 12);
                    }
                    else if (gridArray[i, j - 1].isIlluminated == true /*&& j < gridHeight && i < gridLength && j - 1 > 0 && i > 0*/)
                    {
                        Debug.Log("Bas");
                        direction = directionFaced.DOWN;
                        gridArray[i, j - 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j - 1].hasLuo = true;

                        SetValue(i, j - 1, 12);
                    }

                    //else if (gridArray[i - 1, j].value == 56 && j < gridHeight && i < gridLength && j > 0 && i - 1 > 0)
                    //{
                    //    Debug.Log("Gauche");
                    //    gridArray[i - 1, j].value = 12;
                    //    gridArray[i, j].hasLuo = false;
                    //    SetValue(i, j, 0);
                    //    gridArray[i - 1, j].hasLuo = true;

                    //    SetValue(i - 1, j, 12);
                    //}
                }

            }
        }
    }


}
