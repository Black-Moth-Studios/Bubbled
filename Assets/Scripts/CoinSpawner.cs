using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;  // Reference to the coin prefab

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.2f;

    public float spawnRadiusX = 5f;  // Width of the spawn area (along the X axis)
    public float spawnRadiusY = 5f;  // Height of the spawn area (along the Y axis)

    public int maxCoinAmount;
    public int curCoinAmount = 0;
    public float coinTime;

    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitAndSpawnCoin());
        // Initially spawn the first coin
        //SpawnCoin();
    }

    void Update()
    {
        // Start the coroutine to spawn a new coin after the delay
    }

    void SpawnCoin()
    {
        Vector3 randomPosition;
        int attempts = 20;

        do
        {
            float randomX = Random.Range(-spawnRadiusX, spawnRadiusX);
            float randomY = Random.Range(-spawnRadiusY, spawnRadiusY);

            randomPosition = new Vector3(randomX, randomY, 0);

            attempts--;
        } while (
            Physics2D.OverlapCircle(randomPosition, checkRadius, groundLayer)
            && attempts > 0
        );

        Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        curCoinAmount++;
    }

    // Coroutine to handle waiting for 2 seconds and then spawning the next coin
    IEnumerator WaitAndSpawnCoin()
    {
        while (true)
        {
            yield return new WaitForSeconds(coinTime);

            if (curCoinAmount < maxCoinAmount)
            {
                SpawnCoin();
            }
        }
    }

    // Call this method when the coin is collected by the player
    public void CoinCollected()
    {
        // Mark the coin as collected to trigger the spawn of the next one
        curCoinAmount--;
    }
}