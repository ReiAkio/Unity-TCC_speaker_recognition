using UnityEngine;

public class PlayerInstantiationManager : MonoBehaviour
{
    public GameObject playerPrefab;// Assign your player prefab in the inspector
    private PetAnimationManager petAnimManager;

    public bool CanPlayerAct()
    {
        return Connection.statusCode == 200;
    }


    private void Start()
    {
        // existing subscriptions...
        StartCoroutine(Connection.SendRaviRequest(InstantiatePlayerBasedOnSpeaker));
    }

    private void InstantiatePlayerBasedOnSpeaker(string speaker)
    {
        if (!string.IsNullOrEmpty(speaker))
        {
            // Here you might want to have some logic to determine the color based on the speaker
            // For the purpose of this example, I'll use random colors.
            InstantiatePlayerWithNewColor();
        }
    }

    private void OnDestroy()
    {
        // Always good practice to unsubscribe when the object is destroyed.
        Connection.onRequestComplete -= HandleRequestComplete;
    }

    private void HandleRequestComplete(long statusCode)
    {
        if (statusCode == 200)
        {
            InstantiatePlayerWithNewColor();
        }
        else
        {
            Debug.Log(statusCode);
        }
    }

    private void InstantiatePlayerWithNewColor()
    {
        // Create a new player at position (0,0,0) and no rotation.
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        // Create a new random color.
        Color newColor = new Color(Random.value, Random.value, Random.value); 

        // Set the new player's color.
        newPlayer.GetComponent<SpriteRenderer>().color = newColor; 
    }
}