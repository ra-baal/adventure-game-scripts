using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject FirstTerrainTile;
    public GameObject[] TerrainTiles2; // top
    public GameObject[] TerrainTiles2to1; // top to center
    public GameObject[] TerrainTiles1to2; // center to top
    public GameObject[] TerrainTiles1; // center
    public GameObject[] TerrainTiles1to0; // center to bottom
    public GameObject[] TerrainTiles0to1; // bottom to center
    public GameObject[] TerrainTiles0; // bottom

    private BoxCollider2D boxCollider2d;

    private int index = 0;
    private int currentLevelHeight = 1;

    private void Start()
    {
        this.boxCollider2d = this.GetComponent<BoxCollider2D>();

        generateFirstTile();
        generateTile();
        generateTile();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            this.generateTile();
    }

    private void generateFirstTile()
    {
        GameObject terrainTile = Instantiate(this.FirstTerrainTile);
        terrainTile.transform.position = Vector2.right * index * 21f; // 21 is x-length of one tile
        this.index++;
    }

    private void generateTile()
    {
        int level = this.chooseLevelHeight();

        GameObject terrainTile = Instantiate(this.randomTerrainTile(level));
        terrainTile.transform.position = Vector2.right * index * 21f; // 21 is x-length of one tile
        this.boxCollider2d.transform.position = Vector2.right * (index - 2) * 21f;
        this.index++;
        this.currentLevelHeight = level;

        // spawning opponents
        foreach (SpawnPoint spawnPoint in terrainTile.GetComponentsInChildren<SpawnPoint>())
        {
            int r = Random.Range(0, 10); // 0 to 9
            if ( r < 8) // 80% chance   
                spawnPoint.SpawnRandomOneFromList();
        }
    }

    private GameObject randomTerrainTile(int newLevelHeight)
    {
        if (newLevelHeight == this.currentLevelHeight) // 0, 1 or 2
        {
            switch (newLevelHeight)
            {
                case 0:
                    return this.TerrainTiles0[Random.Range(0, this.TerrainTiles0.Length)];
                case 1:
                    return this.TerrainTiles1[Random.Range(0, this.TerrainTiles1.Length)];
                case 2:
                    return this.TerrainTiles2[Random.Range(0, this.TerrainTiles2.Length)];
            }
        }
        else if (newLevelHeight > this.currentLevelHeight) // 1 or 2 (goes up)
        {
            switch (newLevelHeight)
            {
                case 1:
                    return this.TerrainTiles0to1[Random.Range(0, this.TerrainTiles0to1.Length)];
                case 2:
                    return this.TerrainTiles1to2[Random.Range(0, this.TerrainTiles1to2.Length)];
            }
        }
        else if (newLevelHeight < this.currentLevelHeight) // 1 or 2 (goes down)
        {
            switch (newLevelHeight)
            {
                case 0:
                    return this.TerrainTiles1to0[Random.Range(0, this.TerrainTiles1to0.Length)];
                case 1:
                    return this.TerrainTiles2to1[Random.Range(0, this.TerrainTiles2to1.Length)];
            }
        }

        throw new System.ArgumentException("Invalid level height.");
    }

    private int chooseLevelHeight()
    {
        switch (this.currentLevelHeight)
        {
            case 0:
                return Random.Range(0, 2); // 0 or 1
            case 1:
                return Random.Range(0, 3); // 0, 1 or 2
            case 2:
                return Random.Range(1, 3); // 1 or 2
        }

        throw new System.ArgumentException("Invalid level height.");
    }

}
