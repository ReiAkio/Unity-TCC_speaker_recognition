// Library Imports

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : MonoBehaviour
{

    private float timer = 0.0f;
    public float waitTime = 2.0f; // Time in seconds between each request.
    private string lastResponse = "";

    public static long statusCode { get; private set; }

    public delegate void OnRequestComplete(long statusCode);

    public delegate void OnSpeakerReceived(string speaker);

    public static event OnRequestComplete onRequestComplete;
    public static event OnSpeakerReceived onSpeakerReceived;

    public static string raviSpeaker { get; private set; }
    public static string predictionSpeaker { get; private set; }

    // Making the API Request
    public IEnumerator SendPredictionRequest()
    {
        string apiUrl = "http://127.0.0.1:5000/prediction";
        UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + webRequest.error);
            statusCode =
                (long) webRequest
                    .result; // Isto pode não ser inteiramente preciso, você deve verificar o valor real em caso de erro
        }
        else
        {
            string responseText = webRequest.downloadHandler.text;
            statusCode =
                (long) webRequest
                    .responseCode; // Aqui é onde definimos o statusCode com o código de status HTTP da resposta

            // Check if the new response is different from the last one
            if (responseText != lastResponse)
            {
                lastResponse = responseText; // Update the last response
                Debug.Log("New data received: " + responseText);
                HandleNewData(responseText);

            }
            else
            {
                Debug.Log("Data is the same as the last request.");
            }
        }

        onRequestComplete?.Invoke(statusCode); // Agora statusCode deve conter o código de status HTTP correto
    }

    private void HandleNewData(string responseText)
    {
        // Parse the new JSON data
        PredictionResponse response = JsonUtility.FromJson<PredictionResponse>(responseText);
        predictionSpeaker = response.speaker;

        // Find the PlayerActions component and call HandleSpeech
        var playerActions = FindObjectOfType<PlayerActions>();
        if (playerActions != null)
        {
            playerActions.HandleSpeech(response.speech);

        }
    }

    public static IEnumerator SendRaviRequest(Action<string> onSpeakerReceived)
    {
        string apiUrl = "http://127.0.0.1:5000/ravi";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                PredictionResponse response = JsonUtility.FromJson<PredictionResponse>(responseText);
                Debug.Log(("Status Code endpoint Ravi: " + response.speaker));
                raviSpeaker = response.speaker;

                onSpeakerReceived?.Invoke(response.speaker);
            }
        }
    }

    [System.Serializable]
    public class PredictionResponse
    {
        public string speaker;

        // "speech" will be null if the JSON doesn't contain it
        public string speech;
    }

    public void OnPredictionTest()
    {
        StartCoroutine(SendPredictionRequest());
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            timer = 0f;
            StartCoroutine(SendPredictionRequest());
        }
    }

    public void Start()
    {
        OnPredictionTest();

    }
}
