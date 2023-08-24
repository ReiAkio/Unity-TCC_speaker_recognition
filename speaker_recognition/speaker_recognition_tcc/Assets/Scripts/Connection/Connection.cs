// Library Imports

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : MonoBehaviour
{
    private NewPet newPet;
    // Making the API Request
    IEnumerator SendPredictionRequest()
    {

        yield return new WaitForSeconds(5);
        
        // string ngrokUrl = "http://randomstring.ngrok.io";
        // string apiUrl = ngrokUrl + "/";
        string apiUrl = "http://127.0.0.1:5000/prediction";

        UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
        yield return webRequest.SendWebRequest();


        if (webRequest.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Error: " + webRequest.error);
    }
    else
    {
        string responseText = webRequest.downloadHandler.text;
        Debug.Log("Response: " + responseText);
        // newPet.HandleResponse(responseText);
        
        JsonResponse jsonResponse = JsonUtility.FromJson<JsonResponse>(responseText);

        string speakerValue = jsonResponse.speaker;
        Debug.Log("Speaker: " + speakerValue);

        newPet.HandleResponse(responseText);
    }
        
        
}

    private void Start()
    {
        newPet = FindObjectOfType<NewPet>();
    }

    public void OnPredictionTest()
{
     StartCoroutine(SendPredictionRequest());
}
    

public void Update()
{
    OnPredictionTest();
}

}
