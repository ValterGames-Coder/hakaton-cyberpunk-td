using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    [SerializeField] private HealthUI _ui;
    [SerializeField] private GameObject _winPanel;
    private GameManager _gameManager;
    [HideInInspector] public float fullHealth;
    public event Action OnDamage;
    public event Action OnDied;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        fullHealth = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(_ui != null)
            _ui.UpdateUI(health, fullHealth);
        OnDamage?.Invoke();
        if (health <= 0)
        {
            health = 0;
            OnDied?.Invoke();
            if (_winPanel != null)
            {
                health = 0;
                _gameManager.Finish();
                _winPanel.SetActive(true);
            }
        }
    }
}
