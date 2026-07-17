using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void Start()
    {
        UpdateHealthBar(
            playerHealth.CurrentHealth,
            playerHealth.MaxHealth);
    }

    private void UpdateHealthBar(int current, int max)
    {
        fillImage.fillAmount =
            (float)current / max;
    }
}