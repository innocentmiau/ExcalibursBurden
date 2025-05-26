using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private RectTransform healthBar;
    [SerializeField] private TMP_Text healthText;
    
    private Healthy _healthy;

    public void Setup(Character character)
    {
        _healthy = new Healthy(character.maxHealth);
        UpdateHealthBar();
    }

    public void TakeDamage(float value)
    {
        if (_healthy.TakeDamage(value))
        {
            if (gameObject.name.Equals("Kay")) FindFirstObjectByType<KayDuelScene>().KayLost();
            if (gameObject.CompareTag("Player")) SceneManager.LoadSceneAsync("MainMenu");
            Destroy(gameObject);
            return;
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null) healthBar.localScale = new Vector3(_healthy.Health / _healthy.MaxHealth, 1f, 1f);
        if (healthText != null) healthText.text = $"{_healthy.Health} <color=#FFFFFF66>/ {_healthy.MaxHealth}";
    }
    
}