using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public static DamageController instance;

    [Space(10)]
    // Canvas nơi các số sát thương sẽ xuất hiện
    public Transform numberCanvas;
    // Prefab của số sát thương để hiển thị
    public DamageNumber numberToSpawn;

    // Danh sách chứa các đối tượng DamageNumber để tái sử dụng (Object Pooling)
    private List<DamageNumber> numberPool = new List<DamageNumber>();

    private void Awake()
    {
        // Gán instance để sử dụng singleton
        instance = this;
    }

    // Phương thức tạo số sát thương tại một vị trí cụ thể
    public void SpawnDamage(float damageAmount, Vector3 location)
    {
        // Làm tròn sát thương thành số nguyên
        int rounded = Mathf.RoundToInt(damageAmount);

        // Lấy một đối tượng DamageNumber từ danh sách tái sử dụng
        DamageNumber newDamage = GetFromPool();

        // Thiết lập giá trị sát thương cho số vừa lấy
        newDamage.Setup(rounded);
        newDamage.gameObject.SetActive(true);

        // Đặt vị trí hiển thị số sát thương
        newDamage.transform.position = location;
    }

    // Lấy một đối tượng DamageNumber từ danh sách tái sử dụng
    public DamageNumber GetFromPool()
    {
        DamageNumber numberToOutput = null;

        // Nếu danh sách rỗng, tạo mới một đối tượng
        if (numberPool.Count == 0)
        {
            numberToOutput = Instantiate(numberToSpawn, numberCanvas);
        }
        else
        {
            // Lấy một đối tượng có sẵn trong danh sách
            numberToOutput = numberPool[0];
            numberPool.RemoveAt(0);
        }

        return numberToOutput;
    }

    // Đưa một đối tượng DamageNumber trở lại danh sách tái sử dụng
    public void PlaceInPool(DamageNumber numberToPlace)
    {
        // Tắt đối tượng để không hiển thị trên màn hình
        numberToPlace.gameObject.SetActive(false);

        // Thêm đối tượng vào danh sách để tái sử dụng sau này
        numberPool.Add(numberToPlace);
    }
}
