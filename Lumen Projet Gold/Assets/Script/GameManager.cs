using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

//Fait par Benjamin



public class GameManager : MonoBehaviour
{
    public static bool isDropped = false;
    public static bool canSpawn = false;
    public static bool levelFinished = false;
    public static bool gameOver = false;
    public static bool menuOpen = false;
    public static bool playCrystalSound = false;
    public static bool playStepSound = false;
    public static bool playIntensificationSound = false;
    public static bool playDarkSound = false;
    public static bool playVictorySound = false;
    //public static bool playVictorySound = false;
    //public static bool playVictorySound = false;
    //public static bool playVictorySound = false;
    //public static bool playVictorySound = false;
    //public static bool playVictorySound = false;
    //public static bool playVictorySound = false;
    public static bool audioMuted = false;
    public static bool musicMuted = false;

    private void Awake()
    {
        //PlayGamesPlatform.Activate();
        //OnConnectionResponse(PlayGamesPlatform.Instance.localUser.authenticated);
    }
    void Start()
    {
        menuOpen = false;
        gameOver = false;
        levelFinished = false;
        if (PlayerPrefs.GetInt("LevelsAvailable") == 0)
        {
            PlayerPrefs.SetInt("LevelsAvailable", 1);
            Debug.Log(PlayerPrefs.GetInt("LevelsAvailable"));
        }
        inDarkMode = false;
        

    }
    public GameObject lightPrefab;
    public const int sortingOrderDefault = 5000;
    public static bool canLuoMove = false;
    public static bool objectGrabbed;
    public static bool inDarkMode;
    public static int width;
    public static int height;
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    public static int numberOfLights;
    public static bool intensificationAllowed;
    public static bool isDarkTilesAllowed;

   
    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    private void Update()
    {
        if(canSpawn == true)
        {
            SpawnLight();
            canSpawn = false;
        }
    }
    public void SpawnLight()
    {
        Instantiate(lightPrefab, transform.position, Quaternion.identity);
    }





}
