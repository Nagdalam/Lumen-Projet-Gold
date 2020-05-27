using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Fait par Benjamin
public class Testing2 : MonoBehaviour
{
    private Grid2 grid;
    public GameObject menuInGame, menuVictoire, menuDéfaite;
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
    public Transform movePoint;
    bool isWaiting = false;
    public int luoDirection;
    public GameObject lightPrefabBasic, lightPrefabBilateral, lightPrefabTower, darkTile;
    public int lvlID;
    public Animator luoAnim, lumenAnim;
    public AudioSource audioSource;
    bool shouldWait = false;
    public Text lightCompteur, displayChapter, displayLevel;
    bool stopPathfinding = false;
    public GameObject tutorialToActivate;
    public bool activatetutorial;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 8f * Time.deltaTime);
        grid = new Grid2(gridLength, gridHeight, cellSize, new Vector3(originX, originY, 0)) ;
        grid.numberSize = numberSize;
        grid.originPosition = new Vector3(originX, originY, 0);
        grid.SetLuo(luoXPosition, luoYPosition, luoDirection, luoAnim, lumenAnim);
        grid.SetGoal(goalXPosition, goalYPosition);
        GameManager.width = gridLength;
        GameManager.height = gridHeight;
        
        for (int i = 0; i < numberOfCrystals; i++)
        {
            grid.SetCrystal(crystalXArray[i], crystalYArray[i], crystalTypeArray[i]);
        }
        
        for (int i = 0; i < numberOfDarksTiles; i++)
        {
            grid.SetDark(darkTileXArray[i], darkTileYArray[i]);
        }
        GameManager.numberOfLights = numberOfLights;
        GameManager.intensificationAllowed = intensificationAllowed;
        GameManager.isDarkTilesAllowed = isDarkTilesAllowed;
        if(activatetutorial == true)
        {
            tutorialToActivate.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        string myTextLights =  (GameManager.numberOfLights).ToString();
        string myText = "Lights : ";

        lightCompteur.text = myText + myTextLights;

        if (GameManager.isDropped == true)
        {
            if (GameManager.numberOfLights > 0)
            {
                if (GameManager.inDarkMode == false && shouldWait == false)
                {
                    grid.UseBasicCrystal(GameManager.GetMouseWorldPosition(), 56, lightPrefabBasic, lightPrefabTower, lightPrefabBilateral, originX, originY, audioSource);
                    GameManager.isDropped = false;
                    GameManager.objectGrabbed = false;
                    StartCoroutine(WaitASecond(1f));
                }
                
                else if(GameManager.inDarkMode == true)
                {
                    grid.ActivateDark(GameManager.GetMouseWorldPosition(), darkTile, originX, originY, audioSource);
                    GameManager.inDarkMode = false;
                    GameManager.isDropped = false;
                    GameManager.objectGrabbed = false;
                    StartCoroutine(WaitASecond(1f));
                }
            }
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(GameManager.isDarkTilesAllowed == true)
            {
                grid.SwitchMode(GameManager.GetMouseWorldPosition(), darkTile, originX, originY);
            }
        }

        if (GameManager.numberOfLights <=0)
        {
            
            if(shouldWait == false && stopPathfinding == false) {

            grid.Pathfinder(gridHeight, gridLength, movePoint, shouldWait, lvlID, luoAnim, audioSource, lumenAnim, menuVictoire, menuDéfaite, menuInGame, stopPathfinding) ;
            StartCoroutine(WaitASecond(1f));
            }

        }

        IEnumerator WaitASecond(float waitTime)
        {
            shouldWait = true;
            yield return new WaitForSeconds(1f);
            shouldWait = false;
            GameManager.isDropped = false;
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
