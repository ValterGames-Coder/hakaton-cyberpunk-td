using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private TMP_Text _healthText;

    public void UpdateUI(float health, float fullHealth)
    {
        _bar.fillAmount = health / fullHealth;
        _healthText.text = $"{health} / {fullHealth}";
    } 
}