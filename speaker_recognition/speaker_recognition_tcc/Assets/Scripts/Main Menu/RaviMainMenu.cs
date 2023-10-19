using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaviMainMenu : MonoBehaviour
{

    public void TransicionarParaCena(string nomeDaCenaParaTransicao)
    {
        SceneManager.LoadScene(nomeDaCenaParaTransicao);
    }
}
