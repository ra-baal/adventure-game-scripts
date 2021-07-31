using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    private static bool isPaused;
    private TextDisplay textDisplay;

    private void Start()
    {
        this.textDisplay = FindObjectOfType<TextDisplay>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.switchPauseGame();

            if (isPaused)
                this.textDisplay.displayText("PAUSE\n\nPress Esc to continue the game\nPress M to return to the menu.", float.PositiveInfinity);
            else
                this.textDisplay.displayText("", 0);
        }
        else if (isPaused && Input.GetKeyDown("m"))
        {
            this.switchPauseGame(); // pause off
            SceneManager.LoadScene("Menu");
        }
    }

    private void switchPauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}