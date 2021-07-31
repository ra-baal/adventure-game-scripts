using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("points", 0);
    }

    static public void AddPoints(int points)
    {
        PlayerPrefs.SetInt(
            "points",
            PlayerPrefs.GetInt("points", 0) + points
            );
    }

    private void OnGUI()
    {
        var style = new GUIStyle
        {
            fontSize = 50,
            fontStyle = FontStyle.Bold
        };

        int points = PlayerPrefs.GetInt("points", 0);

        GUI.Label(
            new Rect(20, 20, 100, 30),
            PlayerPrefs.GetInt("points", 0).ToString(), 
            style);
    }
}
