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
        public LayerMask whatIsGround;
        public Transform feetPos;
        public float radius;
        
        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;
        private int coins;

        public GameObject startmenu;
        public GameObject gameoverScreen;
        public GameObject playScreen;


        void Start()
        {
            Time.timeScale = 0;
            gameoverScreen.SetActive(false);

            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            // Player Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidbody.velocity = Vector2.up * jumpForce;
            }
            
            //Player Animation
            animator.SetBool("isJumping", !isGrounded);
            
            isGrounded = Physics2D.OverlapCircle(feetPos.position, radius, whatIsGround);

            
            if (Input.GetKey(KeyCode.Space)) //&&isGrounded deleted
            {
                rigidbody.AddForce(transform.up * jumpForce);
            }

            //if (!isGrounded) animator.SetBool("isJumping", true); // Turn on jump animation
            else animator.SetBool("isJumping", false); // Turn off jump animation
        }

        private void CheckGround()
        {
            isGrounded = Physics2D.OverlapCircle(feetPos.transform.position, 0.2f, whatIsGround);
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
                print(coins + coins.ToString());
            }


            if (collision.CompareTag("Spike"))
            {
                print("HIT SPIKE");
                gameoverScreen.SetActive(true);
                Time.timeScale = 0;
                //condition.text = "You Lose!";
                //transform.position = new Vector2(-1f, 6f);
            }
        }
        
        public void StartGame()
        {
            startmenu = GameObject.FindGameObjectWithTag("Startmenu");
            Debug.Log("Start");
            startmenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void Restart()
        {
            gameoverScreen = GameObject.FindGameObjectWithTag("GameOverScreen");
            Debug.Log("Restart");
            gameoverScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
}