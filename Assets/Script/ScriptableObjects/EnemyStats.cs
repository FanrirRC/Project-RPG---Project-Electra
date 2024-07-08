using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "Enemies/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public event Action OnHealthChanged;
    public event Action OnMaxHealthChanged;

    [BoxGroup("Enemy Profile")]
    public string enemyName;
    [BoxGroup("Enemy Profile")]
    public GameObject model3D;

    [BoxGroup("Enemy Level")]
    public int enemyLV;

    [BoxGroup("Enemy Exp")]
    public int enemyExp;

    [BoxGroup("Enemy Stats")]
    [TitleGroup("Enemy Stats/Resource Stats")]
    [SerializeField, TitleGroup("Enemy Stats/Resource Stats")]
    private int _currentHealthPoint;
    public int currentHealthPoint
    {
        get => _currentHealthPoint;
        set
        {
            _currentHealthPoint = value;
            OnHealthChanged?.Invoke();
        }
    }

    [SerializeField, TitleGroup("Enemy Stats/Resource Stats")]
    private int _maxHealthPoint;
    public int maxHealthPoint
    {
        get => _maxHealthPoint;
        set
        {
            _maxHealthPoint = value;
            OnMaxHealthChanged?.Invoke();
        }
    }

    [TitleGroup("Enemy Stats/Resource Stats")]
    public int skillPoint;

    [TitleGroup("Enemy Stats/Offense Stats")]
    public int strength;
    [TitleGroup("Enemy Stats/Offense Stats")]
    public int magic;

    [TitleGroup("Enemy Stats/Defense Stats")]
    public int physicalDefense;
    [TitleGroup("Enemy Stats/Defense Stats")]
    public int magicDefense;

    private void OnEnable()
    {
        // Initialize current health
        currentHealthPoint = maxHealthPoint;
    }

    private void OnValidate()
    {
        OnHealthChanged?.Invoke();
        OnMaxHealthChanged?.Invoke();
    }
}
