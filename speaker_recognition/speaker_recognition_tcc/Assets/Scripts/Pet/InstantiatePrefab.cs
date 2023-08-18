using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject petPrefab;

    private void Start()
    {
        Connection.OnResponseReceived += ChangePrefabColor;
    }

    private void OnDestroy()
    {
        Connection.OnResponseReceived -= ChangePrefabColor;
    }

    private void ChangePrefabColor(string petName)
    {
        if (petName == "Cindy")
        {
            GameObject instantiatedPet = Instantiate(petPrefab, transform.position, Quaternion.identity);

            // Change the color of the instantiated prefab to a random color
            Renderer renderer = instantiatedPet.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                renderer.material.color = randomColor;
            }
        }
    }
}