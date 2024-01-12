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
        public GameObject creditScreen; 
        public GameObject enemyPrefab;


        void Start()
        {
            Time.timeScale = 0;
            gameoverScreen.SetActive(false);
            creditScreen.SetActive(false);
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        private void SpawnEnemy()
        {
            // Definieren Sie die Position, an der Sie die Spikes erstellen möchten
            Vector3 spawnPosition = new Vector3(26, 0, 0); // Ändern Sie dies entsprechend

            // Erstellen Sie einige Spikes
            for (int i = 0; i < 5; i++)
            {
                // Berechnen Sie die Position für diesen Spike
                Vector3 position = spawnPosition + new Vector3(0, 2, 0); // Ändern Sie dies entsprechend

                // Instanziieren Sie das Spike-Prefab an der berechneten Position
                Instantiate(enemyPrefab, position, Quaternion.identity);
            }
        }

        void Update()
        {
            
            // Player Jump
            if (Input.GetKey(KeyCode.Space))
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SpawnEnemy();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                // Destroy all spikes in the radius
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Enemy"))
                    {
                        print("Enemy in radius");
                        Destroy(collider.gameObject);
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
            if (collision.CompareTag("Spike") || collision.CompareTag("Enemy"))
            {
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


        public void Restart()
        {
            gameoverScreen = GameObject.FindGameObjectWithTag("GameOverScreen");
            Debug.Log("Restart");
            gameoverScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
}