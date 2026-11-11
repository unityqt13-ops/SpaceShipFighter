
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifetime = 5f;

    private Vector2 direction;
    private float moveSpeed;
    private float damage;

    public float Damage => damage;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 shootDirection, float speed, float projectileDamage)
    {
        direction = shootDirection.normalized;
        moveSpeed = speed;
        damage = projectileDamage;
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
}
