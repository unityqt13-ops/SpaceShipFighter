    
using UnityEngine;

[RequireComponent(typeof(BossHealth))]
public class BossPhaseController : MonoBehaviour
{
    [Header("Phase Configuration")]
    [SerializeField] private BossPhaseData[] phases;

    private BossHealth bossHealth;

    private int currentPhaseIndex = -1;

    public BossPhaseData CurrentPhase { get; private set; }

    public int CurrentPhaseIndex => currentPhaseIndex;

    private void Awake()
    {
        bossHealth = GetComponent<BossHealth>();
    }

    private void OnEnable()
    {
        if (bossHealth == null)
            bossHealth = GetComponent<BossHealth>();

        bossHealth.OnHealthChanged += CheckPhase;
    }

    private void Start()
    {
        if (phases == null || phases.Length == 0)
        {
            Debug.LogError("Boss Phase Data is missing!", this);
            enabled = false;
            return;
        }

        // Kiểm tra thứ tự ngưỡng HP.
        for (int i = 0; i < phases.Length; i++)
        {
            if (phases[i] == null)
            {
                Debug.LogError(
                    $"Phase at index {i} is missing!",
                    this
                );

                enabled = false;
                return;
            }

            if (i > 0 &&
                phases[i].HealthThreshold >= phases[i - 1].HealthThreshold)
            {
                Debug.LogError(
                    "Phase thresholds must be arranged from highest to lowest!",
                    this
                );

                enabled = false;
                return;
            }
        }

        // Kiểm tra HP hiện tại để kích hoạt Phase đầu tiên.
        CheckPhase(
            bossHealth.CurrentHealth,
            bossHealth.MaxHealth
        );
    }

    private void OnDisable()
    {
        if (bossHealth != null)
            bossHealth.OnHealthChanged -= CheckPhase;
    }

    private void CheckPhase(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0f || bossHealth.IsDead)
            return;

        float healthPercent = currentHealth / maxHealth;

        // Tìm Phase phù hợp với phần trăm HP hiện tại.
        int newPhaseIndex = -1;

        for (int i = phases.Length - 1; i >= 0; i--)
        {
            if (healthPercent <= phases[i].HealthThreshold)
            {
                newPhaseIndex = i;
                break;
            }
        }

        // Không chuyển ngược Phase khi Boss được hồi máu.
        if (newPhaseIndex <= currentPhaseIndex)
            return;

        currentPhaseIndex = newPhaseIndex;
        CurrentPhase = phases[currentPhaseIndex];

        Debug.Log(
            $"Boss entered Phase {currentPhaseIndex + 1}: " +
            $"{CurrentPhase.PhaseName} | " +
            $"HP: {healthPercent * 100f:F1}%",
            this
        );
    }
}
