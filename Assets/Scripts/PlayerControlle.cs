using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator; // Référence à l'Animator
    public float moveSpeed = 0.05f; // Vitesse de déplacement configurable
    public UIHandler uiHandler; // Référence à UIHandler

    private Vector2 input;
    private float speed;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Obtenir l'Animator s'il n'est pas assigné
        }
        
        currentHealth = maxHealth/2; // Initialiser la santé actuelle à la santé maximale

        // Mettre à jour l'interface utilisateur au début du jeu
        if (uiHandler != null)
        {
            uiHandler.SetHealthValue((float)currentHealth / maxHealth);
        }
        else
        {
            Debug.LogError("UIHandler not assigned in the inspector.");
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
      
        animator.SetFloat("Look X", input.x);
        animator.SetFloat("Look Y", input.y);

   
        animator.SetFloat("Speed", speed);

        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Hit");
        }

       
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Launch");
        }
    }

    public void ChangeHealth(int amount)
    {
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        
        Debug.Log(currentHealth + "/" + maxHealth);
        
      
        if (uiHandler != null)
        {
            uiHandler.SetHealthValue((float)currentHealth / maxHealth);
        }
        else
        {
            Debug.LogError("UIHandler not assigned in the inspector.");
        }

       
        if (currentHealth <= 0)
        {
            
            animator.SetTrigger("Die");

     
        }
    }
}
