using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerType {Player1, Player2}

    [Header("Controls")]
    private PlayerInputActions input;
    public InputActionMap playerMap;
    public bool invertShootDirection;

    [Header("Atributes")]
    public PlayerType player;
    public float Speed;
    public float FloatSpeed;
    public float JumpForce;
    public float escapeAmount = 0f;
    private int timesBubbled = 0;
    public float shotDelay; 
    private float lastShotTime = 0f;
    private bool isStunned = false;
    private bool hasBeenRobbed = false;

    [Header("GameObjects")]
    public GameObject escapeBar;
    public GameObject escapeBarFill;
    public GameObject projectilePrefab;
    public GameObject bubbled;
    public Transform shootPoint;
    public TextMeshProUGUI[] bubbledCount;
    public Animator anim;
    private Rigidbody2D rig;
    public GameController gc;
    public CoinSpawner coinSpawner;


    void Awake()
    {
        input = new PlayerInputActions();
        playerMap = player == PlayerType.Player1 ? input.Player1.Get() : input.Player2.Get();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText(bubbledCount, timesBubbled);

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

        UpdateEscapeBar();
    }

    public void Move(){
        float horizontal = playerMap.FindAction("Move").ReadValue<float>();

        rig.linearVelocity = new Vector2(horizontal * Speed, rig.linearVelocity.y);

        anim.SetBool("Walk", horizontal != 0);

        if(horizontal > 0)
        {
            if(player == PlayerType.Player1)
            {
                transform.eulerAngles = Vector3.zero;
            }

            else
            {
                transform.eulerAngles = new Vector3(0,180,0);
            }
        }
        else if(horizontal < 0)
        {
            if(player == PlayerType.Player1)
            {
                transform.eulerAngles = new Vector3(0,180,0);
            }

            else
            {
                transform.eulerAngles = Vector3.zero;
            }
        }
    }

    void Jump()
    {
        if(playerMap.FindAction("Jump").WasPressedThisFrame())
        {
            if(rig.linearVelocity.y > 0)
            {
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0f);
            }
            
            rig.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    void Shoot()
    {
        if(playerMap.FindAction("Shoot").WasPressedThisFrame() && Time.time - lastShotTime >= shotDelay)
        {
            lastShotTime = Time.time;

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();


            float direction;


            if(invertShootDirection)
            {
                if(player == PlayerType.Player1)
                {
                    direction = transform.eulerAngles.y == 0 ? -1 : 1;
                }
                
                else
                {
                    direction = transform.eulerAngles.y == 0 ? 1 : -1;
                }
            }

            else
            {
                if(player == PlayerType.Player1)
                {
                    direction = transform.eulerAngles.y == 0 ? 1 : -1;
                }
                
                else
                {
                    direction = transform.eulerAngles.y == 0 ? -1 : 1;
                }
            }
            
            rb.linearVelocity = new Vector2(direction * 10, 0);

            Destroy(projectile,5);
        }
    
    }
    
    void UpdateEscapeBar(){
        escapeAmount = Mathf.Clamp(escapeAmount, 0, 10);

        escapeBarFill.transform.localScale = new Vector2(0.05f, escapeAmount/ 10f);

        if (escapeAmount >= 10)
        {
            FreePlayer();
            escapeAmount = 0;
        }
    }

    void FillBar()
    {
        if (playerMap.FindAction("Escape").WasPressedThisFrame())
        {
            escapeAmount += 1;
        }
        
        escapeAmount = Mathf.Clamp(escapeAmount, 0, 10);
    }

    void StunPlayer()
    {
        isStunned = true;
        hasBeenRobbed = false;

        escapeBar.SetActive(true);
        bubbled.SetActive(true);

        timesBubbled++;

        FloatUp();
    }

    void FreePlayer()
    {
        isStunned = false;
        hasBeenRobbed = false;

        escapeBar.SetActive(false);
        bubbled.SetActive(false);

        rig.gravityScale = 1f;
    }

    public void StealCoin(PlayerController thief)
    {
        if(!isStunned || hasBeenRobbed)
            return;
        
        hasBeenRobbed = true;

        if(player == PlayerType.Player1)
        {
            if(gc.coinCountP1 > 0)
            {
                gc.coinCountP1--;
                gc.coinCountP2++;
            }
        }

        else
        {
            if(gc.coinCountP2 > 0)
            {
                gc.coinCountP2--;
                gc.coinCountP1++;
            }
        }

        FreePlayer();
    }

    void FloatUp()
    {
        //invert gravity for this dude
        rig.gravityScale = -FloatSpeed;
    }

    void UpdateText(TextMeshProUGUI[] texts, int value)
    {
        string valueString = value.ToString();
      
        foreach (TextMeshProUGUI text in texts)
        {
            text.text = valueString;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController otherPlayer = collision.gameObject.GetComponent<PlayerController>();

            if (otherPlayer != null)
            {
                otherPlayer.StealCoin(this);
            }
        }

        if (collision.gameObject.CompareTag("Projectile"))
        {
            StunPlayer();
        }

        else if (collision.gameObject.CompareTag("Spikes"))
        {   
            if (isStunned)
            {
                FreePlayer();

                if(player == PlayerType.Player1)
                {
                    gc.coinCountP1--;
                }
                else
                {
                    gc.coinCountP2--;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);

            if(player == PlayerType.Player1)
            {
                gc.coinCountP1++;
            }
            else
            {
                gc.coinCountP2++;
            }
            
            coinSpawner.CoinCollected();
        }
    }
}
