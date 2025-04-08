using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArchController : CharacterController
{
    public static ArchController instance;

    [Space(10)]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public List<ParticleSystem> footParticles;

    [Space(10)]
    public float speed = 3f;
    public float speedMultiplier = 1.1f;

    [Space(10)]
    public float pickupRange = 1.5f;
    public float pickupRangeMultiplier = 1.1f;

    [Space(10)]
    public float playerDistance;

    [Space(10)]
    public int maxWeapons = 3;
    public List<Weapon> unassignedWeapons;
    public List<Weapon> assignedWeapons;
    public List<Weapon> listWeapons;

    [HideInInspector]
    public List<Weapon> fullyLevelledWeapons = new List<Weapon>();

    [HideInInspector]
    public bool isChestClosed = true;
    [HideInInspector]
    public bool isChestSpawned = false;

    [HideInInspector]
    public List<DialogueTrigger> dialogueTriggers = new List<DialogueTrigger>();

    Vector3 movement;
    Vector2 lastMovementDirection = Vector2.down; // Hướng cuối cùng khi idle

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }
    }

    private void OnMovement(InputValue value)
    {
        Vector2 inputMovement = value.Get<Vector2>();
        movement = new Vector3(inputMovement.x, inputMovement.y, 0);

        // Nếu có di chuyển, cập nhật hướng cuối cùng
        if (inputMovement.magnitude > 0)
        {
            lastMovementDirection = inputMovement.normalized;
        }

        // Lật nhân vật dựa trên hướng di chuyển
        if (inputMovement.x != 0)
        {
            spriteRenderer.flipX = inputMovement.x < 0;
        }

        // Cập nhật thông số cho Animator (nếu cần)
    }

    public void OnOpenDialogue()
    {
        foreach (DialogueTrigger trigger in dialogueTriggers)
        {
            if (trigger.isInRange && !DialogueManager.instance.isDialogueOpen)
            {
                trigger.TriggerDialogue();
            }
        }
    }

    public void OnNextSentence()
    {
        DialogueManager.instance.DisplayNextSentence();
    }

    private void FixedUpdate()
    {
        transform.position += movement * speed * Time.fixedDeltaTime;
        playerDistance += movement.magnitude * speed * Time.fixedDeltaTime;
        Running();
    }

    private void Running()
    {
        foreach (ParticleSystem particles in footParticles)
        {
            if (movement.magnitude > 0)
            {
                if (!particles.isPlaying)
                {
                    particles.Play();
                    SFXManager.instance.PlaySFX(0);
                }
            }
            else
            {
                if (particles.isPlaying)
                {
                    particles.Stop();
                    SFXManager.instance.StopSFX(0);
                }
            }
        }
    }

    public void AddWeapon(int weaponNumber)
    {
        if (weaponNumber < unassignedWeapons.Count)
        {
            if (unassignedWeapons[weaponNumber].tag == "PlayerUpdate")
            {
                for (int i = 0; i < unassignedWeapons.Count; i++)
                {
                    if (i != weaponNumber && unassignedWeapons[i].tag != "PlayerUpdate")
                    {
                        weaponNumber = i;
                        break;
                    }
                }
            }

            assignedWeapons.Add(unassignedWeapons[weaponNumber]);
            listWeapons.Add(unassignedWeapons[weaponNumber]);
            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

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
