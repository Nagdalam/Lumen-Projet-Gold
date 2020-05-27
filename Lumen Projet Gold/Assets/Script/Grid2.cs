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
    public float cellSize = 8.9f;
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
                debugTextArray[i, j] = GameManager.CreateWorldText(gridArray[i, j].value.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * 0.5f, 25, Color.grey, TextAnchor.MiddleCenter);
                //Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

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
            if (gridArray[x, y].isCrystal == false && gridArray[x,y].isDark == false)
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
            else if (gridArray[x, y].isDark == true)
            {
                gridArray[x, y].value = 888;
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
        SetValue(x, y, 56);
        gridArray[x, y].isIlluminated = true;
    }
    public void SetDark(int x, int y)
    {
        gridArray[x, y].isDark = true;
        gridArray[x, y].isIlluminated = false;
        SetValue(x, y, 000);
    }

    public void ActivateDark(int x, int y, GameObject darkTile, int originX, int originY, AudioSource audioSource)
    {
        if (x >= 0 && y >= 0)
        {
            
            if (gridArray[x, y].isIlluminated == true && gridArray[x, y].isCrystal == false && gridArray[x, y].hasLuo == false && GameManager.inDarkMode == true)
            {
                gridArray[x, y].value = 00;
                gridArray[x, y].isIlluminated = false;
                debugTextArray[x, y].text = gridArray[x, y].value.ToString();
                GameManager.numberOfLights--;
                
                GameObject instancedObj1 = GameObject.Instantiate(darkTile, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;

            }
        }
    }

    public void ActivateDark(Vector3 worldPosition, GameObject darkTile, int originX, int originY, AudioSource audioSource)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        
        ActivateDark(x, y, darkTile, originX, originY, audioSource);
    }

    public void SwitchMode(Vector3 worldPosition, GameObject darkTile, int originX, int originY)
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
    }

    public void UseBasicCrystal(Vector3 worldPosition, int value, GameObject lightPrefabBasic, GameObject lightPrefabTower,GameObject lightPrefabBilateral ,int originX, int originY, AudioSource audioSource)
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
                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    GameObject instancedObj = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + (x + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    SetValue(x - 1, y, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y + 1, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y - 1, value);
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 1) * cellSize), -2), Quaternion.identity) as GameObject;
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    Debug.Log(gridArray[x, y].usageCount);
                    SetValue(x + 2, y, value);
                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 2) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x - 2, y, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 2) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y + 2, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 2) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y - 2, value);
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabBasic, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 2) * cellSize), -2), Quaternion.identity) as GameObject;
                }

            }
            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWERNORTH)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    SetValue(x, y + 1, value);
                    GameObject instancedObj = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + (x + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;

                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y + 2, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 2) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y + 3, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 3) * cellSize), -2), Quaternion.identity) as GameObject;

                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x, y + 4, value);
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 4) * cellSize), -2), Quaternion.identity) as GameObject;

                }
            }
            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWERSOUTH)
            {
                if (gridArray[x, y].usageCount == 0)
                {

                    GameObject instancedObj = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + (x + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;

                    SetValue(x, y - 1, value);
                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y - 2, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 2) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y - 3, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 3) * cellSize), -2), Quaternion.identity) as GameObject;
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x, y - 4, value);
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 4) * cellSize), -2), Quaternion.identity) as GameObject;
                    Debug.Log(gridArray[x, y].usageCount);
                }
            }

            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWERWEST)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    GameObject instancedObj = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + (x + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;

                    SetValue(x - 1, y, value);
                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x - 2, y, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 2) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x - 3, y, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 3) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 4) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x - 4, y, value);
                }
            }

            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.TOWEREAST)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    GameObject instancedObj = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + (x + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;

                    SetValue(x + 1, y, value);
                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    SetValue(x + 2, y, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 2) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    SetValue(x + 3, y, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 3) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                }
                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x + 4, y, value);
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabTower, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 4) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                }
            }



            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.BILATERALHORIZONTAL)
            {
                
                if (gridArray[x, y].usageCount == 0)
                {
                    GameObject instancedObj = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + (x + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;

                    SetValue(x + 1, y, value);
                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    SetValue(x + 2, y, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 2) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    SetValue(x - 1, y, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    SetValue(x - 2, y, value);
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 2) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                }

                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x + 3, y, value);
                    GameObject instancedObj5 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1) + 3) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                    SetValue(x - 3, y, value);
                    GameObject instancedObj6 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1) - 3) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;
                }
            }

            else if (gridArray[x, y].isCrystal == true && gridArray[x, y].typeCrystal == crystalType.BILATERALVERTICAL)
            {
                if (gridArray[x, y].usageCount == 0)
                {
                    GameObject instancedObj = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + (x + 1) * cellSize, ((originY - (cellSize / 2)) + (y + 1) * cellSize), 0), Quaternion.identity) as GameObject;

                    SetValue(x, y + 1, value);
                    GameObject instancedObj1 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y + 2, value);
                    GameObject instancedObj2 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 2) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y - 1, value);
                    GameObject instancedObj3 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 1) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y - 2, value);
                    GameObject instancedObj4 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 2) * cellSize), -2), Quaternion.identity) as GameObject;
                }

                else if (gridArray[x, y].usageCount == 1 && GameManager.intensificationAllowed == true)
                {
                    SetValue(x, y + 3, value);
                    GameObject instancedObj5 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) + 3) * cellSize), -2), Quaternion.identity) as GameObject;
                    SetValue(x, y - 3, value);
                    GameObject instancedObj6 = GameObject.Instantiate(lightPrefabBilateral, new Vector3((originX - (cellSize / 2)) + ((x + 1)) * cellSize, ((originY - (cellSize / 2)) + ((y + 1) - 3) * cellSize), -2), Quaternion.identity) as GameObject;
                }
            }
            
            GameManager.objectGrabbed = false;
            gridArray[x, y].usageCount++;
        }
        


    }

    public void SetLuo(int x, int y, int initialDirection, Animator luoAnim, Animator lumenAnim)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y].hasLuo = true;
            gridArray[x, y].isIlluminated = true;
            if (initialDirection == 1)
            {
                luoAnim.SetBool("idleUp", true);
                lumenAnim.SetBool("faceUp", true);
                direction = directionFaced.UP;
            }
            else if (initialDirection == 2)
            {
                luoAnim.SetBool("idleRight", true);
                lumenAnim.SetBool("faceRight", true);
                direction = directionFaced.RIGHT;
            }
            else if (initialDirection == 3)
            {
                luoAnim.SetBool("idleDown", true);
                lumenAnim.SetBool("faceDown", true);
                direction = directionFaced.DOWN;
            }
            else if (initialDirection == 4)
            {
                luoAnim.SetBool("idleLeft", true);
                lumenAnim.SetBool("faceLeft", true);
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

    public void Pathfinder(int gridHeight, int gridLength, Transform playerTransform, bool isWaiting, int lvlID, Animator luoAnim, AudioSource audioSource, Animator lumenAnim, GameObject victoryUI, GameObject defeatUI, GameObject inGameUI)
    {
        luoAnim.SetBool("idleLeft", false);
        luoAnim.SetBool("idleRight", false);
        luoAnim.SetBool("idleUp", false);
        luoAnim.SetBool("idleDown", false);
        bool foundGoal = false;
        for (int j = 0; j < gridHeight; j++)
        {
            for (int i = 0; i < gridLength; i++)
            {
                if (gridArray[i, j].isGoal == true && gridArray[i, j].hasLuo == true)
                {
                    Debug.Log("Niveau terminé");
                    PlayerPrefs.SetInt("LevelsAvailable", lvlID);
                    
                    inGameUI.SetActive(false);
                    victoryUI.SetActive(true);
                    //SceneManager.LoadScene(lvlID + 1);
                }

                if (gridArray[i, j].hasLuo == true && direction == directionFaced.UP && gridArray[i, j].isGoal == false && foundGoal == false)
                {
                    if (j!= height && gridArray[i, j + 1].isIlluminated == true && i < GameManager.width && j < GameManager.height && gridArray[i, j + 1].isDark == false && gridArray[i,j+1].value==56)
                    {
                        //Debug.Log("Haut");
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceUp", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", true);
                        LerpManager.startLerping = true;
                        direction = directionFaced.UP;
                        gridArray[i, j + 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j + 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, 8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j + 1, 12);



                    }

                    else if (i!= width && gridArray[i + 1, j].isIlluminated == true && gridArray[i + 1, j].isDark == false)
                    {
                        //Debug.Log("Droite");
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceRight", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", true);
                        lumenAnim.SetBool("faceUp", false);
                        LerpManager.startLerping = true;
                        direction = directionFaced.RIGHT;
                        gridArray[i + 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i + 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i + 1, j, 12);
                    }
                    else if (i!=0 && gridArray[i - 1, j].isIlluminated == true && gridArray[i - 1, j].isDark == false && gridArray[i - 1, j].isCrystal == false)
                    {
                        LerpManager.startLerping = true;
                        //Debug.Log("Gauche");
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceLeft", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", true);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.LEFT;
                        gridArray[i - 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i - 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(-8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i - 1, j, 12);
                    }
                    else
                    {
                        luoAnim.SetBool("idleActivated", true);
                        inGameUI.SetActive(false);
                        defeatUI.SetActive(true);
                    }
                }

                if (gridArray[i, j].hasLuo == true && direction == directionFaced.DOWN && gridArray[i, j].isGoal == false && foundGoal == false)
                {
                    if (j!=0 && gridArray[i, j - 1].isIlluminated == true && gridArray[i, j - 1].isDark == false)
                    {
                        LerpManager.startLerping = true;
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceDown", true);
                        lumenAnim.SetBool("faceDown", true);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.DOWN;
                        gridArray[i, j - 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j - 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, -8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j - 1, 12);
                        
                    }

                    

                    else if (i!= width &&gridArray[i + 1, j].isIlluminated == true && gridArray[i + 1, j].isDark == false)
                    {
                        LerpManager.startLerping = true;
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceRight", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", true);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.RIGHT;
                        gridArray[i + 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i + 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i + 1, j, 12);
                    }
                    else if (i!=0 && gridArray[i - 1, j].isIlluminated == true && gridArray[i - 1, j].isDark == false && gridArray[i - 1, j].isCrystal == false)
                    {
                        LerpManager.startLerping = true;
                        //Debug.Log("Gauche");
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceLeft", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", true);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.LEFT;
                        gridArray[i - 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i - 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(-8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i - 1, j, 12);
                    }
                    else
                    {
                        luoAnim.SetBool("idleActivated", true);
                        inGameUI.SetActive(false);
                        defeatUI.SetActive(true);
                    }
                }

                if (gridArray[i, j].hasLuo == true && direction == directionFaced.LEFT && gridArray[i, j].isGoal == false && foundGoal == false)
                {

                    if (i != 0 && gridArray[i - 1, j].isIlluminated == true && gridArray[i - 1, j].value==56 && gridArray[i - 1, j].isDark == false && gridArray[i - 1, j].isCrystal == false)
                    {
                        LerpManager.startLerping = true;
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceLeft", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", true);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.LEFT;
                        gridArray[i - 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i - 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(-8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i - 1, j, 12);
                    }
                    else if (j!= height && gridArray[i, j + 1].isIlluminated == true && gridArray[i, j + 1].isDark == false)
                    {
                        LerpManager.startLerping = true;
                        //Debug.Log("Haut");
                        
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceUp", true);
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", true);
                        direction = directionFaced.UP;
                        gridArray[i, j + 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j + 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, 8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j + 1, 12);
                    }
                    else if (j !=0 && gridArray[i, j - 1].isIlluminated == true && gridArray[i, j - 1].isDark == false)
                    {
                        LerpManager.startLerping = true;
                        //Debug.Log("Bas");
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceDown", true);
                        lumenAnim.SetBool("faceDown", true);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.DOWN;
                        gridArray[i, j - 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j - 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, -8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j - 1, 12);
                    }
                    else
                    {
                        luoAnim.SetBool("idleActivated", true);
                        inGameUI.SetActive(false);
                        defeatUI.SetActive(true);
       
                    }

                }

                if (gridArray[i, j].hasLuo == true && direction == directionFaced.RIGHT && gridArray[i, j].isGoal == false && foundGoal == false)
                {
                    if (i!= width && gridArray[i + 1, j].isIlluminated == true && gridArray[i, j + 1].isDark == false)
                    {
                        LerpManager.startLerping = true;
                        Debug.Log("Droite");
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceRight", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", true);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.RIGHT;
                        gridArray[i + 1, j].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i + 1, j].hasLuo = true;
                        playerTransform.position += new Vector3(8.9f, 0f, 0f);
                        foundGoal = true;
                        SetValue(i + 1, j, 12);
                    }
                    else if (j != height && gridArray[i, j + 1].isIlluminated == true && gridArray[i, j + 1].isDark == false)
                    {
                        LerpManager.startLerping = true;
                        //Debug.Log("Haut");
                        luoAnim.SetBool("faceDown", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceUp", true);
                        lumenAnim.SetBool("faceDown", false);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", true);
                        direction = directionFaced.UP;
                        gridArray[i, j + 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j + 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, 8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j + 1, 12);
                    }
                    else if (j!= 0 && gridArray[i, j - 1].isIlluminated == true && gridArray[i, j - 1].isDark == false)
                    {
                        LerpManager.startLerping = true;
                        //Debug.Log("Bas");
                        luoAnim.SetBool("faceRight", false);
                        luoAnim.SetBool("faceLeft", false);
                        luoAnim.SetBool("faceUp", false);
                        luoAnim.SetBool("faceDown", true);
                        lumenAnim.SetBool("faceDown", true);
                        lumenAnim.SetBool("faceLeft", false);
                        lumenAnim.SetBool("faceRight", false);
                        lumenAnim.SetBool("faceUp", false);
                        direction = directionFaced.DOWN;
                        gridArray[i, j - 1].value = 12;
                        gridArray[i, j].hasLuo = false;
                        SetValue(i, j, 0);
                        gridArray[i, j - 1].hasLuo = true;
                        playerTransform.position += new Vector3(0f, -8.9f, 0f);
                        foundGoal = true;
                        SetValue(i, j - 1, 12);
                    }
                    else
                    {
                        luoAnim.SetBool("idleActivated", true);
                        
                        inGameUI.SetActive(false);
                        defeatUI.SetActive(true);
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
