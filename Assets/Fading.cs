using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour
{

    public Texture2D fadeOutTextture;
    public float fadeSpeed = 0.0f;

    private int drawDepth = -1000;
    private float alpha = 1;
    private int fadeDir = -1;  //in = -1; out = 1
    private bool beginFade = false;

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        if (beginFade == true)
        {
  	    	alpha += fadeDir * fadeSpeed * Time.deltaTime;
    	    alpha = Mathf.Clamp01(alpha);
  			Debug.Log("alpha:" + alpha);
            GUI.color = new Color(GUI.color.r, GUI.color.b, alpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTextture);
			beginFade = false;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        beginFade = true;
        return (fadeSpeed);
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
        BeginFade(-1);
    }
}
