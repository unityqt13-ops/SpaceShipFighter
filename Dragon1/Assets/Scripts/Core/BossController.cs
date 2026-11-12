
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Configuration")]
    [SerializeField] private BossData bossData;

    // Cho phép các hệ thống khác đọc cấu hình Boss.
    public BossData Data => bossData;

    private void Awake()
    {
        // Kiểm tra đã gán dữ liệu Boss chưa.
        if (bossData == null)
        {
            Debug.LogError(
                "BossData has not been assigned!",
                this
            );

            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // Kiểm tra dữ liệu Boss đã được kết nối.
        Debug.Log(
            $"Boss Initialized: {bossData.BossName} | " +
            $"HP: {bossData.MaxHealth} | " +
            $"Move Speed: {bossData.MoveSpeed}",
            this
        );
    }
}
