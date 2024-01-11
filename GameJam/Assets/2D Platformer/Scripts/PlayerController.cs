using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;

        [HideInInspector] public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;
        private int coins;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Space)) //&&isGrounded deleted
            {
                rigidbody.AddForce(transform.up * jumpForce);
            }
            if (!isGrounded) animator.SetBool("isJumping", false); // Turn on jump animation
        }

        
        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Coin"))
            {
                Destroy(collision.gameObject);
                coins++;
            }

            if (collision.CompareTag("Spike"))
            {
                print("HIT SPIKE");
                //menu.SetActive(true);
                //condition.text = "You Lose!";
                //transform.position = new Vector2(-1f, 6f);
            }
        }
    }
}