
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        if (bossHealth == null || healthSlider == null)
        {
            Debug.LogError("BossHealthBar is missing references!", this);
            enabled = false;
            return;
        }

        // Thiết lập thanh máu.
        healthSlider.minValue = 0f;
        healthSlider.maxValue = 1f;
        healthSlider.value = 1f;

        // Đăng ký sự kiện cập nhật máu.
        bossHealth.OnHealthChanged += UpdateHealthBar;
        bossHealth.OnBossDied += HideHealthBar;

        // Cập nhật máu ban đầu.
        UpdateHealthBar(
            bossHealth.CurrentHealth,
            bossHealth.MaxHealth
        );
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0f)
            return;

        healthSlider.value = currentHealth / maxHealth;
    }

    private void HideHealthBar()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged -= UpdateHealthBar;
            bossHealth.OnBossDied -= HideHealthBar;
        }
    }
}
