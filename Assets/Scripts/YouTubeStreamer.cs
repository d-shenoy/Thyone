using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class YouTubeStreamer : MonoBehaviour
{
    // public List<string> videoIDList = new List<string>(new string[] {
    //     "xkFTZcUPjBg",
    //     "h4mmeN8gv9o",
    //     "RWiI7AkDX-o",
    //     "iG9CE55wbtY",
    //     "4sZdcB6bjI8",
    //     "NiMgOklgeos",
    //     "X1DlJpPqDFo",
    //     "PdxPCeWw75k",
    //     "6LmPq7D-ds0",
    //     "dcwuBo4PvE0",
    //     "gmj-azFbpkA",
    //     "B12syeJflbQ",
    //     "Sm5xF-UYgdg",
    //     "16p9YRF0l-g",
    //     "GB4s5b9NL3I",
    //     "9kBKQS7J7xI",
    //     "8D0pwe4vaQo",
    //     "86x-u-tz0MA",
    //     "N7wF2AdVy2Q",
    //     "NAkkckxE9i8",
    //     "sY0Pf_pfqCI",
    //     "VJoQj00RZHg",
    //     "zd-dqUuvLk4",
    //     "_waBFUg_oT8",
    //     "_1VpOweDio8"
    // });

    public List<string> videoIDList = new List<string>();
    public List<string> videoURLs;
    public List<string> videoThumbnails;

    public const string youtubeBaseURI = "http://www.youtube.com/get_video_info";
    public event Action<string, List<string>> OnComplete;

    private string m_videoURL;
    public string videoURL
    {
        get
        {
            return m_videoURL;
        }
        set
        {
            m_videoURL = value;
        }
    }

    private string m_thumbnailURL;
    public string thumbNailURL
    {
        get
        {
            return m_thumbnailURL;
        }
        set
        {
            m_thumbnailURL = value;
        }
    }

    private bool m_urlsAquired;
    public bool urlsAcquired
    {
        get
        {
            return m_urlsAquired;
        }
        set
        {
            m_urlsAquired = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        //APIManager manager = gameObject.AddComponent<APIManager>();
        Categories.getVideoIds(Categories.selectedCategory, out videoIDList);
        urlsAcquired = false;
        videoURLs = new List<string>();
        videoThumbnails = new List<string>();

        //Get all video and thumbnail URLs
        foreach (string url in videoIDList)
        {
            GetURL(url);
        }
    }

    public void GetURL(string videoID)
    {

        string youtubeQuery = youtubeBaseURI + "?video_id=" + videoID;
        WWW www = new WWW(youtubeQuery);
        StartCoroutine(WaitForRequest(www));

    }

    public IEnumerator thumbnailEnumerator()
    {
        // Spit out next thumbnail if any.
        foreach (string thumbnail in videoThumbnails) yield return thumbnail;
        // Indicate end of enumeration (optional).
        yield break;
    }

    public IEnumerator videoURLEnumerator()
    {
        // Spit out next thumbnail if any.
        foreach (string url in videoURLs) yield return url;
        // Indicate end of enumeration (optional).
        yield break;
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        //Debug.Log("Response received" + www.error);

        // check for errors
        if (www.error == null)
        {
            int responseCode = getResponseCode(www);

            if (responseCode == 200)
            {
                IDictionary<string, string> dict = new Dictionary<string, string>();
                stringToDictionary(www.text, '&', out dict);

    
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    if (kvp.Key.Equals("url_encoded_fmt_stream_map"))
                    {
                        //We get something ridiculous like this - It is URL escaped ',' delimited string

                        //url%3Dhttp%253A%252F%252Fr2---sn-vgqsenl7.googlevideo.com%252Fvideoplayback%253Fitag%253D22%2526key%253Dyt6%2526mt%253D1475081959%2526upn%253DoUWmGTWdXcg%2526lmt%253D1470908563204081%2526sparams%253Ddur%25252Cid%25252Cinitcwndbps%25252Cip%25252Cipbits%25252Citag%25252Clmt%25252Cmime%25252Cmm%25252Cmn%25252Cms%25252Cmv%25252Cnh%25252Cpl%25252Cratebypass%25252Csource%25252Cupn%25252Cexpire%2526ratebypass%253Dyes%2526ipbits%253D0%2526mv%253Dm%2526nh%253DIgpwcjA5Lm9yZDEyKgkxMjcuMC4wLjE%2526source%253Dyoutube%2526ms%253Dau%2526id%253Do-AHB51v92-XdSl8b4WrO6v426zI2uFK18yzZdCCYpO9Zo%2526expire%253D1475104329%2526initcwndbps%253D2972500%2526mime%253Dvideo%25252Fmp4%2526ip%253D50.204.141.174%2526pl%253D22%2526mn%253Dsn-vgqsenl7%2526mm%253D31%2526signature%253D0D1BFE8B4742F5B71A423C6FC0D1C6F4C5F4E9BD.3C153117E8A060B55A326FAA2ABE966FDB8B22F2%2526dur%253D875.810%26itag%3D22%26type%3Dvideo%252Fmp4%253B%2Bcodecs%253D%2522avc1.64001F%252C%2Bmp4a.40.2%2522%26quality%3Dhd720%26fallback_host%3Dtc.v24.cache2.googlevideo.com%2Curl%3Dhttp%253A%252F%252Fr2---sn-vgqsenl7.googlevideo.com%252Fvideoplayback%253Fitag%253D43%2526key%253Dyt6%2526mt%253D1475081959%2526upn%253DoUWmGTWdXcg%2526lmt%253D1417449177908594%2526sparams%253Ddur%25252Cid%25252Cinitcwndbps%25252Cip%25252Cipbits%25252Citag%25252Clmt%25252Cmime%25252Cmm%25252Cmn%25252Cms%25252Cmv%25252Cnh%25252Cpl%25252Cratebypass%25252Csource%25252Cupn%25252Cexpire%2526ratebypass%253Dyes%2526ipbits%253D0%2526mv%253Dm%2526nh%253DIgpwcjA5Lm9yZDEyKgkxMjcuMC4wLjE%2526source%253Dyoutube%2526ms%253Dau%2526id%253Do-AHB51v92-XdSl8b4WrO6v426zI2uFK18yzZdCCYpO9Zo%2526expire%253D1475104329%2526initcwndbps%253D2972500%2526mime%253Dvideo%25252Fwebm%2526ip%253D50.204.141.174%2526pl%253D22%2526mn%253Dsn-vgqsenl7%2526mm%253D31%2526signature%253DCB4AE10FFE881F2E628F44DB4866741C4AECC3BE.397761972DBB895A3D13F7ACC33CEB4040DB2614%2526dur%253D0.000%26itag%3D43%26type%3Dvideo%252Fwebm%253B%2Bcodecs%253D%2522vp8.0%252C%2Bvorbis%2522%26quality%3Dmedium%26fallback_host%3Dtc.v13.cache4.googlevideo.com%2Curl%3Dhttp%253A%252F%252Fr2---sn-vgqsenl7.googlevideo.com%252Fvideoplayback%253Fitag%253D18%2526key%253Dyt6%2526mt%253D1475081959%2526upn%253DoUWmGTWdXcg%2526lmt%253D1447933771016819%2526sparams%253Ddur%25252Cid%25252Cinitcwndbps%25252Cip%25252Cipbits%25252Citag%25252Clmt%25252Cmime%25252Cmm%25252Cmn%25252Cms%25252Cmv%25252Cnh%25252Cpl%25252Cratebypass%25252Csource%25252Cupn%25252Cexpire%2526ratebypass%253Dyes%2526ipbits%253D0%2526mv%253Dm%2526nh%253DIgpwcjA5Lm9yZDEyKgkxMjcuMC4wLjE%2526source%253Dyoutube%2526ms%253Dau%2526id%253Do-AHB51v92-XdSl8b4WrO6v426zI2uFK18yzZdCCYpO9Zo%2526expire%253D1475104329%2526initcwndbps%253D2972500%2526mime%253Dvideo%25252Fmp4%2526ip%253D50.204.141.174%2526pl%253D22%2526mn%253Dsn-vgqsenl7%2526mm%253D31%2526signature%253DB304BF6DC14185D6351A63ACBF53730345484C05.52A815FAD61C002CD5EDDFE96BE88082F6C68C9F%2526dur%253D875.810%26itag%3D18%26type%3Dvideo%252Fmp4%253B%2Bcodecs%253D%2522avc1.42001E%252C%2Bmp4a.40.2%2522%26quality%3Dmedium%26fallback_host%3Dtc.v13.cache7.googlevideo.com%2Curl%3Dhttp%253A%252F%252Fr2---sn-vgqsenl7.googlevideo.com%252Fvideoplayback%253Fitag%253D36%2526nh%253DIgpwcjA5Lm9yZDEyKgkxMjcuMC4wLjE%2526signature%253DDD1777FDC6B54FD94FEFC7FA1F91BC8A15AE8AD4.381DFB933D235D72AD758A2107D46459933D0C82%2526ms%253Dau%2526mv%253Dm%2526key%253Dyt6%2526id%253Do-AHB51v92-XdSl8b4WrO6v426zI2uFK18yzZdCCYpO9Zo%2526mt%253D1475081959%2526expire%253D1475104329%2526initcwndbps%253D2972500%2526lmt%253D1417447642387022%2526mime%253Dvideo%25252F3gpp%2526ip%253D50.204.141.174%2526upn%253DoUWmGTWdXcg%2526pl%253D22%2526mn%253Dsn-vgqsenl7%2526mm%253D31%2526source%253Dyoutube%2526sparams%253Ddur%25252Cid%25252Cinitcwndbps%25252Cip%25252Cipbits%25252Citag%25252Clmt%25252Cmime%25252Cmm%25252Cmn%25252Cms%25252Cmv%25252Cnh%25252Cpl%25252Csource%25252Cupn%25252Cexpire%2526dur%253D875.856%2526ipbits%253D0%26itag%3D36%26type%3Dvideo%252F3gpp%253B%2Bcodecs%253D%2522mp4v.20.3%252C%2Bmp4a.40.2%2522%26quality%3Dsmall%26fallback_host%3Dtc.v13.cache3.googlevideo.com%2Curl%3Dhttp%253A%252F%252Fr2---sn-vgqsenl7.googlevideo.com%252Fvideoplayback%253Fitag%253D17%2526nh%253DIgpwcjA5Lm9yZDEyKgkxMjcuMC4wLjE%2526signature%253D3B0962335D695ECE72849523B14E49C05FFB0C37.DD154CD5EC488843987DDA62E86FBEA256562306%2526ms%253Dau%2526mv%253Dm%2526key%253Dyt6%2526id%253Do-AHB51v92-XdSl8b4WrO6v426zI2uFK18yzZdCCYpO9Zo%2526mt%253D1475081959%2526expire%253D1475104329%2526initcwndbps%253D2972500%2526lmt%253D1417447621003020%2526mime%253Dvideo%25252F3gpp%2526ip%253D50.204.141.174%2526upn%253DoUWmGTWdXcg%2526pl%253D22%2526mn%253Dsn-vgqsenl7%2526mm%253D31%2526source%253Dyoutube%2526sparams%253Ddur%25252Cid%25252Cinitcwndbps%25252Cip%25252Cipbits%25252Citag%25252Clmt%25252Cmime%25252Cmm%25252Cmn%25252Cms%25252Cmv%25252Cnh%25252Cpl%25252Csource%25252Cupn%25252Cexpire%2526dur%253D875.903%2526ipbits%253D0%26itag%3D17%26type%3Dvideo%252F3gpp%253B%2Bcodecs%253D%2522mp4v.20.3%252C%2Bmp4a.40.2%2522%26quality%3Dsmall%26fallback_host%3Dtc.v8.cache6.googlevideo.com

                        string[] splitStrings;
                        splitStrings = kvp.Value.Split(new char[] { ',' });

                        foreach (string s1 in splitStrings)
                        {

                            //Now you get a list of videos of different quality/formats
                            IList<string> videoList;
                            stringToList(s1, ',', out videoList);

                            foreach (string video in videoList)
                            {
                                IDictionary<string, string> videoData = new Dictionary<string, string>();
                                stringToDictionary(video, '&', out videoData);

                                string videoURL = getVideoURL(videoData);
                                // Debug.Log("VideoURL = " + videoURL);
                                if (!string.IsNullOrEmpty(videoURL))
                                {
                                    //Debug.Log("Adding URL " + videoURL);
                                    videoURLs.Add(videoURL);
                                    if (videoURLs.Count >= videoIDList.Count)
                                    {
                                        Debug.Log("All URLS acquired");
                                        urlsAcquired = true;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                string videoThumbnail = getVideoThumbnail(dict);
                if (!string.IsNullOrEmpty(videoThumbnail))
                {
                    Debug.Log("Thumbnail received");
                    videoThumbnails.Add(videoThumbnail);
                    if(videoThumbnails.Count >= videoIDList.Count)
                    {
                        OnComplete(APIManager.ParsingError.NoError, videoThumbnails);
                    }
                }
            }
            else
            {
                Debug.Log("WWW Error: " + responseCode);
            }
        }
        else{
            Debug.Log("Network error");
        }
    }

    private string getVideoURL(IDictionary<string, string> videoData)
    {
        string videoURL = null;

        if (videoData == null)
        {
            Debug.Log("Video Data is Empty");
        }

        //we will pick only hd720 - we need fallback to pick lower resolution videos
        //if we dont get 720p resolution video.
        if (videoData.ContainsKey("quality") && videoData["quality"].Equals("hd720") &&
        videoData.ContainsKey("type") && videoData["type"].Contains("mp4"))
        {
            if (videoData.ContainsKey("url"))
            {
                videoURL = WWW.UnEscapeURL(videoData["url"]);
                //Debug.Log("VideoURL (HD) = " + videoURL + "\n");
            }
        }
        else if (videoData.ContainsKey("quality") && videoData["quality"].Equals("medium") &&
        videoData.ContainsKey("type") && videoData["type"].Contains("mp4"))
        {
            if (videoData.ContainsKey("url"))
            {
                videoURL = WWW.UnEscapeURL(videoData["url"]);
                //Debug.Log("VideoURL (Medium) = " + videoURL + "\n");
            }
        }
        else if (videoData.ContainsKey("quality") && videoData["quality"].Equals("small") &&
        videoData.ContainsKey("type") && videoData["type"].Contains("mp4"))
        {
            if (videoData.ContainsKey("url"))
            {
                videoURL = WWW.UnEscapeURL(videoData["url"]);
                //Debug.Log("VideoURL (Small) = " + videoURL + "\n");
            }
        }

        return videoURL;
    }

    private string getVideoThumbnail(IDictionary<string, string> videoData)
    {
        string videoThumbnail = null;

        if (videoData.ContainsKey("iurlhq"))
        {
            videoThumbnail = WWW.UnEscapeURL(videoData["iurlhq"]);
            //Debug.Log("thumnnailURL (Hi)= " + videoThumbnail);
        }
        else if (videoData.ContainsKey("iurl"))
        {
            videoThumbnail = WWW.UnEscapeURL(videoData["iurl"]);
            // Debug.Log("thumnnailURL (Lo)= " + videoThumbnail);
        }

        return videoThumbnail;
    }

    private void stringToDictionary(string input, char delimiter, out IDictionary<string, string> dict)
    {
        string[] splitStrings;
        splitStrings = input.Split(new char[] { delimiter });

        dict = new Dictionary<string, string>();

        foreach (string s in splitStrings)
        {
            string[] kvpString;
            kvpString = s.Split(new char[] { '=' });
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(kvpString[0], kvpString[1]);

            string result;
            if (!dict.TryGetValue(kvp.Key, out result))
            {
                dict.Add(kvp);
            }
            else
            {
                Debug.Log("Unable to add " + kvp.Key + "Value = " + kvp.Value);
            }
        }
    }

    private void stringToList(string input, char delimiter, out IList<string> list)
    {
        string s2 = WWW.UnEscapeURL(input);

        string[] splitStrings;
        splitStrings = s2.Split(new char[] { ',' });
        list = new List<string>(splitStrings);
    }
    public static int getResponseCode(WWW request)
    {

        int ret = 0;
        if (request.responseHeaders == null)
        {
            Debug.LogError("no response headers.");
        }
        else
        {
            if (!request.responseHeaders.ContainsKey("STATUS"))
            {
                Debug.LogError("response headers has no STATUS.");
            }
            else
            {
                ret = parseResponseCode(request.responseHeaders["STATUS"]);
            }
        }

        return ret;
    }

    public static int parseResponseCode(string statusLine)
    {

        int ret = 0;

        string[] components = statusLine.Split(' ');

        if (components.Length < 3)
        {

            Debug.LogError("invalid response status: " + statusLine);
        }
        else
        {

            if (!int.TryParse(components[1], out ret))
            {
                Debug.LogError("invalid response code: " + components[1]);
            }
        }

        return ret;
    }
}
