using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [Space(10)]
    // Xác định lượng sát thương gây ra cho kẻ địch.
    public float damageAmount = 5f;
    // Thời gian tồn tại của đối tượng sát thương trước khi bị hủy.
    public float lifeTime = 3f;
    // Tốc độ mà đối tượng sát thương phát triển đến kích thước mục tiêu.
    public float growSpeed = 5f;

    [Space(10)]
    public bool destroyParent;
    // Nếu true, kẻ địch sẽ bị đẩy lùi khi nhận sát thương.
    public bool shouldKnockBack;
    // Nếu true, nguồn sát thương sẽ bị hủy ngay sau khi va chạm lần đầu với kẻ địch.
    public bool destroyOnImpact;

    [Space(10)]
    // Bật hoặc tắt chức năng gây sát thương theo thời gian (DoT - Damage over Time).
    public bool damageOverTime;
    // Khoảng thời gian giữa các lần gây sát thương trong chế độ DoT.
    public float timeBetweenDamage;

    // Bộ đếm thời gian giữa các lần gây sát thương DoT.
    private float damageCounter;
    // Danh sách các kẻ địch hiện đang trong phạm vi sát thương DoT.
    private List<EnemyController> enemiesInRange = new List<EnemyController>();
    // Danh sách các boss hiện đang trong phạm vi sát thương DoT.
    private List<BossController> bossInRange = new List<BossController>();
    // Kích thước mục tiêu của đối tượng sát thương.
    private Vector3 targetSize;

    void Start()
    {
        // Khởi tạo kích thước mục tiêu và đặt kích thước ban đầu là 0.
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        // Phát triển kích thước đối tượng đến kích thước mục tiêu theo thời gian.
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

        // Giảm thời gian tồn tại của đối tượng và kiểm tra xem đã hết thời gian chưa.
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            // Bắt đầu thu nhỏ kích thước của đối tượng về 0.
            targetSize = Vector3.zero;

            // Kiểm tra xem đối tượng đã hoàn toàn thu nhỏ chưa.
            if (transform.localScale.x == 0f)
            {
                // Hủy đối tượng này và tùy chọn hủy cả đối tượng cha nếu cần.
                Destroy(gameObject);
                if (destroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        // Áp dụng sát thương theo thời gian nếu chế độ DoT được bật.
        if (damageOverTime)
        {
            // Đếm thời gian và áp dụng sát thương cho tất cả kẻ địch trong phạm vi.
            damageCounter -= Time.deltaTime;
            if (damageCounter <= 0)
            {
                damageCounter = timeBetweenDamage;
                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] != null)
                    {
                        enemiesInRange[i].TakeDamage(damageAmount, shouldKnockBack);
                    }
                    else
                    {
                        // Loại bỏ bất kỳ tham chiếu null nào khỏi danh sách.
                        enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
                for (int b = 0; b < bossInRange.Count; b++)
                {
                    if (bossInRange[b] != null)
                    {
                        bossInRange[b].TakeDamage(damageAmount, shouldKnockBack);
                    }
                    else
                    {
                        bossInRange.RemoveAt(b);
                        b--;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Xử lý va chạm để gây sát thương ngay lập tức hoặc thêm vào danh sách DoT.
        if (!damageOverTime)
        {
            // Gây sát thương ngay lập tức khi va chạm.
            if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            {
                if (collision.CompareTag("Boss"))
                {
                    // Gây sát thương cho boss.
                    collision.GetComponent<BossController>().TakeDamage(damageAmount, shouldKnockBack);
                }
                else
                {
                    // Gây sát thương cho kẻ địch thường.
                    collision.GetComponent<EnemyController>().TakeDamage(damageAmount, shouldKnockBack);
                }
                if (destroyOnImpact)
                {
                    // Hủy đối tượng sát thương sau va chạm.
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            // Thêm kẻ địch hoặc boss vào danh sách DoT.
            if (collision.CompareTag("Enemy"))
            {
                enemiesInRange.Add(collision.GetComponent<EnemyController>());
            }
            else if (collision.CompareTag("Boss"))
            {
                bossInRange.Add(collision.GetComponent<BossController>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Loại bỏ kẻ địch hoặc boss khỏi danh sách DoT khi rời khỏi phạm vi sát thương.
        if (damageOverTime)
        {
            if (collision.CompareTag("Enemy"))
            {
                enemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
            else if (collision.CompareTag("Boss"))
            {
                bossInRange.Remove(collision.GetComponent<BossController>());
            }
        }
    }
}
