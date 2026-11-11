
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAttackPattern
{
    Straight,
    Spread,
    Random
}

[RequireComponent(typeof(BossController))]
[RequireComponent(typeof(BossMovement))]
[RequireComponent(typeof(BossHealth))]
[RequireComponent(typeof(BossPhaseController))]
public class BossAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject[] projectilePrefabs = new GameObject[3];
    [SerializeField] private Transform attackPoint;

    [Header("Default Attack Pattern")]
    [SerializeField] private BossAttackPattern attackPattern =
        BossAttackPattern.Straight;

    [Header("Spread Settings")]
    [SerializeField] private int projectileCount = 5;
    [SerializeField] private float spreadAngle = 60f;

    private BossController bossController;
    private BossMovement bossMovement;
    private BossHealth bossHealth;
    private BossPhaseController bossPhaseController;
    private BossData bossData;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
        bossMovement = GetComponent<BossMovement>();
        bossHealth = GetComponent<BossHealth>();
        bossPhaseController = GetComponent<BossPhaseController>();
    }

    private void Start()
    {
        bossData = bossController.Data;

        if (bossData == null || attackPoint == null)
        {
            Debug.LogError(
                "BossAttack is missing configuration!",
                this
            );

            enabled = false;
            return;
        }

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        // Chờ Boss đến vị trí chiến đấu.
        while (!bossMovement.HasReachedPosition &&
               !bossHealth.IsDead)
        {
            yield return null;
        }

        while (!bossHealth.IsDead)
        {
            Shoot();

            yield return new WaitForSeconds(
                GetAttackInterval()
            );
        }
    }

    private BossPhaseData GetCurrentPhase()
    {
        return bossPhaseController != null
            ? bossPhaseController.CurrentPhase
            : null;
    }

    private float GetAttackInterval()
    {
        BossPhaseData phase = GetCurrentPhase();

        float interval = phase != null
            ? phase.AttackInterval
            : bossData.AttackInterval;

        return Mathf.Max(0.05f, interval);
    }

    // Lấy danh sách đạn của Phase hiện tại.
    // Nếu Phase chưa có cấu hình thì dùng danh sách mặc định.
    
    private List<GameObject> GetAvailableProjectiles()
    {
        BossPhaseData phase = GetCurrentPhase();

        List<GameObject> available = new List<GameObject>();

        // Ưu tiên lấy đạn từ Phase hiện tại
        if (phase != null && phase.ProjectilePrefabs != null)
        {
            foreach (GameObject prefab in phase.ProjectilePrefabs)
            {
                if (prefab != null && !available.Contains(prefab))
                {
                    available.Add(prefab);
                }
            }
        }

        // Nếu Phase không có đạn hợp lệ,
        // lấy danh sách đạn mặc định trong BossAttack
        if (available.Count == 0 && projectilePrefabs != null)
        {
            foreach (GameObject prefab in projectilePrefabs)
            {
                if (prefab != null && !available.Contains(prefab))
                {
                    available.Add(prefab);
                }
            }
        }

        return available;
    }


    // Chọn ngẫu nhiên một trong 3 loại đạn.
    private GameObject GetRandomProjectile()
    {
        List<GameObject> available = GetAvailableProjectiles();

        if (available.Count == 0)
        {
            Debug.LogError(
                "Boss has no projectile prefab assigned!",
                this
            );

            return null;
        }

        int randomIndex = Random.Range(0, available.Count);

        return available[randomIndex];
    }

    private void Shoot()
    {
        // Mỗi lượt tấn công chọn ngẫu nhiên một loại đạn.
        GameObject selectedProjectile = GetRandomProjectile();

        if (selectedProjectile == null)
            return;

        BossPhaseData phase = GetCurrentPhase();

        BossAttackPattern selectedPattern = phase != null
            ? phase.AttackPattern
            : attackPattern;

        // Random pattern: chọn Straight hoặc Spread.
        if (selectedPattern == BossAttackPattern.Random)
        {
            selectedPattern = Random.Range(0, 2) == 0
                ? BossAttackPattern.Straight
                : BossAttackPattern.Spread;
        }

        switch (selectedPattern)
        {
            case BossAttackPattern.Straight:
                ShootStraight(selectedProjectile);
                Debug.Log("Boss used Straight Shot!");
                break;

            case BossAttackPattern.Spread:
                ShootSpread(selectedProjectile);
                Debug.Log("Boss used Spread Shot!");
                break;
        }
    }

    private void ShootStraight(GameObject selectedProjectile)
    {
        CreateProjectile(Vector2.down, selectedProjectile);
    }

    private void ShootSpread(GameObject selectedProjectile)
    {
        if (projectileCount <= 0)
            return;

        if (projectileCount == 1)
        {
            CreateProjectile(Vector2.down, selectedProjectile);
            return;
        }

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + angleStep * i;

            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector2.down;

            CreateProjectile(direction, selectedProjectile);
        }
    }

    private void CreateProjectile(
        Vector2 direction,
        GameObject selectedPrefab)
    {
        GameObject projectile = Instantiate(
            selectedPrefab,
            attackPoint.position,
            Quaternion.identity
        );

        BossProjectile bossProjectile =
            projectile.GetComponent<BossProjectile>();

        if (bossProjectile == null)
        {
            Debug.LogError(
                "Projectile prefab is missing BossProjectile!",
                projectile
            );

            Destroy(projectile);
            return;
        }

        BossPhaseData phase = GetCurrentPhase();

        float damageMultiplier = phase != null
            ? phase.DamageMultiplier
            : 1f;

        float speedMultiplier = phase != null
            ? phase.ProjectileSpeedMultiplier
            : 1f;

        float finalDamage =
            bossData.ProjectileDamage * damageMultiplier;

        float finalSpeed =
            bossData.ProjectileSpeed * speedMultiplier;

        bossProjectile.Initialize(
            direction,
            finalSpeed,
            finalDamage
        );
    }
}
