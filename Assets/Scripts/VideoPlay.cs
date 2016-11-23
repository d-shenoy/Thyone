using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class VideoPlay : MonoBehaviour
{
    float innerRadius = 0;
    float outerRadius = 0;
    [SerializeField]
    private VrInteractiveItem interactive;

    [SerializeField]
    private SelectionSlider slider;

    private MultiVideos script;

    private string videoURL;
    public string VideoURL
    {
        get
        {
            return videoURL;
        }
        set
        {
            videoURL = value;
        }
    }

    private bool videoFound = false;
    private MediaPlayerCtrl player;
    [SerializeField]
    //private MenuProcessor menuCanvasObject;
    //private Canvas menuCanvas;

    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private bool unfade;
    private bool unfadeDone;
    private bool fadeDone;

    // Use this for initialization
    void Start()
    {
        slider.OnBarFilled += FoundVideo;
        script = GameObject.Find("VideoGameObject").GetComponent<MultiVideos>();
        player = gameObject.GetComponent<MediaPlayerCtrl>();
        player.m_TargetMaterial[0] = gameObject;
        
        MenuSelector menuSelector = GameObject.Find("MenuOrb(Clone)").GetComponent<MenuSelector>() as MenuSelector;
        menuSelector.OnMenuInvoked += ReturnQuadToOriginalPosition;

        MenuProcessor menuProcessor = GameObject.Find("Menu/Exit").GetComponent<MenuProcessor>() as MenuProcessor;
        menuProcessor.exitPressed += restartPlay;

        //menuCanvas = GameObject.Find("Menu Canvas").GetComponent<Canvas>() as Canvas;

        //menuCanvasObject = GameObject.Find("Menu Canvas").GetComponent<MenuProcessor>();
        //menuCanvasObject.OnMenuClicked += ReturnQuadToOriginalPosition;

        //menuCanvas.GetComponent<CanvasGroup>().alpha = 0;
        
        unfade = false;
        unfadeDone = false;
        fadeDone = false;

    }

    // Update is called once per frame
    void Update()
    {
        //if (script.foundGameObject != null && script.foundGameObject != gameObject)
        //{
            // if (script.foundGameObject.GetComponent<VideoPlay>().unfade == true && unfadeDone != true)
            // {
            //     unfadeDone = true;
            //     Debug.Log("Fading in...");
            //     //gameObject.SetActive(true);
            //     gameObject.GetComponent<Renderer>().enabled = true;
            //     //menuCanvas.GetComponent<CanvasGroup>().alpha = 0;
                
            // }
            // else if (fadeDone != true)
            // {
            //     Debug.Log("Fading out...");
            //     //iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", 0.2f));
            //     //gameObject.SetActive(false);
            //     gameObject.GetComponent<Renderer>().enabled = false;
            //     //Destroy(gameObject);
            //     fadeDone = true;
            // }
        //}
        //if (script.foundGameObject == gameObject)
        //{

            // if (videoFound == false)
            // {
            //     GvrViewer viewer = GameObject.Find("GvrMain").GetComponent<GvrViewer>();
            //     viewer.Recenter();
            //     player.strFileName = VideoURL;

            //     originalRotation = gameObject.transform.rotation;
            //     originalPosition = gameObject.transform.position;

            //     gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            //     iTween.MoveTo(gameObject, new Vector3(0, 15.0f, -20.0f), 2.0f);
            //     //menuCanvas.GetComponent<CanvasGroup>().alpha = 1;

            //     //iTween.MoveTo(menuCanvas.gameObject, new Vector3(0, 0.0f, -20.0f), 2.0f);

            //     videoFound = true;
            //     GetComponent<EventTrigger>().enabled = false;
            //     ;

            // }
        //}

    }

    public void ReturnQuadToOriginalPosition()
    {
        //if (script.foundGameObject == gameObject)
        //{
            player.Pause();
            //unfade = true;
            //iTween.MoveTo(gameObject, originalPosition, 2.0f);
            //gameObject.transform.rotation = originalRotation;
            //GetComponent<EventTrigger>().enabled = true;
        //}
    }

    public void restartPlay()
    {
        player.Play();
    }

    void FoundVideo()
    {
        Debug.Log("Found Video");
        //gameObject.GetComponent<Renderer>().enabled = true;

        //script.foundGameObject = gameObject;
        originalRotation = gameObject.transform.rotation;
        originalPosition = gameObject.transform.position;

        //Tween to new position on the screen.
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        iTween.MoveTo(gameObject, new Vector3(0, 0.0f, 4.0f), 2.0f);
 
        //Disable any further events on this clone.
        GetComponent<EventTrigger>().enabled = false;

        //Play the video
        script.playVideo(gameObject, VideoURL);
    }

}