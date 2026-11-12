
using UnityEngine;

[CreateAssetMenu(   
    fileName = "NewBossData",
    menuName = "Space Fight/Boss/Boss Data"
                )
]
public class BossData : ScriptableObject
{
    [Header("Basic Information")]
    [SerializeField] private string bossName;
    [TextArea(2, 4)]
    [SerializeField] private string description;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 5000f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Combat Settings")]
    [SerializeField] private float contactDamage = 30f;
    [SerializeField] private float projectileDamage = 20f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float attackInterval = 2f;

    // Public properties để các script khác đọc dữ liệu.
    public string BossName => bossName;
    public string Description => description;
    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float ContactDamage => contactDamage;
    public float ProjectileDamage => projectileDamage;
    public float ProjectileSpeed => projectileSpeed;
    public float AttackInterval => attackInterval;
}
