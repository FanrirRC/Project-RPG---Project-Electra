using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject enemyHPBarPrefab;
    [SerializeField] private SpawnPositionV2 spawnPositionScript;

    private List<GameObject> enemies = new List<GameObject>();
    private List<EnemyHP> enemyHPBars = new List<EnemyHP>();

    void Start()
    {
        // Attach to the enemy spawn event
        spawnPositionScript.OnEnemySpawned += HandleEnemySpawned;
    }

    private void OnDestroy()
    {
        // Detach from the enemy spawn event
        spawnPositionScript.OnEnemySpawned -= HandleEnemySpawned;
    }

    private void HandleEnemySpawned(GameObject enemy)
    {
        if (enemy != null)
        {
            enemies.Add(enemy);
            AttachHealthBar(enemy);
        }
    }

    private void AttachHealthBar(GameObject enemy)
    {
        if (enemy != null && enemyHPBarPrefab != null)
        {
            GameObject healthBar = Instantiate(enemyHPBarPrefab, transform);
            EnemyStats enemyStats = enemy.GetComponent<ScriptableDisplayer>()?.enemyStats;

            if (enemyStats != null)
            {
                // Find the "hp" child object in the healthBar prefab
                Transform hpTransform = healthBar.transform.Find("hp");
                if (hpTransform != null)
                {
                    EnemyHP enemyHP = hpTransform.GetComponent<EnemyHP>();
                    if (enemyHP != null)
                    {
                        enemyHP.UpdateEnemyStats(enemyStats);
                        enemyHPBars.Add(enemyHP);
                    }
                    else
                    {
                        Debug.LogError("EnemyHP script is missing on the 'hp' child object of the HP bar prefab.");
                    }
                }
                else
                {
                    Debug.LogError("No 'hp' child object found in the HP bar prefab. HealthBar name: " + healthBar.name);
                }
            }
            else
            {
                Debug.LogError("EnemyStats component is missing on the enemy prefab.");
            }
        }
        else
        {
            Debug.LogError("Enemy or EnemyHPBarPrefab is null. Cannot attach health bar.");
        }
    }

}
