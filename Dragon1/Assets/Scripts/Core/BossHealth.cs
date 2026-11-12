
using System;
using UnityEngine;

[RequireComponent(typeof(BossController))]
public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject explosionPrefab;

    private BossController bossController;
    private bool isDead;

    // Các sự kiện để hệ thống khác đăng ký theo dõi.
    public event Action<float, float> OnHealthChanged;
    public event Action OnBossDied;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        bossController = GetComponent<BossController>();

        if (bossController.Data == null)
        {
            Debug.LogError("BossData is missing!", this);
            enabled = false;
            return;
        }

        // Lấy HP tối đa từ ScriptableObject.
        maxHealth = bossController.Data.MaxHealth;
        currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log(
            $"{bossController.Data.BossName} HP initialized: {currentHealth}",
            this
        );
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);

        Debug.Log(
            $"{bossController.Data.BossName} received {damage} damage. " +
            $"HP: {currentHealth}/{maxHealth}",
            this
        );

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log($"{bossController.Data.BossName} has been defeated!", this);

        OnBossDied?.Invoke();
        
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    // Hàm kiểm thử trong Unity Editor.
    [ContextMenu("TEST/Take 500 Damage")]
    private void TestTakeDamage()
    {
        TakeDamage(500f);
    }
    
}
