using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Singleton instance để có thể truy cập LevelController từ bất kỳ đâu
    public static LevelController instance;

    [Space(10)]
    // Tham chiếu đến script Experi encePickup (vật phẩm kinh nghiệm)
    public ExperiencePickup pickup;
    // Tham chiếu đến script VortexCoin (rương phần thưởng)
    public VortexCoin vortex;

    [Space(10)]
    // Điểm kinh nghiệm hiện tại của người chơi
    public int currentExperience;
    // Danh sách số điểm kinh nghiệm cần thiết để lên từng cấp
    public List<int> expLevels;
    // Cấp độ hiện tại của người chơi
    public int currentLevel = 1;
    // Tổng số cấp độ trong game
    public int levelCount = 100;
    // Danh sách vũ khí có thể nâng cấp khi lên cấp
    public List<Weapon> weaponsToUpgrade;
    // Nút mặc định được chọn khi hiển thị UI
    public GameObject defaultSelectedButton;

    // Số lượng kẻ địch đã bị người chơi tiêu diệt
    public int enemiesDefeated = 0;

    void Awake()
    {
        // Gán thể hiện instance để truy cập dễ dàng
        instance = this;
    }

    void Start()
    {
        // Đảm bảo danh sách expLevels có đủ cấp độ theo levelCount
        while (expLevels.Count < levelCount)
        {
            // Tăng dần lượng kinh nghiệm cần thiết theo cấp độ
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }

    // Phương thức nhận kinh nghiệm khi người chơi thu thập vật phẩm
    public void GetExp(int amountToGet)
    {
        // Cộng thêm điểm kinh nghiệm vào tổng số hiện tại
        currentExperience += amountToGet;

        // Kiểm tra nếu đủ kinh nghiệm để lên cấp
        if (currentExperience >= expLevels[currentLevel])
        {
            LevelUp(); // Gọi phương thức xử lý lên cấp
        }

        // Cập nhật giao diện hiển thị kinh nghiệm và cấp độ
        UIController.instance.UpdateExperience(currentExperience, expLevels[currentLevel], currentLevel);
    }

    // Phương thức sinh ra vật phẩm kinh nghiệm tại vị trí nhất định
    public void SpawnExp(Vector3 position, int expValue)
    {
        Debug.Log($"[EXP SPAWN] Tạo exp tại {position} với giá trị {expValue}");
        // Tạo vật phẩm kinh nghiệm tại vị trí chỉ định với giá trị exp cụ thể
        Instantiate(pickup, position, Quaternion.identity).experienValue = expValue;
    }

    // Phương thức sinh ra rương phần thưởng (VortexCoin) tại vị trí cụ thể
    public void SpawnChest(Vector3 position)
    {
        // Kiểm tra nếu rương chưa được sinh ra trước đó
        if (CharacterController.instance.isChestSpawned == false)
        {
            // Đánh dấu rằng rương đã được sinh ra
            CharacterController.instance.isChestSpawned = true;
            Instantiate(vortex, position, Quaternion.identity);
        }
    }

    // Phương thức xử lý khi người chơi lên cấp
    void LevelUp()
    {
        // Dừng phát âm thanh hiện tại
        SFXManager.instance.StopSFX(0);

        // Trừ đi số kinh nghiệm đã dùng để lên cấp
        currentExperience -= expLevels[currentLevel];

        // Tăng cấp độ của người chơi
        currentLevel++;

        // Đảm bảo cấp độ hiện tại không vượt quá cấp độ tối đa
        if (currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        // Dừng game và hiển thị bảng lựa chọn nâng cấp
        GameController.instance.gameActive = false;
        UIController.instance.levelUpPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);

        // Tạm dừng game bằng cách đặt timeScale về 0
        Time.timeScale = 0f;

        // Xóa danh sách vũ khí có thể nâng cấp trước đó
        weaponsToUpgrade.Clear();

        // Tạo danh sách vũ khí có sẵn để nâng cấp
        List<Weapon> availableWeapons = new List<Weapon>();
        availableWeapons.AddRange(CharacterController.instance.assignedWeapons);

        // Chọn một vũ khí trong danh sách để nâng cấp
        if (availableWeapons.Count > 0)
        {
            int selected = Random.Range(0, availableWeapons.Count);
            weaponsToUpgrade.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected);
        }

        // Nếu số lượng vũ khí hiện có chưa đạt tối đa, thêm vũ khí chưa được sử dụng
        if (CharacterController.instance.assignedWeapons.Count + CharacterController.instance.fullyLevelledWeapons.Count < CharacterController.instance.maxWeapons)
        {
            availableWeapons.AddRange(CharacterController.instance.unassignedWeapons);
        }

        // Chọn thêm các vũ khí khác để bổ sung vào danh sách nâng cấp
        for (int i = weaponsToUpgrade.Count; i < 3; i++)
        {
            if (availableWeapons.Count > 0)
            {
                int selected = Random.Range(0, availableWeapons.Count);
                weaponsToUpgrade.Add(availableWeapons[selected]);
                availableWeapons.RemoveAt(selected);
            }
        }

        // Cập nhật giao diện các nút nâng cấp vũ khí
        for (int i = 0; i < weaponsToUpgrade.Count; i++)
        {
            UIController.instance.levelUpButton[i].UpdateButtonDisplay(weaponsToUpgrade[i]);
        }

        // Bật hoặc tắt các nút dựa trên số lượng nâng cấp có thể thực hiện
        for (int i = 0; i < UIController.instance.levelUpButton.Length; i++)
        {
            if (i < weaponsToUpgrade.Count)
            {
                UIController.instance.levelUpButton[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.instance.levelUpButton[i].gameObject.SetActive(false);
            }
        }
    }
}
