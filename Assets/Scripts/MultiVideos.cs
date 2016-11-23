using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiVideos : MonoBehaviour
{
    public int gridX = 5;
    public int gridY = 5;
    public float xSpacing = 5.0f;
    public float ySpacing = 2.0f;
    public float startXPosition = -30;
    public float startYPosition = -20;
    private int currentRetry = 0;
    private int RetryCount = 2;
    [SerializeField]
    private MenuSelector menuOrb = null;
    private List<string> thumbnails;
    private YouTubeStreamer youTubeStreamerScript;
    private LayersManipulation layerManipulator;
    private GameObject videoGameObject;
    [SerializeField]
    //private MenuProcessor menuProcessor;
    IEnumerator Start()
    {
        MenuProcessor exitProcessor = GameObject.Find("Menu/Exit").GetComponent<MenuProcessor>() as MenuProcessor;
        MenuProcessor browseProcessor = GameObject.Find("Menu/MenuRow/Browse").GetComponent<MenuProcessor>() as MenuProcessor;

        exitProcessor.exitPressed += onMenuExitPressed;
        browseProcessor.browsePressed += onBrowsePressed;

        layerManipulator = gameObject.AddComponent<LayersManipulation>();
        layerManipulator.showLayer("VideoLayer");
        layerManipulator.hideLayer("MenuLayer");

        //Destroy the Category GameObject and save some memory.
        GameObject.Destroy(GameObject.Find("CategoryGameObject"));

        videoGameObject = GameObject.Find("VideoGameObject");

        YouTubeStreamer m_script;
        m_script = videoGameObject.GetComponent<YouTubeStreamer>();

        youTubeStreamerScript = gameObject.GetComponent<YouTubeStreamer>(); ;
        youTubeStreamerScript.OnComplete += onDownloadComplete;

        while (m_script.urlsAcquired == false)
        {
            yield return null;
        }
    }

    public void onBrowsePressed()
    {
        Debug.Log("Browse Pressed");
        SceneManager.LoadScene("Categories");
    }

    public void onMenuExitPressed()
    {
        CanvasGroup canvasGroup = GameObject.Find("Menu").GetComponent<CanvasGroup>() as CanvasGroup;
        canvasGroup.blocksRaycasts = false;

        Debug.Log("Exit pressed");
        layerManipulator.showLayer("VideoLayer");
        layerManipulator.hideLayer("MenuLayer");
    }

    public void playVideo(GameObject clonedPrefab, string videoURL)
    {
        GvrViewer viewer = GameObject.Find("GvrMain").GetComponent<GvrViewer>();
        viewer.Recenter();
        MediaPlayerCtrl player = clonedPrefab.GetComponent<MediaPlayerCtrl>();
        player.m_strFileName = videoURL;
    }

    void onDownloadComplete(string error, List<string> thumbnailList)
    {
        if (error == APIManager.ParsingError.NoError)
        {
            thumbnails = thumbnailList;
            DisplayVideoThumbnails();
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

    public void DisplayVideoThumbnails()
    {
        int gridX = 5;
        int gridY = 5;
        float xSpacing = 5.0f;
        float ySpacing = 0.0f;
        float startXPosition = -15.0f;
        float startYPosition = -5.0f;
        float exitButtonPosition = startYPosition / 2;

        GameObject thumbnailPrefab = (GameObject)Resources.Load("Prefabs/Thumbnail") as GameObject;
        Vector3 size = thumbnailPrefab.GetComponent<Renderer>().bounds.size;
        Transform myTransform = thumbnailPrefab.transform;

        Quaternion originalRotation = myTransform.rotation;

        IEnumerator thumbnailEnumerator = youTubeStreamerScript.thumbnailEnumerator();
        IEnumerator videoURLEnumerator = youTubeStreamerScript.videoURLEnumerator();

        GvrViewer viewer = GameObject.Find("GvrMain").GetComponent<GvrViewer>();
        viewer.Recenter();

        //Current x,y position to calculate relative positions of the prefab.
        float xPosition = 0.0f;
        float yPosition = 0.0f;
        float zPosition = 0.0f;

        //Then God said - "Let there be more clones"
        for (int y = 0; y < gridY; y++)
        {

            //Reset rotations for the next row.
            myTransform.rotation = originalRotation;

            for (int x = 0; x < gridX; x++)
            {
                yPosition = (y * (size.y / 2 + ySpacing)) + startYPosition;

                //Clone the prefab.
                GameObject clone = Instantiate(thumbnailPrefab, new Vector3(xPosition, yPosition, zPosition), Quaternion.identity) as GameObject;
                //Put it under the GvrMain object.
                //clone.transform.SetParent(viewer.transform);
                clone.transform.SetParent(videoGameObject.transform);

                Vector3 temp;

                switch (x)
                {
                    case 0:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, -60.0f, clone.transform.rotation.z);
                        temp = new Vector3(startXPosition, yPosition, 14.7f);
                        clone.transform.position += temp;
                        break;
                    case 1:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, -25.0f, clone.transform.rotation.z);
                        temp = new Vector3(-8.0f, yPosition, 20.0f);
                        clone.transform.position += temp;
                        break;
                    case 2:
                        clone.transform.rotation = originalRotation;
                        temp = new Vector3(0, yPosition, 24.0f);
                        clone.transform.position += temp;
                        break;
                    case 3:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, 25.0f, clone.transform.rotation.z);
                        temp = new Vector3(8.0f, yPosition, 20.0f);
                        clone.transform.position += temp;
                        break;
                    case 4:
                        clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.x, 60.0f, clone.transform.rotation.z);
                        temp = new Vector3(-startXPosition, yPosition, 14.7f);
                        clone.transform.position += temp;
                        break;
                }

                thumbnailEnumerator.MoveNext();
                string url = thumbnailEnumerator.Current as string;

                //Set the video URL on this clone so it can be played later.
                VideoPlay player = clone.GetComponent<VideoPlay>();
                videoURLEnumerator.MoveNext();
                player.VideoURL = videoURLEnumerator.Current as string;
                Renderer renderer = clone.GetComponent<Renderer>();
                renderer.material.mainTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);

                //Apply thumbnail texture to the Prefab.
                WWW www = new WWW(url);
                StartCoroutine(applyTexture(www, clone));
            }
        }

        MenuSelector newMenuOrb = Instantiate(menuOrb, new Vector3(0, startYPosition - 2.0f, 5.0f), Quaternion.Euler(45, 0, 0)) as MenuSelector;
        newMenuOrb.OnMenuInvoked += showMenu;
        newMenuOrb.gameObject.layer = LayerMask.NameToLayer("VideoLayer");
    }

    IEnumerator applyTexture(WWW www, GameObject clone)
    {
        yield return www;
        Renderer renderer = clone.GetComponent<Renderer>();
        renderer.material.mainTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
        www.LoadImageIntoTexture(renderer.material.mainTexture as Texture2D);
    }

    public void showMenu()
    {
        Debug.Log("Showing Menu...");
       CanvasGroup canvasGroup = GameObject.Find("Menu").GetComponent<CanvasGroup>() as CanvasGroup;
        canvasGroup.blocksRaycasts = true;

        iTween.FadeTo(videoGameObject, 0.7f, 1.0f);
        layerManipulator.showLayer("OverlayLayer");
        layerManipulator.showLayer("MenuLayer");
    }
}
