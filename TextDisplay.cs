using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    private Rect rect;
    private GUIStyle style;

    private Coroutine coroutine;

    private string text = "";

    public void Start()
    {
        this.rect = new Rect(Screen.width / 2, Screen.height / 5, 100, 30);

        this.style = new GUIStyle
        {
            fontSize = 35,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

    }

    public void displayText(string text, float duration)
    {
        if (coroutine != null)
            this.StopCoroutine(coroutine);

        this.text = text;

        coroutine = this.StartCoroutine(this.clearTextCoroutine(duration));
    }

    public void clearText()
    {
        this.text = "";
    }

    private IEnumerator clearTextCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.text = "";
    }

    private void OnGUI()
    {
        GUI.Label(rect, text, style);
    }

}
