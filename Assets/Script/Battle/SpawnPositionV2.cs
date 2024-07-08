using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPositionV2 : MonoBehaviour
{
    [SerializeField] public GameObject[] playerPrefabs = new GameObject[3];
    [SerializeField] public GameObject enemyPrefab;

    public event Action<GameObject> OnEnemySpawned;

    void Start()
    {
        SpawnPlayers();
        GameObject enemy = SpawnEnemy();
        OnEnemySpawned?.Invoke(enemy);
    }

    private void SpawnPlayers()
    {
        List<GameObject> assignedPlayerPrefabs = new List<GameObject>();

        foreach (var character in playerPrefabs)
        {
            if (character != null)
                assignedPlayerPrefabs.Add(character);
        }

        int playerCount = assignedPlayerPrefabs.Count;

        switch (playerCount)
        {
            case 1:
                InstantiateCharacter(assignedPlayerPrefabs[0], new Vector3(0, 0.25f, -5), Quaternion.identity);
                break;
            case 2:
                InstantiateCharacter(assignedPlayerPrefabs[0], new Vector3(-1.25f, 0.25f, -5), Quaternion.identity);
                InstantiateCharacter(assignedPlayerPrefabs[1], new Vector3(1.25f, 0.25f, -5), Quaternion.identity);
                break;
            case 3:
                InstantiateCharacter(assignedPlayerPrefabs[0], new Vector3(0, 0.25f, -5), Quaternion.identity);
                InstantiateCharacter(assignedPlayerPrefabs[1], new Vector3(2.5f, 0.25f, -5), Quaternion.identity);
                InstantiateCharacter(assignedPlayerPrefabs[2], new Vector3(-2.5f, 0.25f, -5), Quaternion.identity);
                break;
        }
    }

    private GameObject SpawnEnemy()
    {
        Vector3 position = new Vector3(0, 0, 5f);
        Quaternion rotation = Quaternion.identity;
        return InstantiateCharacter(enemyPrefab, position, rotation);
    }

    private GameObject InstantiateCharacter(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab != null)
        {
            GameObject characterObject = Instantiate(prefab, position, rotation);
            return characterObject;
        }
        return null;
    }
}
