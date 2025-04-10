using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;
    public static CharacterController SelectedCharacter { get; private set; }

    public void SelectThisCharacter()
    {
        SelectedCharacter = this;
        
    }
    public static CharacterController ActiveCharacter { get; private set; }

    protected virtual void OnEnable()
    {
        ActiveCharacter = this;
    }

    protected virtual void OnDisable()
    {
        if (ActiveCharacter == this)
            ActiveCharacter = null;
    }

    protected virtual void Awake()
    {
        instance = this;
    }

    public float playerDistance;

    // Dùng để kiểm tra trạng thái rương
    public bool isChestClosed = true;
    public bool isChestSpawned = false;

    // Phạm vi nhặt vật phẩm
    public float pickupRange = 50f;
    public float pickupRangeMultiplier = 1.1f;

    // Tốc độ di chuyển
    public float speed = 3f;
    public float speedMultiplier = 1.1f;

    [Space(10)]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public List<ParticleSystem> footParticles;

    [Space(10)]
    public int maxWeapons = 3;
    public List<Weapon> unassignedWeapons;
    public List<Weapon> assignedWeapons;
    public List<Weapon> listWeapons;

    [HideInInspector]
    public List<Weapon> fullyLevelledWeapons = new List<Weapon>();
    [HideInInspector]
    public List<DialogueTrigger> dialogueTriggers = new List<DialogueTrigger>();

    // ====== Các hàm hỗ trợ vũ khí và nâng cấp ======
    public void AddWeapon(Weapon weaponToAdd)
    {
        weaponToAdd.gameObject.SetActive(true);
        assignedWeapons.Add(weaponToAdd);

        if (weaponToAdd.tag != "PlayerUpdate")
        {
            listWeapons.Add(weaponToAdd);
        }

        unassignedWeapons.Remove(weaponToAdd);
    }

    public void SpeedLevelUp()
    {
        speed *= speedMultiplier;
    }

    public void PickupRangeLevelUp()
    {
        pickupRange *= pickupRangeMultiplier;
    }
}
