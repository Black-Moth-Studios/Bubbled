using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;  // Reference to the coin prefab
    private bool isCoinCollected = false;  // To track if the coin has been collected

    public float spawnRadiusX = 5f;  // Width of the spawn area (along the X axis)
    public float spawnRadiusY = 5f;  // Height of the spawn area (along the Y axis)

    public int maxCoinAmount;
    public int curCoinAmount = 0;
    public float coinTime;

    void Start()
    {
        Time.timeScale = 1;
        // Initially spawn the first coin
        //SpawnCoin();
    }

    void Update()
    {
        // Start the coroutine to spawn a new coin after the delay
        StartCoroutine(WaitAndSpawnCoin());
        isCoinCollected = false;  // Reset to prevent spawning multiple times
    }

    void SpawnCoin()
    {
        
        // Generate a random position within a rectangular area around the spawner
        float randomX = Random.Range(-spawnRadiusX, spawnRadiusX);  // Random X position
        float randomY = Random.Range(-spawnRadiusY, spawnRadiusY);  // Random Y position
        Vector3 randomPosition = new Vector3(randomX, randomY, transform.position.z);  // Adjust Z to match spawner's position (since this is a 2D game)

        // Instantiate the coin at the random position
        Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        curCoinAmount++;
        
    }

    // Coroutine to handle waiting for 2 seconds and then spawning the next coin
    IEnumerator WaitAndSpawnCoin()
    {
        
        // Wait for 2 seconds before spawning the next coin
        yield return new WaitForSeconds(coinTime);

        if(curCoinAmount < maxCoinAmount)
        {
            // Spawn the next coin
            SpawnCoin();
        }
    }

    // Call this method when the coin is collected by the player
    public void CoinCollected()
    {
        // Mark the coin as collected to trigger the spawn of the next one
        isCoinCollected = true;
        curCoinAmount--;
    }
}