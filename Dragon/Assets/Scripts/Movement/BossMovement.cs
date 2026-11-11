
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BossController))]
public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 battlePosition = new Vector2(0, 3);

    [Header("Landing Settings")]
    [SerializeField] private float landingWaitTime = 1f;

    [Header("Automatic Flying")]
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float flyHeight = 0.3f;
    [SerializeField] private float flySpeed = 1.5f;
    [SerializeField] private float flyHeightSpeed = 2f;

    private BossController bossController;

    private float moveSpeed;
    private bool hasReachedPosition;
    private bool isFlying;

    private Vector3 flightCenter;
    private float flightStartTime;

    public bool HasReachedPosition => hasReachedPosition;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
    }

    private void Start()
    {
        if (bossController.Data == null)
        {
            Debug.LogError("BossData is missing!", this);
            enabled = false;
            return;
        }

        // Lấy tốc độ đáp xuống từ BossData.
        moveSpeed = bossController.Data.MoveSpeed;

        StartCoroutine(EnterBattle());
    }

    private IEnumerator EnterBattle()
    {
        // GIAI ĐOẠN 1: Đáp xuống vị trí chiến đấu.
        Vector3 targetPosition = new Vector3(
            battlePosition.x,
            battlePosition.y,
            transform.position.z
        );

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;

        Debug.Log("Boss has landed!", this);

        // GIAI ĐOẠN 2: Nghỉ một chút.
        yield return new WaitForSeconds(landingWaitTime);

        // GIAI ĐOẠN 3: Bắt đầu bay tự động.
        flightCenter = transform.position;
        flightStartTime = Time.time;

        isFlying = true;
        hasReachedPosition = true;

        Debug.Log("Boss starts flying and attacking!", this);
    }

    private void Update()
    {
        if (!isFlying)
            return;

        FlyAutomatically();
    }

    private void FlyAutomatically()
    {
        float elapsedTime = Time.time - flightStartTime;

        // Bay qua lại trái và phải.
        float x = flightCenter.x +
            Mathf.Sin(elapsedTime * flySpeed) * moveDistance;

        // Nhấp nhô lên xuống.
        float y = flightCenter.y +
            Mathf.Sin(elapsedTime * flyHeightSpeed) * flyHeight;

        transform.position = new Vector3(
            x,
            y,
            flightCenter.z
        );
    }
}
