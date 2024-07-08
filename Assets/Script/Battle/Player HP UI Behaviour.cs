using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPUIBehaviour : MonoBehaviour
{
    // Reference to the SpawnPositionV2 script
    [SerializeField] private SpawnPositionV2 spawnPositionScript;

    // GameObjects to be activated/deactivated based on the playerPrefabs count
    [SerializeField] private GameObject[] gameObjectsToManage = new GameObject[3];

    // References to HP scripts attached to HP UI objects
    private HP[] hpScripts;

    void Start()
    {
        // Initialize array of HP scripts based on gameObjectsToManage
        hpScripts = new HP[gameObjectsToManage.Length];
        for (int i = 0; i < gameObjectsToManage.Length; i++)
        {
            HP hpScript = gameObjectsToManage[i].GetComponentInChildren<HP>();
            if (hpScript != null)
            {
                hpScripts[i] = hpScript;
            }
            else
            {
                Debug.LogError("HP script not found on HP UI object: " + gameObjectsToManage[i].name);
            }
        }

        ManageGameObjectsBasedOnPlayerPrefabs();
    }

    private void ManageGameObjectsBasedOnPlayerPrefabs()
    {
        int playerCount = 0;

        // Count how many player characters are assigned in the SpawnPositionV2 script
        foreach (var character in spawnPositionScript.playerPrefabs)
        {
            if (character != null)
                playerCount++;
        }

        // Ensure there are no more than 3 player characters
        if (playerCount > 3)
        {
            playerCount = 3; // Limit player count to 3
        }

        // Activate/deactivate game objects based on player count
        for (int i = 0; i < gameObjectsToManage.Length; i++)
        {
            if (i < playerCount)
            {
                gameObjectsToManage[i].SetActive(true);
                // Update the HP UI directly
                UpdateHPUI(i);
            }
            else
            {
                gameObjectsToManage[i].SetActive(false);
            }
        }
    }

    private void UpdateHPUI(int index)
    {
        HP hpScript = hpScripts[index];
        GameObject playerPrefab = spawnPositionScript.playerPrefabs[index];

        if (hpScript != null && playerPrefab != null)
        {
            ScriptableDisplayer displayer = playerPrefab.GetComponent<ScriptableDisplayer>();

            if (displayer != null && displayer.characterStats != null)
            {
                CharacterStats characterStats = displayer.characterStats;

                // Subscribe to health change events
                characterStats.OnHealthChanged += () => OnHealthChanged(characterStats, hpScript);
                characterStats.OnMaxHealthChanged += () => OnMaxHealthChanged(characterStats, hpScript);

                // Update HP UI using CharacterStats data
                hpScript.slider.minValue = 0;
                hpScript.slider.maxValue = characterStats.maxHealthPoint;
                hpScript.slider.value = characterStats.currentHealthPoint;
                hpScript.fill.color = hpScript.gradient.Evaluate(hpScript.slider.normalizedValue);
                hpScript.hpText.text = $"HP: {characterStats.currentHealthPoint} / {characterStats.maxHealthPoint}";
            }
            else
            {
                Debug.LogError($"ScriptableDisplayer or CharacterStats component not found on player prefab: {playerPrefab.name}. Cannot update HP UI.");
            }
        }
        else
        {
            Debug.LogError($"HP script or player prefab is null at index {index}. Cannot update HP UI.");
        }
    }

    private void OnHealthChanged(CharacterStats characterStats, HP hpScript)
    {
        hpScript.slider.value = characterStats.currentHealthPoint;
        hpScript.fill.color = hpScript.gradient.Evaluate(hpScript.slider.normalizedValue);
        hpScript.hpText.text = $"HP: {characterStats.currentHealthPoint} / {characterStats.maxHealthPoint}";
    }

    private void OnMaxHealthChanged(CharacterStats characterStats, HP hpScript)
    {
        hpScript.slider.maxValue = characterStats.maxHealthPoint;
        hpScript.slider.value = characterStats.currentHealthPoint; // Adjust the current value if necessary
        hpScript.fill.color = hpScript.gradient.Evaluate(hpScript.slider.normalizedValue);
        hpScript.hpText.text = $"HP: {characterStats.currentHealthPoint} / {characterStats.maxHealthPoint}";
    }
}
