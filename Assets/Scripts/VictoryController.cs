using UnityEngine;
using UnityEngine.UIElements;

public class VictoryController : MonoBehaviour
{

    public GameController gameController;
    public int matchesWonP1 = 0;
    public int matchesWonP2 = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("VictoryController");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(matchesWonP1 == 2)
        {   
            //activate match summary screen

            //wait 5 seconds
            
            //P1 win screen
            
        }
        else if(matchesWonP2 == 2)
        {
            //activate match summary screen

            //wait 5 seconds
            
            //P2 win screen
        }
        else
        {
            //wait 5 seconds and go to next match
        }
    }
}
