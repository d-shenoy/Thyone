using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class APIManager : MonoBehaviour
{
    public event Action<string, string> OnComplete;

    public static class ParsingError
    {
        public static string JSONParsingError = "Error in Parsing JSON - REST API Issues ?";
        public static string NetworkResponseError = "Error in response - Network issue ?";
        public static string NoError = "No Error";
    }

    public class HttpResponseError
    {
        public string code;
        public string reason;
        public string message;

        public HttpResponseError(string code, string reason, string message)
        {
            this.code = code;
            this.reason = reason;
            this.message = message;
        }
    }

    public void URLRequest(string url, WWWForm form, Hashtable headers)
    {
        byte[] data = (form != null) ? form.data : null;
        WWW www = new WWW(url, data, Utils.HashtableToDictionary<string, string>(headers));

        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        string error = ParsingError.NoError;

        error = (www.error != null) ? ParsingError.NetworkResponseError : ParsingError.NoError;

        if (error != ParsingError.NoError)
        {
            HttpResponseError networkError = checkResponseCode(www.text);

            if (networkError.code != "200")
            {
                error = ParsingError.NetworkResponseError + networkError.code;
            }
        }
        OnComplete(error, www.text);
    }

    public static HttpResponseError checkResponseCode(string statusLine)
    {
        JSONObject obj = new JSONObject(statusLine);
        if (obj != null)
        {
            HttpResponseError error = new HttpResponseError(obj["statusCode"].ToString(), obj["error"].ToString(), obj["message"].ToString());
            return error;
        }
        return new HttpResponseError(null, "JSONParsingError", "Unable to parse the response string");
    }
}
