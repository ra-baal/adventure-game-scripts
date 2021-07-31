using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] Prefabs;
    public int NeededPointsToActive = 0; // number of points required to active the spawn point
    public int NeededPointsToInactive = int.MaxValue;

    public void SpawnRandomOneFromList()
    {
        if (PlayerPrefs.GetInt("points", 0) < this.NeededPointsToActive
            || PlayerPrefs.GetInt("points", 0) >= this.NeededPointsToInactive)
                return; // inactive

        Instantiate(
            this.Prefabs[Random.Range(0, this.Prefabs.Length)],
            this.transform.position,
            Quaternion.identity
            );
    }

    public void Spawn(GameObject opponent)
    {
        if (PlayerPrefs.GetInt("points", 0) < this.NeededPointsToActive
            || PlayerPrefs.GetInt("points", 0) >= this.NeededPointsToInactive)
                return; // inactive

        Instantiate(
            opponent,
            this.transform.position,
            Quaternion.identity
            );
    }

}
