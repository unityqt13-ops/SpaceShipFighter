
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewBossPhase",
    menuName = "Space Fight/Boss/Boss Phase Data"
)]
public class BossPhaseData : ScriptableObject
{
    [Header("Phase Information")]
    [SerializeField] private string phaseName;

    [TextArea(2, 4)]
    [SerializeField] private string description;

    [Header("Health Threshold")]
    [Range(0f, 1f)]
    [SerializeField] private float healthThreshold = 1f;

    [Header("Attack Settings")]
    [SerializeField] private BossAttackPattern attackPattern =
        BossAttackPattern.Straight;

    [SerializeField] private float attackInterval = 2f;

    [Header("Projectile Prefabs - 3 Types")]
    [SerializeField] private GameObject[] projectilePrefabs = new GameObject[3];

    [Header("Combat Multipliers")]
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float projectileSpeedMultiplier = 1f;

    public string PhaseName => phaseName;
    public string Description => description;
    public float HealthThreshold => healthThreshold;
    public BossAttackPattern AttackPattern => attackPattern;
    public float AttackInterval => attackInterval;

    public GameObject[] ProjectilePrefabs => projectilePrefabs;

    public float DamageMultiplier => damageMultiplier;
    public float ProjectileSpeedMultiplier => projectileSpeedMultiplier;
}
