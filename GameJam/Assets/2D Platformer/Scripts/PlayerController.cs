using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
        private float Score = 0;
        public TextMeshProUGUI scoreText;


        public GameObject startmenu;
        public GameObject gameoverScreen;
        public GameObject creditScreen;
        public GameObject enemyPrefab;
        public GameObject spikePrefab;


        void Start()
        {
            Time.timeScale = 0;
            gameoverScreen.SetActive(false);
            creditScreen.SetActive(false);
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            SpawnSpike();
            SpawnEnemy();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        private void SpawnSpike()
        {
            Vector3 spawnPosition = new Vector3(26, Random.Range(-3f, 3f), 0);
            Instantiate(spikePrefab, spawnPosition, Quaternion.identity);
        }

        private void SpawnEnemy()
        {
            Vector3 spawnPosition = new Vector3(26, Random.Range(-3f, 3f), 0);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }

        private void KillEnemy(GameObject enemy)
        {
            // Calculate the distance between the player and the enemy
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Calculate the score increase. The closer the player is to the enemy, the higher the score.
            // You can adjust the formula as needed to get the desired score increase.
            int scoreIncrease = Mathf.RoundToInt(100 / (distance + 1));

            // Increase the score
            Score += scoreIncrease;

            // Destroy the enemy
            Destroy(enemy);
        }

        void Update()
        {
            // Set Score
            scoreText.text = "Score: " + Score;
            
            // Player Jump
            if (Input.GetKey(KeyCode.Space))
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                // Destroy all enemies in the radius
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Enemy"))
                    {
                        KillEnemy(collider.gameObject); // Call the KillEnemy method here
                    }
                }

                // Set Animation
                animator.SetTrigger("Explode");
            }

            //Player Animation
            animator.SetBool("isJumping", !isGrounded);

            // Falling Animation
            if (rigidbody.velocity.y < 0)
            {
                animator.SetBool("isFalling", true);
            }
            else if (isGrounded)
            {
                animator.SetBool("isFalling", false);
            }

            isGrounded = Physics2D.OverlapCircle(feetPos.position, radius, whatIsGround);

            if (Input.GetKey(KeyCode.Space)) //&&isGrounded deleted
            {
                rigidbody.AddForce(transform.up * jumpForce);
            }

            //if (!isGrounded) animator.SetBool("isJumping", true); // Turn on jump animation
            else animator.SetBool("isJumping", false); // Turn off jump animation


            // Check if there are any spikes or enemies left in the game
            if (GameObject.FindGameObjectsWithTag("Spike").Length == 0)
            {
                SpawnSpike();
            }

            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                SpawnEnemy();
            }
        }

        private void CheckGround()
        {
            isGrounded = Physics2D.OverlapCircle(feetPos.transform.position, 0.2f, whatIsGround);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Spike") || collision.CompareTag("Enemy"))
            {
                gameoverScreen.SetActive(true);
                Time.timeScale = 0;
            }
        }

        public void StartGame()
        {
            startmenu = GameObject.FindGameObjectWithTag("Startmenu");
            Debug.Log("Start");
            startmenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void EnterCredit()
        {
            startmenu.SetActive(false);
            creditScreen.SetActive(true);
        }

        public void EscapeCredits()
        {
            startmenu.SetActive(true);
            creditScreen.SetActive(false);
        }

        public void GoToMenu()
        {
            startmenu.SetActive(true);
            gameoverScreen.SetActive(false);
            ResetGame();
        }

        public void Restart()
        {
            ResetGame();
            gameoverScreen = GameObject.FindGameObjectWithTag("GameOverScreen");
            Debug.Log("Restart");
            gameoverScreen.SetActive(false);
            Time.timeScale = 1;
        }

        public void ResetGame()
        {
            // Destroy all enemys and spikes
            GameObject[] spikes = GameObject.FindGameObjectsWithTag("Spike");
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject spike in spikes)
            {
                Destroy(spike);
            }

            foreach (GameObject enemy in enemys)
            {
                Destroy(enemy);
            }
            
            Score = 0;
            SpawnEnemy();
            SpawnSpike();
        }
    }
}