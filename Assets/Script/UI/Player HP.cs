using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject hpTextObject;

    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {
        hpText = hpTextObject.GetComponent<TextMeshProUGUI>(); // Assuming TextMeshPro is used for hpText
    }

    // Update is called once per frame
    void Update()
    {
        // Update UI elements here if needed
    }

    public void UpdateCharacterStats(CharacterStats characterStats)
    {
        if (characterStats != null)
        {
            // Update slider min/max values based on CharacterStats
            slider.minValue = 0;
            slider.maxValue = characterStats.maxHealthPoint;

            // Update slider value
            slider.value = characterStats.currentHealthPoint;

            // Update fill color based on gradient
            fill.color = gradient.Evaluate(slider.normalizedValue);

            // Update HP text
            if (hpText != null)
            {
                hpText.text = $"HP: {characterStats.currentHealthPoint} / {characterStats.maxHealthPoint}";
            }
        }
        else
        {
            Debug.LogError("CharacterStats is null. Cannot update HP UI.");
        }
    }
}
