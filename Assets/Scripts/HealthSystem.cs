using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private RectTransform healthBar;
    [SerializeField] private TMP_Text healthText;
    
    private Healthy _healthy;

    public void Setup(Character character)
    {
        _healthy = new Healthy(character.maxHealth);
    }

    public void TakeDamage(float value)
    {
        _healthy.TakeDamage(value);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null) healthBar.localScale = new Vector3(_healthy.Health / _healthy.MaxHealth, 1f, 1f);
        if (healthText != null) healthText.text = $"{_healthy.Health} <color=#FFFFFF66>/ {_healthy.MaxHealth}";
    }
    
}