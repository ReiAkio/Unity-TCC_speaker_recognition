using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewPet : MonoBehaviour
{

    public void HandleResponse(string responseText)
    {
        var jsonData = JsonUtility.FromJson<ResponseData>(responseText);

        
        string speakerName = jsonData.speaker;
        Debug.Log("Speaker: " + speakerName);

        
    }
    
    [System.Serializable]
    public class ResponseData
    {
        public string speaker;
    }
}
