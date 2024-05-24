using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   // Public variables
   public float speed;
   public bool vertical;
   public float changeTime = 3.0f;
   public LayerMask unwalkableMask;

   // Private variables
   Rigidbody2D rigidbody2d;
   Animator animator;
   float timer;
   int direction = 1;

   bool aggressive = true;

   // Start is called before the first frame update
   void Start()
   {
       rigidbody2d = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>(); // Get the Animator component
       timer = changeTime;
   }

   // Update is called every frame
   void Update()
   {
       timer -= Time.deltaTime;

       if (timer < 0)
       {
           direction = -direction;
           timer = changeTime;
           UpdateAnimator();
       }
   }

   // FixedUpdate has the same call rate as the physics system
   void FixedUpdate()
   {
       Vector2 position = rigidbody2d.position;

       if (vertical)
       {
           position.y = position.y + speed * direction * Time.deltaTime;
       }
       else
       {
           position.x = position.x + speed * direction * Time.deltaTime;
       }

       // Check if the new position is in a non-walkable area
       if (IsPositionWalkable(position))
       {
           rigidbody2d.MovePosition(position);
       }
       else
       {
           direction = -direction; // Reverse direction if the new position is not walkable
           UpdateAnimator();
       }
   }

   void UpdateAnimator()
   {
       bool movingBackward = direction > 0;
       animator.SetBool("Back", movingBackward);
   }

   bool IsPositionWalkable(Vector2 position)
   {
       Collider2D hitCollider = Physics2D.OverlapCircle(position, 0.1f, unwalkableMask); // Utilisation d'un petit rayon pour vérifier une zone ponctuelle
       return hitCollider == null;
   }

      public void Fix()
   {
       aggressive = false;
       GetComponent<Rigidbody2D>().simulated = false;
       Destroy(gameObject);
   }
      void OnCollisionEnter2D(Collision2D other)
   {
       PlayerController player = other.gameObject.GetComponent<PlayerController>();


       if (player != null)
       {
           player.ChangeHealth(-1);
       }
   }
   
}
