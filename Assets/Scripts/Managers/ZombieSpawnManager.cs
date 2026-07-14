using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] private DifficultyProfile difficultyProfile;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform spawnPointRoot;

    private Transform[] spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 2f;

    [SerializeField] private float overlapRadius = 0.75f;

    [SerializeField] private LayerMask zombieLayer;

    private float spawnTimer;
    private void Awake()
    {
        if (spawnPointRoot == null)
        {
            Debug.LogError("Spawn Point Root is missing.");
            return;
        }

        List<Transform> points = new();

        foreach (Transform child in spawnPointRoot)
        {
            points.Add(child);
        }

        spawnPoints = points.ToArray();
    }
    private void Update()
    {
        if (difficultyProfile == null)
            return;

        DifficultyLevelData level = GetCurrentDifficulty();

        spawnTimer += Time.deltaTime;

        if (spawnTimer < level.spawnInterval)
            return;

        spawnTimer = 0f;

        SpawnWave(level);
    }

    private DifficultyLevelData GetCurrentDifficulty()
    {
        float difficulty =
            DifficultyManager.Instance.Difficulty;

        DifficultyLevelData current =
            difficultyProfile.levels[0];

        foreach (DifficultyLevelData level in difficultyProfile.levels)
        {
            if (difficulty >= level.difficultyThreshold)
                current = level;
            else
                break;
        }

        return current;
    }

    private void SpawnWave(DifficultyLevelData level)
    {
        SpawnZombie(
            PoolType.SmallZombie,
            level.smallZombiesAmount);

        SpawnZombie(
            PoolType.BigZombie,
            level.bigZombiesAmount);
    }

    private void SpawnZombie(
        PoolType poolType,
        int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!TryGetSpawnPosition(out Vector3 position))
                continue;

            ObjectPoolManager.Instance.Get(
                poolType,
                position,
                Quaternion.identity);
        }
    }

    private bool TryGetSpawnPosition(out Vector3 position)
    {
        position = Vector3.zero;

        if (spawnPoints.Length == 0)
            return false;

        const int maxAttempt = 10;

        for (int i = 0; i < maxAttempt; i++)
        {
            Transform point =
                spawnPoints[
                    Random.Range(0, spawnPoints.Length)];

            Vector2 random =
                Random.insideUnitCircle * spawnRadius;

            Vector3 candidate =
                point.position +
                new Vector3(
                    random.x,
                    0f,
                    random.y);

            bool blocked =
                Physics.CheckSphere(
                    candidate,
                    overlapRadius,
                    zombieLayer);

            if (blocked)
                continue;

            position = candidate;

            return true;
        }

        return false;
    }
}