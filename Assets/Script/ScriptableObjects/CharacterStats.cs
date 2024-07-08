using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterStats", menuName = "Characters/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public event Action OnHealthChanged;
    public event Action OnMaxHealthChanged;

    [Header("Character Name")]
    public string characterName;

    [TabGroup("Visible Stats")]
    [Header("Character Level")]
    public int characterLV;
    [TabGroup("Visible Stats")]
    public int exp;
    [TabGroup("Visible Stats")]
    public int reqExp;

    [TabGroup("Visible Stats")]
    [Header("Resource Stats")]
    [SerializeField, TabGroup("Visible Stats")]
    private int _currentHealthPoint;
    public int currentHealthPoint
    {
        get => _currentHealthPoint;
        set
        {
            if (_currentHealthPoint != value)
            {
                _currentHealthPoint = value;
                OnHealthChanged?.Invoke();
            }
        }
    }

    [SerializeField, TabGroup("Visible Stats")]
    private int _maxHealthPoint;
    public int maxHealthPoint
    {
        get => _maxHealthPoint;
        set
        {
            if (_maxHealthPoint != value)
            {
                _maxHealthPoint = value;
                OnMaxHealthChanged?.Invoke();
            }
        }
    }

    [TabGroup("Visible Stats")]
    public int currentSkillPoint;
    [TabGroup("Visible Stats")]
    public int maxSkillPoint;

    [TabGroup("Visible Stats")]
    [Header("Offense Stats")]
    public int strength;
    [TabGroup("Visible Stats")]
    public int magic;

    [TabGroup("Visible Stats")]
    [Header("Defense Stats")]
    public int physicalDefense;
    [TabGroup("Visible Stats")]
    public int magicDefense;

    [TabGroup("Hidden Stats Additives")]
    [Header("Pre-Rounded Stats Additives")]
    public float maxHealthPointFloat;
    [TabGroup("Hidden Stats Additives")]
    public float maxSkillPointFloat;
    [TabGroup("Hidden Stats Additives")]
    public float strengthFloat;
    [TabGroup("Hidden Stats Additives")]
    public float magicFloat;
    [TabGroup("Hidden Stats Additives")]
    public float physicalDefenseFloat;
    [TabGroup("Hidden Stats Additives")]
    public float magicDefenseFloat;

    [TabGroup("Hidden Stats Additives")]
    [Header("Stats Increases per Level")]
    public float healthIncrease = 2.5f;
    [TabGroup("Hidden Stats Additives")]
    public float skillPointIncrease = 1.5f;
    [TabGroup("Hidden Stats Additives")]
    public float strengthIncrease = 2.5f;
    [TabGroup("Hidden Stats Additives")]
    public float magicIncrease = 2.0f;
    [TabGroup("Hidden Stats Additives")]
    public float physicalDefenseIncrease = 1.8f;
    [TabGroup("Hidden Stats Additives")]
    public float magicDefenseIncrease = 1.7f;

    [TabGroup("Hidden Stats Additives")]
    [Header("Req Exp Parameters")]
    private float BaseStart = 50;
    [TabGroup("Hidden Stats Additives")]
    private float BaseEnd = 167.5f;
    [TabGroup("Hidden Stats Additives")]
    private float Extra = 25;
    [TabGroup("Hidden Stats Additives")]
    private float AccelAStart = 0.5f;
    [TabGroup("Hidden Stats Additives")]
    private float AccelAEnd = 2;
    [TabGroup("Hidden Stats Additives")]
    private float AccelB = 2;
    [TabGroup("Hidden Stats Additives")]
    private const int maxLevel = 99;

    private void OnEnable()
    {
        // Initialize float values with the integer values on enable
        maxHealthPointFloat = maxHealthPoint;
        maxSkillPointFloat = maxSkillPoint;
        strengthFloat = strength;
        magicFloat = magic;
        physicalDefenseFloat = physicalDefense;
        magicDefenseFloat = magicDefense;

        // Initialize current health and skill points
        currentHealthPoint = maxHealthPoint;
        currentSkillPoint = maxSkillPoint;

        CalculateRequiredExp();
    }

    private void OnValidate()
    {
        OnHealthChanged?.Invoke();
        OnMaxHealthChanged?.Invoke();
    }

    public void IncreaseExp(int value)
    {
        if (characterLV >= maxLevel)
        {
            return; // Cannot gain more exp if at max level
        }

        exp += value;
        while (exp >= reqExp && characterLV < maxLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        if (characterLV < maxLevel)
        {
            characterLV++;
            CalculateRequiredExp();
            IncreaseStats();
        }
    }

    public void CalculateRequiredExp()
    {
        reqExp = Mathf.RoundToInt(CalculateRequiredExp(characterLV, BaseStart, BaseEnd, AccelAStart, AccelAEnd, AccelB, Extra));
    }

    private float CalculateRequiredExp(int lvl, float baseStart, float baseEnd, float accelAStart, float accelAEnd, float accelB, float extra)
    {
        if (lvl <= 1)
        {
            return 0; // or any other appropriate value for level <= 1
        }

        float baseValue = baseStart + (baseEnd - baseStart) * (lvl - 1) / (maxLevel - 1);
        float accelAValue = accelAStart + (accelAEnd - accelAStart) * (lvl - 1) / (maxLevel - 1);

        float term1 = Mathf.Pow(lvl - 1, 0.9f + accelAValue / 250);
        float term2 = lvl * (lvl + 1);
        float term3 = 6 + Mathf.Pow(lvl, 2) / (50 * accelB);
        float term4 = (lvl - 1) * extra;

        float result = baseValue * term1 * term2 / term3 + term4;

        // Cap the result to 999,999 if it exceeds that value
        result = Mathf.Min(result, 999999);

        return result;
    }

    public int GetRequiredExp(int lvl)
    {
        return Mathf.RoundToInt(CalculateRequiredExp(lvl, BaseStart, BaseEnd, AccelAStart, AccelAEnd, AccelB, Extra));
    }

    private void IncreaseStats()
    {
        // Increase the float values
        maxHealthPointFloat += healthIncrease;
        maxSkillPointFloat += skillPointIncrease;
        strengthFloat += strengthIncrease;
        magicFloat += magicIncrease;
        physicalDefenseFloat += physicalDefenseIncrease;
        magicDefenseFloat += magicDefenseIncrease;

        // Update the public integer values
        maxHealthPoint = Mathf.FloorToInt(maxHealthPointFloat);
        maxSkillPoint = Mathf.FloorToInt(maxSkillPointFloat);
        strength = Mathf.FloorToInt(strengthFloat);
        magic = Mathf.FloorToInt(magicFloat);
        physicalDefense = Mathf.FloorToInt(physicalDefenseFloat);
        magicDefense = Mathf.FloorToInt(magicDefenseFloat);

        // Reset current health and skill points to new max values
        currentHealthPoint = maxHealthPoint;
        currentSkillPoint = maxSkillPoint;
    }

    public void TakeDamage(int damage)
    {
        currentHealthPoint -= damage;
        if (currentHealthPoint < 0)
        {
            currentHealthPoint = 0;
        }
    }

    public void Heal(int healAmount)
    {
        currentHealthPoint += healAmount;
        if (currentHealthPoint > maxHealthPoint)
        {
            currentHealthPoint = maxHealthPoint;
        }
    }

    public void UseSkillPoints(int skillPoints)
    {
        currentSkillPoint -= skillPoints;
        if (currentSkillPoint < 0)
        {
            currentSkillPoint = 0;
        }
    }

    public void RegainSkillPoints(int skillPoints)
    {
        currentSkillPoint += skillPoints;
        if (currentSkillPoint > maxSkillPoint)
        {
            currentSkillPoint = maxSkillPoint;
        }
    }
}
