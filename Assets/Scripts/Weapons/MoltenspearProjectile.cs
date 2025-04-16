using UnityEngine;

public class MoltenspearProjectile : MonoBehaviour
{
    private EnemyDamager damager; // X? lý sát th??ng
    private float speed; // T?c ?? bay c?a ng?n giáo
    private float range; // Ph?m vi bay c?a ng?n giáo
    private float damage; // Sát th??ng c?a ng?n giáo

    private Vector3 startPosition; // V? trí b?t ??u c?a ng?n giáo

    public LayerMask whatIsEnemy; // Added this field to define enemy layers

    public void Initialize(EnemyDamager damager, float speed, float range, float damage)
    {
        this.damager = damager;
        this.speed = speed;
        this.range = range;
        this.damage = damage;

        startPosition = transform.position;

        // H?y ng?n giáo sau khi v??t quá ph?m vi
        Destroy(gameObject, range / speed);
    }

    void Update()
    {
        // Di chuy?n ng?n giáo v? phía tr??c
        transform.position += transform.up * speed * Time.deltaTime;

        // Ki?m tra n?u ng?n giáo v??t quá ph?m vi
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsEnemy) != 0) // Updated to use the new field
        {
            // Gây sát th??ng cho k? ??ch
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // H?y ng?n giáo sau khi va ch?m
            Destroy(gameObject);
        }
    }
}
