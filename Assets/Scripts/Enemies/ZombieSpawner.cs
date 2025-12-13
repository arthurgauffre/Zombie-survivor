using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;

    public float spawnInterval = 5f;
    public float difficultyIncreaseTime = 10f;

    void Start()
    {
        StartCoroutine(SpawnZombies());
        StartCoroutine(IncreaseDifficulty());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseTime);

            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.5f);
        }
    }
}
