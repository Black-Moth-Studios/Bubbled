using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    public int coinCountP1;
    public int coinCountP2;
    public int matchesWonP1 = 0;
    public int matchesWonP2 = 0;
    public TextMeshProUGUI coinTextP1;
    public TextMeshProUGUI coinTextP2;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinP1Sum;
    public TextMeshProUGUI coinP2Sum;
    public TextMeshProUGUI coinP1Vic;
    public TextMeshProUGUI coinP2Vic;
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
        coinTextP1.text = coinCountP1.ToString();
        coinTextP2.text = coinCountP2.ToString();

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
            currentTime -= Time.deltaTime;  // Decrease the time by the time passed since last frame
            timerText.text = Mathf.Ceil(currentTime).ToString();  // Update the UI Text to show the time remaining
        }
        else
        {
            FillSummary();

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
                if(coinCountP1 >= coinCountP2)
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



                //string currentSceneName = SceneManager.GetActiveScene().name;
                
                //SceneManager.LoadScene(currentSceneName);
                //Time.timeScale = 1;
            }
        }
    }

    void FillSummary()
    {
        coinP1Sum.text = coinCountP1.ToString();

        coinP2Sum.text = coinCountP2.ToString();

        coinP1Vic.text = coinCountP1.ToString();

        coinP2Vic.text = coinCountP2.ToString();
    }
}
