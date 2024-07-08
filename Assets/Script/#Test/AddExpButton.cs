using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddExpButton : MonoBehaviour
{
    public CharacterStats characterStats; // Assign in the Inspector
    public Button addButton;              // Assign in the Inspector
    public int expToAdd = 2000;

    void Start()
    {
        if (addButton != null)
        {
            addButton.onClick.AddListener(OnAddExpButtonClick);
        }
        else
        {
            Debug.LogError("Add Button not assigned!");
        }
    }

    void OnAddExpButtonClick()
    {
        characterStats.IncreaseExp(expToAdd);
        Debug.Log("Current Level: " + characterStats.characterLV);
        Debug.Log("Current Exp: " + characterStats.exp);
        Debug.Log("Required Exp for next level: " + characterStats.reqExp);
    }
}
