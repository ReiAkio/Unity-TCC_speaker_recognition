using UnityEngine;

public class LightBulb : MonoBehaviour
{
    public GameObject lightEffect; // Assign the light effect sprite in the inspector

    // Call this method to "turn on" the light
    public void TurnOnLight()
    {
        lightEffect.SetActive(true);
    }

    // Call this method to "turn off" the light
    public void TurnOffLight()
    {
        lightEffect.SetActive(false);
    }
}