using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Timing")]
    public float spawnInterval = 5f;
    public float timeBetweenRounds = 5f;

    [Header("Round Settings")]
    public int initialZombiesPerRound = 3;
    public int extraPerRound = 2;

    // reference to round system
    private RoundSystem roundSystem;

    // track spawned zombies for the current round
    private List<GameObject> activeZombies = new List<GameObject>();
    private bool isSpawning = false;

    void Start()
    {
        if (zombiePrefab == null)
        {
            Debug.LogError("ZombieSpawner: zombiePrefab not assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("ZombieSpawner: no spawnPoints assigned.");
            return;
        }

        roundSystem = FindObjectOfType<RoundSystem>();
        if (roundSystem != null)
        {
            roundSystem.OnRoundStarted += HandleRoundStarted;
            // If RoundSystem already started and invoked before subscription, start manually
            // but RoundSystem invokes OnRoundStarted in Start(), so this should cover initial start.
            // Ensure we start the current round if the event already fired before we subscribed
            HandleRoundStarted(roundSystem.currentRound);
        }
        else
        {
            Debug.LogWarning("ZombieSpawner: No RoundSystem found. Spawner will not start until a round is triggered.");
        }
    }

    private void OnDestroy()
    {
        if (roundSystem != null)
            roundSystem.OnRoundStarted -= HandleRoundStarted;
    }

    private void HandleRoundStarted(int round)
    {
        // avoid starting the same round twice if we're already spawning
        if (isSpawning) return;

        Debug.Log($"ZombieSpawner: HandleRoundStarted called for round {round}");

        // start spawning for this round
        StartCoroutine(SpawnRound(round));
    }

    private IEnumerator SpawnRound(int round)
    {
        isSpawning = true;
        int zombiesThisRound = Mathf.Max(1, initialZombiesPerRound + (round - 1) * extraPerRound);
        Debug.Log($"ZombieSpawner: Starting round {round}, spawning {zombiesThisRound} zombies");

        activeZombies.Clear();

        for (int i = 0; i < zombiesThisRound; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject z = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
            if (z != null) activeZombies.Add(z);
            Debug.Log($"ZombieSpawner: Spawned zombie #{i+1} of {zombiesThisRound} at {spawnPoint.position}");

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log($"ZombieSpawner: Round {round} spawned. Waiting for all zombies to be killed...");

        // wait until all spawned zombies are destroyed
        while (activeZombies.Exists(g => g != null))
        {
            activeZombies.RemoveAll(g => g == null);
            yield return null;
        }

        Debug.Log($"ZombieSpawner: Round {round} complete (all zombies killed)");

        // wait a short delay before starting next round (gives player breather)
        if (timeBetweenRounds > 0f)
        {
            Debug.Log($"ZombieSpawner: Waiting {timeBetweenRounds}s before next round");
            yield return new WaitForSeconds(timeBetweenRounds);
        }

        // allow starting new spawns
        isSpawning = false;

        // notify RoundSystem to advance
        if (roundSystem != null)
            roundSystem.NextRound();
    }
}
