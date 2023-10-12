// Library Imports

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : MonoBehaviour
{

    public static long statusCode { get; private set; }
    public delegate void OnRequestComplete(long statusCode);
    public static event OnRequestComplete onRequestComplete;
    // Making the API Request
    public static IEnumerator SendPredictionRequest()
    {
        // string ngrokUrl = "http://randomstring.ngrok.io";
        // string apiUrl = ngrokUrl + "/";
        string apiUrl = "http://127.0.0.1:5000/prediction";

        UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
        yield return webRequest.SendWebRequest();


        if (webRequest.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Error: " + webRequest.error);
        statusCode = webRequest.responseCode;
        Debug.Log(("error code: " + statusCode));

    }
    else
    {
        string responseText = webRequest.downloadHandler.text;
        Debug.Log("Response: " + responseText);
        statusCode = webRequest.responseCode;
        Debug.Log(("Status Code: " + statusCode));
    }
        
        onRequestComplete?.Invoke(statusCode);
}

[System.Serializable]
public class PredictionResponse
{
    public string speaker;
    public string speech;
}

public void OnPredictionTest()
{
     StartCoroutine(SendPredictionRequest());
}


public void Start()
{
    OnPredictionTest();
}

}
