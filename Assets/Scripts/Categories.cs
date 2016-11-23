using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Categories : MonoBehaviour
{
    public static List<LuminopiaContent> categoriesList = new List<LuminopiaContent>();
    private bool keyFound = false;
    private const string BaseURI = "http://api.ct1-staging.luminopia.com";
    private const string Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjNDEzYjM4ZC1kMTIzLTRmMTQtYmFjMC1kM2ViZTU1YmY2NmUiLCIkdHlwZSI6InBhdGllbnQiLCJpYXQiOjE0NzU3ODczMjZ9.Ek4EHSKnu2MlaVLb41YOofAUjNLgLPOrz5cbCPovkro";
    private const string CategoriesQuery = BaseURI + "/patients/recommended_content";
    private int RetryCount = 2;
    private int currentRetry = 0;
    private APIManager manager;
    private List<string> categories;
    public static string selectedCategory = null;
    [SerializeField]
    private MenuSelector menuOrb = null;
    private GameObject categoryGameObject;
    private LayersManipulation layerManipulator;
    [SerializeField]
    private MenuProcessor menuProcessor;


    void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        categoryGameObject = GameObject.Find("CategoryGameObject");
    }

    void Start()
    {
        //Browse and Exit in categories mean the same thing.
        menuProcessor.exitPressed += OnMenuExitPressed;
        menuProcessor.browsePressed += OnMenuExitPressed;

        layerManipulator = gameObject.AddComponent<LayersManipulation>();
        layerManipulator.showLayer("CategoryLayer");
        layerManipulator.hideLayer("MenuLayer");
        manager = gameObject.AddComponent<APIManager>();
        manager.OnComplete += parseCategoriesResponse;
        getVideoCategories();
        GameObject.DontDestroyOnLoad(gameObject);

        CanvasGroup canvasGroup = GameObject.Find("Menu").GetComponent<CanvasGroup>() as CanvasGroup;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnMenuExitPressed()
    {
        GameObject canvasObject = GameObject.Find("Menu/Canvas");
        canvasObject.GetComponent<GraphicRaycaster>().enabled = false;

        Debug.Log("Exit pressed");
        iTween.FadeTo(categoryGameObject, 1.0f, 1.0f);
        foreach (Transform t in transform)
        {
            t.GetComponent<LineRenderer>().enabled = true;
        }

        layerManipulator.showLayer("CategoryLayer");
        layerManipulator.hideLayer("MenuLayer");
    }

    public void loadSceneWithVideos(string category)
    {
        selectedCategory = category;
        changeLevel();
    }

    void onDownloadComplete(string error, List<string> categoryList)
    {
        if (error == APIManager.ParsingError.NoError)
        {
            categories = categoryList;
            categories.Sort();
            displayCategories();
            //StartCoroutine(changeLevel());
            //changeLevel();
        }
        else if (currentRetry != RetryCount)
        {
            currentRetry++;
            Debug.Log("Error retrieving Categories - Retrying: !" + error);
        }
        else
        {
            Debug.Log("Retries complete, Giving up - Please check your network and try again later");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        // if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button"))
        //   print("You clicked the button!");

        //GameObject sphere = GameObject.Find("Sphere");
        // Debug.Log("Position:x = " + sphere.transform.localPosition.x + "y = " +  sphere.transform.localPosition.y + "z = " + sphere.transform.localPosition.z );
        // Debug.Log("Scale: x = " + sphere.transform.localScale.x + "y = " +  sphere.transform.localScale.y + "z = " + sphere.transform.localScale.z );

        //Drawing.DrawCircle(sphere.transform.position,100, Color.white, 10.0f, 10);
        //Drawing.DrawCircle(new Vector2(800,500),100, Color.white, 10.0f, 10);
        //GUI.Label(new Rect(500, 500, 100, 50), "Hello World!");
    }

    public IEnumerator categoriesEnumerator()
    {
        // Spit out next category if any.
        foreach (string category in categories) yield return category;
    }

    public void printCategories()
    {
        IEnumerator enumerator = categoriesEnumerator();
        while (enumerator.MoveNext())
        {
            //Debug.Log("Category: " + enumerator.Current);
        }
    }

    public void displayCategories()
    {
        int gridX = 5;
        int gridY = 3;
        float xSpacing = 7.0f;
        float ySpacing = 0f;
        float startXPosition = -10.0f;
        float startYPosition = -5.0f;

        GameObject categoryPrefab = (GameObject)Resources.Load("Prefabs/CategoryOrb") as GameObject;
        Vector3 size = categoryPrefab.GetComponent<Renderer>().bounds.size;
        Transform myTransform = categoryPrefab.transform;

        Quaternion originalRotation = myTransform.rotation;
        IEnumerator enumerator = categoriesEnumerator();

        GvrViewer viewer = GameObject.Find("GvrMain").GetComponent<GvrViewer>();
        viewer.Recenter();

        //Current x,y,z position to calculate relative positions of the prefab.
        float xPosition = 0.0f;
        float yPosition = 0.0f;
        float zPosition = 0.0f;

        Camera vrCamera = GameObject.Find("GvrMain/Head/Main Camera").GetComponentInChildren<Camera>();
        Vector3 currentPosition = gameObject.transform.position;

        float z = 10.0f;

        //Then God said - "Let there be more clones"
        for (int y = 0; y < gridY; y++)
        {
            //Reset rotations for the next row.
            myTransform.rotation = originalRotation;

            for (int x = 0; x < gridX; x++)
            {
                yPosition = (y * (size.y + ySpacing)) + startYPosition;

                enumerator.MoveNext();

                //Clone the prefab.
                GameObject clone = Instantiate(categoryPrefab, new Vector3(xPosition, yPosition, zPosition), Quaternion.identity) as GameObject;

                Vector3 temp;

                switch (x)
                {
                    case 0:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, -60.0f, clone.transform.rotation.z);
                        temp = new Vector3(startXPosition, yPosition, 14.7f);
                        clone.transform.position += temp;
                        break;
                    case 1:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, -45.0f, clone.transform.rotation.z);
                        temp = new Vector3(-6.0f, yPosition, 16.8f);
                        clone.transform.position += temp;
                        break;
                    case 2:
                        clone.transform.rotation = originalRotation;
                        temp = new Vector3(0, yPosition, 20.0f);
                        clone.transform.position += temp;
                        break;
                    case 3:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, 45.0f, clone.transform.rotation.z);
                        temp = new Vector3(5.0f, yPosition, 16.8f);
                        clone.transform.position += temp;
                        break;
                    case 4:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, 60.0f, clone.transform.rotation.z);
                        temp = new Vector3(-startXPosition, yPosition, 13.2f);
                        clone.transform.position += temp;
                        break;
                }

                //Put it under the CategoryGameObject.
                clone.transform.SetParent(categoryGameObject.transform);

                TextMesh[] text = clone.GetComponentsInChildren<TextMesh>();
                text[0].text = enumerator.Current as string;
                clone.name = enumerator.Current as string;

                //This is if we are using the experimental 2D prefab.
                //Remove later if we dont ever use it.
                //Text [] text = clone.GetComponentsInChildren<Text>();
                //text[0].text = enumerator.Current as string;
            }
        }

        MenuSelector newMenuOrb = Instantiate(menuOrb, new Vector3(startXPosition + 10.0f, startYPosition - 2.0f, 5.0f), Quaternion.Euler(45.0f, 0, 0)) as MenuSelector;
        newMenuOrb.OnMenuInvoked += showMenu;
    }

    public void getVideoCategories()
    {
        Hashtable headers = new Hashtable();
        headers.Add("authorization", Authorization);

        manager.URLRequest(CategoriesQuery, null, headers);
    }

    public static void getVideoIds(string category, out List<string> videos)
    {
        videos = categoriesList.Find((x) => category.ToLower().Equals(x.name, StringComparison.InvariantCultureIgnoreCase)).videoIds;
    }

    public void parseCategoriesResponse(string error, string response)
    {
        List<string> categoryNames = new List<string>();

        //Network error.
        if (error != APIManager.ParsingError.NoError)
        {
            onDownloadComplete(error, null);
            return;
        }

        //Successful Network response, Proceed with parsing JSON response.
        JSONObject obj = new JSONObject(response);
        if (obj == null)
        {
            error = APIManager.ParsingError.JSONParsingError;
        }
        foreach (JSONObject jobj in obj.list)
        {
            List<string> test = new List<string>();
            foreach (JSONObject j in jobj.list[0].list)
            {
                string videoID = j.ToString().Split(new string[] { "youtube://video/" }, StringSplitOptions.None)[1].TrimEnd(new char[] { '"' });
                test.Add(videoID);
            }
            LuminopiaContent content = new LuminopiaContent(jobj.list[1].ToString(), test);
            if (content.name != null)
            {
                categoriesList.Add(content);
            }
            else
            {
                error = APIManager.ParsingError.JSONParsingError;
                break;
            }
        }
        foreach (LuminopiaContent content in categoriesList)
        {
            categoryNames.Add(content.name);
        }
        onDownloadComplete(error, categoryNames);
    }

    public void showMenu()
    {
        CanvasGroup canvasGroup = GameObject.Find("Menu").GetComponent<CanvasGroup>() as CanvasGroup;
        canvasGroup.blocksRaycasts = true;

        Debug.Log("Showing Menu...");
        iTween.FadeTo(categoryGameObject, 0.7f, 1.0f);
        foreach (Transform t in transform)
        {
            t.GetComponent<LineRenderer>().enabled = false;
        }
        layerManipulator.showLayer("MenuLayer");
    }

    void changeLevel()
    {
        layerManipulator.hideLayer("MenuLayer");
        ScreenFader fader = gameObject.GetComponent<ScreenFader>();
        fader.fadeIn = false;
        SceneManager.LoadScene("Videos");
    }
}
