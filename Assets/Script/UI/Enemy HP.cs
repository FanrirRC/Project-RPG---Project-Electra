using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public Slider slider;

    private EnemyStats enemyStats;

    private void OnEnable()
    {
        if (enemyStats != null)
        {
            enemyStats.OnHealthChanged += UpdateCurrentHP;
            enemyStats.OnMaxHealthChanged += UpdateMaxHP;
        }
    }

    private void OnDisable()
    {
        if (enemyStats != null)
        {
            enemyStats.OnHealthChanged -= UpdateCurrentHP;
            enemyStats.OnMaxHealthChanged -= UpdateMaxHP;
        }
    }

    public void UpdateEnemyStats(EnemyStats newStats)
    {
        if (enemyStats != null)
        {
            enemyStats.OnHealthChanged -= UpdateCurrentHP;
            enemyStats.OnMaxHealthChanged -= UpdateMaxHP;
        }

        enemyStats = newStats;

        if (enemyStats != null)
        {
            enemyStats.OnHealthChanged += UpdateCurrentHP;
            enemyStats.OnMaxHealthChanged += UpdateMaxHP;
            slider.maxValue = enemyStats.maxHealthPoint;
            slider.value = enemyStats.currentHealthPoint;
        }
    }

    private void UpdateCurrentHP()
    {
        slider.value = enemyStats.currentHealthPoint;
    }

    private void UpdateMaxHP()
    {
        slider.maxValue = enemyStats.maxHealthPoint;
    }
}
