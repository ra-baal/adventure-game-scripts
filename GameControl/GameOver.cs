using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Player player;
    private bool isOver = false;

    private void Start()
    {
        this.player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (this.isOver)
            return; // waiting for loading menu

        if (this.player.isDead)
        {
            this.isOver = true;
            FindObjectOfType<TextDisplay>().displayText("Game Over", 3);

            StartCoroutine(this.goToMenuCoroutine());
        }
    }

    private IEnumerator goToMenuCoroutine()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Menu");
    }
}
