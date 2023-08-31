using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAnimationManager : MonoBehaviour

{
    [SerializeField]

    public Animator idlePetAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        idlePetAnimation = GetComponent<Animator>();
        idlePetAnimation.enabled = false;
    }
    
    IEnumerator InitiatePredictionRequest()
    {
        // Start the request and get the UnityWebRequest object
        yield return Connection.SendPredictionRequest();
    }
    
    private void HandlePredictionRequestCompleted(bool success)
    {
        if (success)
        {
            PlayIdlePetAnimation("IdlePetAnimation");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        PlayIdlePetAnimation("IdlePetAnimation");
        
    }

    public void PlayIdlePetAnimation(string animation)
    {
        
        if (Connection.statusCode == 200)
        {
            idlePetAnimation.enabled = true;
            idlePetAnimation.Play(animation);
            Debug.Log("Connection status code: " + Connection.statusCode);
        }
            
        }
    }
    

