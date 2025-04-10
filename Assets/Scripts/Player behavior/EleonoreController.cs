using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EleonoreController : CharacterController
{
    public Transform GetTransform()
    {
        return transform;
    }
    

    
    Vector3 movement;
    


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

    

    
}
