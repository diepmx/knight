using UnityEngine;

public class MoltenspearProjectile : MonoBehaviour
{
    private EnemyDamager damager; // X? l� s�t th??ng
    private float speed; // T?c ?? bay c?a ng?n gi�o
    private float range; // Ph?m vi bay c?a ng?n gi�o
    private float damage; // S�t th??ng c?a ng?n gi�o

    private Vector3 startPosition; // V? tr� b?t ??u c?a ng?n gi�o

    public LayerMask whatIsEnemy; // Added this field to define enemy layers

    public void Initialize(EnemyDamager damager, float speed, float range, float damage)
    {
        this.damager = damager;
        this.speed = speed;
        this.range = range;
        this.damage = damage;

        startPosition = transform.position;

        // H?y ng?n gi�o sau khi v??t qu� ph?m vi
        Destroy(gameObject, range / speed);
    }

    void Update()
    {
        // Di chuy?n ng?n gi�o v? ph�a tr??c
        transform.position += transform.up * speed * Time.deltaTime;

        // Ki?m tra n?u ng?n gi�o v??t qu� ph?m vi
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsEnemy) != 0) // Updated to use the new field
        {
            // G�y s�t th??ng cho k? ??ch
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // H?y ng?n gi�o sau khi va ch?m
            Destroy(gameObject);
        }
    }
}
