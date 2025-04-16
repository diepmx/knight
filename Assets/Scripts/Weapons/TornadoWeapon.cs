using UnityEngine;

public class TornadoWeapon : MonoBehaviour
{
    private EnemyDamager damager; // X? lý sát th??ng
    private float speed; // T?c ?? di chuy?n c?a c?n l?c
    private float lifeTime; // Th?i gian t?n t?i c?a c?n l?c
    private float range; // Ph?m vi ?nh h??ng c?a c?n l?c

    private Vector3 moveDirection; // H??ng di chuy?n c?a c?n l?c

    public void Initialize(EnemyDamager damager, float speed, float lifeTime, float range)
    {
        this.damager = damager;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.range = range;

        // Ch?n h??ng di chuy?n ng?u nhiên
        moveDirection = Random.insideUnitCircle.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Di chuy?n c?n l?c
        transform.position += moveDirection * speed * Time.deltaTime;

        // C?p nh?t ph?m vi ?nh h??ng
        damager.transform.position = transform.position;
        damager.transform.localScale = Vector3.one * range;
    }
}
