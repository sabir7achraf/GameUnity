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

    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

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
        // Met à jour les variables du blend tree
        animator.SetFloat("Look X", input.x);
        animator.SetFloat("Look Y", input.y);

        // Met à jour la vitesse de déplacement
        animator.SetFloat("Speed", speed);

        // Handle Hit Trigger
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Hit");
        }

        // Handle Launch Trigger
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Launch");
        }
    }

    public void ChangeHealth(int amount)
    {
        // Changer la santé actuelle en ajoutant le montant et en la limitant entre 0 et maxHealth
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        // Afficher la nouvelle valeur de la santé dans la console pour le débogage
        Debug.Log(currentHealth + "/" + maxHealth);
        
        // Si la santé atteint 0, déclencher des actions supplémentaires (par exemple, la mort du joueur)
        if (currentHealth <= 0)
        {
            // Par exemple, jouer une animation de mort
            animator.SetTrigger("Die");

            // Autres actions possibles :
            // - Désactiver les contrôles du joueur
            // - Afficher un écran de game over
            // - Réinitialiser le niveau
            // - Etc.
        }
    }
}
