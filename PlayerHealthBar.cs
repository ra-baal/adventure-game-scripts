using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public Texture2D texture;

    private Player player;

    private void Start()
    {
        this.player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnGUI()
    {
        // health
        float maxWidth = Screen.width * 0.25f;
        float currentWith = player.CurrentHealth / (float)player.MaxHealth * maxWidth;
        GUI.DrawTexture(
            new Rect(
                Screen.width - currentWith - 20,
                Screen.height - 40,
                currentWith,
                20),
            texture);
    }
}
