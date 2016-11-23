using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CodeProcessor : MonoBehaviour
{
    private APIManager apiManager;

    // Use this for initialization
    void Start()
    {
        apiManager = gameObject.AddComponent<APIManager>();
        apiManager.OnComplete += loadCategories;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClear()
    {
        Debug.Log("Clearing text");
        InputField input = gameObject.GetComponent<InputField>() as InputField;
        input.clear();
    }

    public void onPasscodeEntered(InputField input)
    {
        Debug.Log("Entered Text:" + input.text);

        WWWForm form = new WWWForm();
        form.AddField("access_code", input.text);

        string url = "http://api.ct1-staging.luminopia.com/patients/login";
        Hashtable headers = new Hashtable();
        headers.Add("Content-Type", "application/x-www-form-urlencoded");
        apiManager.URLRequest(url, form, headers);
    }

    public void loadCategories(string error, string response)
    {
        Debug.Log(response);
        if (error == APIManager.ParsingError.NoError)
        {
            Debug.Log("Loading Scene....");
            ScreenFader fader = GameObject.Find("FaderObject").GetComponent<ScreenFader>();
            fader.fadeIn = false;
            SceneManager.LoadScene("Categories");
        }
    }
}

public static class Extension
{
    public static void clear(this InputField inputfield)
    {
        inputfield.Select();
        inputfield.text = "";
    }
}