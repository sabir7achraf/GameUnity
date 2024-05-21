using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator; // Référence à l'Animator
    public float moveSpeed = 0.05f; // Vitesse de déplacement configurable

    private Vector2 input;
    private float speed;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Obtenir l'Animator s'il n'est pas assigné
        }
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void HandleMovement()
    {
        float horizontal = 0.0f;
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1.0f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1.0f;
        }

        float vertical = 0.0f;
        if (Keyboard.current.upArrowKey.isPressed)
        {
            vertical = 1.0f;
        }
        else if (Keyboard.current.downArrowKey.isPressed)
        {
            vertical = -1.0f;
        }

        input = new Vector2(horizontal, vertical);
        speed = input.magnitude;

        Vector2 position = transform.position;
        position.x += moveSpeed * horizontal;
        position.y += moveSpeed * vertical;
        transform.position = position;
    }

    void HandleAnimation()
    {
        animator.SetFloat("Speed", speed);

        // Handle Hit Trigger
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Hit");
        }

        // Handle Launch Trigger
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Launch");
        }
    }
}
