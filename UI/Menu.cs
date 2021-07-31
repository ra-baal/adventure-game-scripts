using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayRandomMap()
    {
        SceneManager.LoadScene("RandomMap");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
