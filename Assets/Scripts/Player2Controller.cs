using TMPro;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    
    public float Speed;
    public float FloatSpeed;
    public float JumpForce;
    public float escapeAmount = 0f;
    
    public GameObject escapeBar;
    public GameObject escapeBarFill;
    public GameObject projectilePrefab;
    public GameObject bubbled;
    public Transform shootPoint;  

    public bool isJumping = false;
    public bool doubleJump;

    private int timesBubbled = 0;
    public TextMeshProUGUI bubbledCount;
    public TextMeshProUGUI bubbledCountSum;
    public Animator anim;

    public float shotDelay;  // Delay between shots in seconds
    private float lastShotTime = 0f;  // Time of the last shot

    private bool isStunned = false;

    private Rigidbody2D rig;

    public GameController gc;
    
    public CoinSpawner coinSpawner; // Reference to the CoinSpawner

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rig = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        bubbledCount.text = timesBubbled.ToString();
        bubbledCountSum.text = timesBubbled.ToString();

        if(!isStunned)
        {
            Move();
            Jump();
            Shoot();
        }
        else
        {
            FillBar();
        }
        

        if(escapeAmount == 0)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0f);
        }
        else if(escapeAmount == 1)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.1f);
        }
        else if(escapeAmount == 2)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.2f);
        }
        else if(escapeAmount == 3)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.3f);
        }
        else if(escapeAmount == 4)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.4f);
        }
        else if(escapeAmount == 5)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.5f);
        }
        else if(escapeAmount == 6)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.6f);
        }
        else if(escapeAmount == 7)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.7f);
        }
        else if(escapeAmount == 8)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.8f);
        }
        else if(escapeAmount == 9)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 0.9f);
        }
        else if(escapeAmount == 10)
        {
            escapeBarFill.transform.localScale = new Vector2(0.05f, 1f);
            FreePlayer();
            escapeAmount = 0;
        }
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal_P2"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        if(Input.GetAxis("Horizontal_P2") > 0f)
        {
            anim.SetBool("ringoWalk", true);
            anim.SetBool("ringoIdle", false);
            transform.eulerAngles = new Vector3(0f,180f,0f);
        }

        if(Input.GetAxis("Horizontal_P2") < 0f)
        {
            anim.SetBool("ringoWalk", true);
            anim.SetBool("ringoIdle", false);
            transform.eulerAngles = new Vector3(0f,0f,0f);
        }

        if(Input.GetAxis("Horizontal_P2") == 0f)
        {
            anim.SetBool("ringoWalk", false);
            anim.SetBool("ringoIdle", true);
        }
        
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(!isJumping)
            {
                isJumping = true;
                //srcJump.clip = sfx1;
                //srcJump.Play();
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                //anim.SetBool("jump", true);
            }
            else if (doubleJump)
            {
                //srcJump.clip = sfx1;
                //srcJump.Play();
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                doubleJump = false;
            } 
        }
    }

    void Shoot()
    {
        // Check if enough time has passed since the last shot
        if (Input.GetKeyDown(KeyCode.L) && Time.time - lastShotTime >= shotDelay)
        {
            // Update the last shot time
            lastShotTime = Time.time;

            // Instantiate the projectile at the shoot point and give it the correct direction
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            // Determine the direction of the projectile
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (transform.eulerAngles.y == 0f) // If facing right
            {
                projectileRb.linearVelocity = new Vector2(-10f, 0f);
            }
            else // If facing left
            {
                projectileRb.linearVelocity = new Vector2(10f, 0f);
            }

            Destroy(projectile, 5f);  // Destroy after 5 seconds to avoid clutter
        }
    }

    void FillBar()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("apertou espaço");
            escapeAmount += 1;
        }
        
        escapeAmount = Mathf.Clamp(escapeAmount, 0, 10);
    }

    void StunPlayer()
    {
        //block any action
        isStunned = true;

        //activate escape bar
        escapeBar.SetActive(true);
        bubbled.SetActive(true);

        timesBubbled++;

        //float upwards
        FloatUp();
    }

    void FreePlayer()
    {
        //free player action
        isStunned = false;

        //deactivate escape bar
        escapeBar.SetActive(false);
        bubbled.SetActive(false);

        //Free fall
        rig.gravityScale = 1f;
    }

    void FloatUp()
    {
        //invert gravity for this dude
        rig.gravityScale = -FloatSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the Ground
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player has collided with the Ground!");
            isJumping = false;
        }

        else if (collision.gameObject.CompareTag("Projectile"))
        {
            //Freeze player
            //Debug.Log("Oh não você me acertou");
            StunPlayer();
        }
        else if (collision.gameObject.CompareTag("Spikes"))
        {
            //Pop bubble
            FreePlayer();

            //LOSE COINS
            gc.coinCountP2--;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            gc.coinCountP2++;
            coinSpawner.CoinCollected();
        }
    }
}
