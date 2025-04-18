﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController
{
    public Transform GetTransform()
    {
        return transform;
    }
  

   
    Vector3 movement;
    Vector2 lastMovementDirection = Vector2.down; // Hướng cuối cùng khi idle

  

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

        // Cập nhật thông số cho Animator (Blend Tree)
        animator.SetFloat("Horizontal", lastMovementDirection.x);
        animator.SetFloat("Vertical", lastMovementDirection.y);
        animator.SetFloat("Speed", inputMovement.magnitude);
    }


    public void OnOpenDialogue()
    {
        if (dialogueTriggers == null) return;

        // Dùng vòng for ngược để có thể xóa phần tử null trong danh sách
        for (int i = dialogueTriggers.Count - 1; i >= 0; i--)
        {
            DialogueTrigger trigger = dialogueTriggers[i];

            // Nếu null thì xóa ra khỏi danh sách
            if (trigger == null)
            {
                dialogueTriggers.RemoveAt(i);
                continue;
            }

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
