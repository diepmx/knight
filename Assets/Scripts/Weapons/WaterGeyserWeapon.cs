using UnityEngine;

public class WaterGeyserWeapon : MonoBehaviour
{
    private EnemyDamager damager; // X? lý sát th??ng
    private float lifeTime; // Th?i gian t?n t?i c?a c?t n??c
    private float range; // Ph?m vi ?nh h??ng c?a c?t n??c

    public void Initialize(EnemyDamager damager, float lifeTime, float range)
    {
        this.damager = damager;
        this.lifeTime = lifeTime;
        this.range = range;

        // C?p nh?t ph?m vi ?nh h??ng
        damager.transform.localScale = Vector3.one * range;

        // H?y c?t n??c sau khi h?t th?i gian t?n t?i
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // C?p nh?t v? trí c?a damager ?? trùng v?i v? trí c?a c?t n??c
        damager.transform.position = transform.position;
    }
}
