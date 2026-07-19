using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    public int coinCountP1;
    public int coinCountP2;
    public int matchesWonP1 = 0;
    public int matchesWonP2 = 0;
    public TextMeshProUGUI[] coinTextP1;
    public TextMeshProUGUI[] coinTextP2;
    public TextMeshProUGUI timerText;
    public GameObject matchSummary;
    public GameObject p1Won;
    public GameObject p2Won;
    public GameObject tie;
    public float matchDuration = 30f;  // Duration of the match in seconds
    private float currentTime;  // The current time left
    private bool setEndMatch = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = matchDuration;  // Initialize the current time with the match duration
        
    }

    /*void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }*/

    // Update is called once per frame
    void Update()
    {
        UpdateText(coinTextP1, coinCountP1);
        UpdateText(coinTextP2, coinCountP2);

        if(coinCountP1 < 0)
        {
            coinCountP1 = 0;
        }
        else if(coinCountP2 < 0)
        {
            coinCountP2 = 0;
        }

        // Update the countdown timer only if there's time remaining
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = $"{seconds:00}";
        }

        else
        {

            // Time has run out
            timerText.text = "0";

            

            // Freeze time
            Time.timeScale = 0;
            
            if(!p1Won.activeSelf && !p2Won.activeSelf && !tie.activeSelf)
            {
                // Show end of match screen
                matchSummary.SetActive(true);
                setEndMatch = true;
            }
            

            // Go to next match
            if(setEndMatch == true && Input.GetKeyDown(KeyCode.Space))
            {
                if(coinCountP1 > coinCountP2)
                {
                    Debug.Log("Player 1 wins!!!");
                    //set P1 victory screen true
                    matchSummary.SetActive(false);
                    p1Won.SetActive(true);
                
                    matchesWonP1++;
                }
                else if(coinCountP1 < coinCountP2)
                {
                    Debug.Log("Player 2 wins!!!");
                    //set P2 victory screen true
                    matchSummary.SetActive(false);
                    p2Won.SetActive(true);
                
                    matchesWonP2++;
                }
                else
                {
                    Debug.Log("It's a tie!!!");
                    matchSummary.SetActive(false);
                    tie.SetActive(true);
                }
            }
        }
    }

    void UpdateText(TextMeshProUGUI[] texts, int value)
    {
        string valueString = value.ToString();
      
        foreach (TextMeshProUGUI text in texts)
        {
            text.text = valueString;
        }
    }

    public int GetCoins(PlayerController.PlayerType player)
    {
        if(player == PlayerController.PlayerType.Player1)
            return coinCountP1;
        
        else
            return coinCountP2;
    }

    public void AddCoin(PlayerController.PlayerType player, int amount = 1)
    {
        if (player == PlayerController.PlayerType.Player1)
            coinCountP1 += amount;
        else
            coinCountP2 += amount;
    }
    
    public void RemoveCoin(PlayerController.PlayerType player, int amount = 1)
    {
        if (player == PlayerController.PlayerType.Player1)
            coinCountP1 = Mathf.Max(0, coinCountP1 - amount);
        else
            coinCountP2 = Mathf.Max(0, coinCountP2 - amount);
    }
}
