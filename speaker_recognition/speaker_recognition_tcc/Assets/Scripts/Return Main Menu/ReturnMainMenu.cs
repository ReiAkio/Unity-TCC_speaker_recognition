using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    public KeyCode keyCode;
    void Update()
    {
        // Verifica se a tecla "E" foi pressionada
        if (Input.GetKeyDown(keyCode))
        {
            // Carrega a cena Main Menu (substitua "MainMenu" pelo nome de sua cena Main Menu)
            SceneManager.LoadScene("MainMenu");
        }
    }
}
